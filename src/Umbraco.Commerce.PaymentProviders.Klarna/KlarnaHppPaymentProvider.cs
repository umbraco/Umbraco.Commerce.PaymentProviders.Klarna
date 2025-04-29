using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Umbraco.Commerce.Common.Logging;
using Umbraco.Commerce.Core.Api;
using Umbraco.Commerce.Core.Models;
using Umbraco.Commerce.Core.PaymentProviders;
using Umbraco.Commerce.Extensions;
using Umbraco.Commerce.PaymentProviders.Klarna.Api;
using Umbraco.Commerce.PaymentProviders.Klarna.Api.Models;

namespace Umbraco.Commerce.PaymentProviders.Klarna
{
    [PaymentProvider("klarna-hpp")]
    public class KlarnaHppPaymentProvider : KlarnaPaymentProviderBase<KlarnaHppSettings>
    {
        private readonly ILogger<KlarnaHppPaymentProvider> _logger;

        public KlarnaHppPaymentProvider(
            UmbracoCommerceContext ctx,
            ILogger<KlarnaHppPaymentProvider> logger)
            : base(ctx)
        {
            _logger = logger;
        }

        public override bool CanFetchPaymentStatus => true;
        public override bool CanCancelPayments => true;
        public override bool CanCapturePayments => true;
        public override bool CanRefundPayments => true;
        public override bool CanPartiallyRefundPayments => true;

        public override IEnumerable<TransactionMetaDataDefinition> TransactionMetaDataDefinitions =>
        [
            new TransactionMetaDataDefinition("klarnaSessionId"),
            new TransactionMetaDataDefinition("klarnaOrderId"),
            new TransactionMetaDataDefinition("klarnaReference"),
        ];

        public override string GetCancelUrl(PaymentProviderContext<KlarnaHppSettings> ctx)
        {
            ctx.Settings.MustNotBeNull("ctx.Settings");
            ctx.Settings.CancelUrl.MustNotBeNull("ctx.Settings.CancelUrl");

            var cancelUrl = ctx.Settings.CancelUrl;

            if (ctx.HttpContext.Request != null)
            {
                IQueryCollection qs = ctx.HttpContext.Request.Query;

                StringValues reason = qs["reason"];

                cancelUrl = AppendQueryStringParam(cancelUrl, "reason", reason);

                if (!string.IsNullOrWhiteSpace(ctx.Settings.ErrorUrl) && (reason == "failure" || reason == "error"))
                {
                    return AppendQueryStringParam(ctx.Settings.ErrorUrl, "reason", reason);
                }
            }

            return cancelUrl;
        }

        public override async Task<PaymentFormResult> GenerateFormAsync(PaymentProviderContext<KlarnaHppSettings> ctx, CancellationToken cancellationToken = default)
        {
            var klarnaSecretToken = Guid.NewGuid().ToString("N");

            var clientConfig = GetKlarnaClientConfig(ctx.Settings);
            var client = new KlarnaClient(clientConfig);

            // Get currency information
            var billingCountry = await Context.Services.CountryService.GetCountryAsync(ctx.Order.PaymentInfo.CountryId.Value);
            var billingCountryCode = billingCountry.Code.ToUpperInvariant();

            // Ensure billing country has valid ISO 3166 code
            var iso3166Countries = await Context.Services.CountryService.GetIso3166CountryRegionsAsync();
            if (iso3166Countries.All(x => x.Code != billingCountryCode))
            {
                throw new Exception("Country must be a valid ISO 3166 billing country code: " + billingCountry.Name);
            }

            var currency = await Context.Services.CurrencyService.GetCurrencyAsync(ctx.Order.CurrencyId);
            var currencyCode = currency.Code.ToUpperInvariant();

            // Ensure currency has valid ISO 4217 code
            if (!Iso4217.CurrencyCodes.ContainsKey(currencyCode))
            {
                throw new Exception("Currency must be a valid ISO 4217 currency code: " + currency.Name);
            }

            // Prepair ctx.Order lines
            // NB: We add ctx.Order lines without any discounts applied as we'll then add
            // one global discount amount at the end. This is just the easiest way to
            // allow everything to add up and successfully validate at the Klarna end.
            var orderLines = ctx.Order.OrderLines.Select(orderLine => new KlarnaOrderLine
            {
                Reference = orderLine.Sku,
                Name = orderLine.Name,
                Type = !string.IsNullOrWhiteSpace(ctx.Settings.ProductTypePropertyAlias) && orderLine.Properties.ContainsKey(ctx.Settings.ProductTypePropertyAlias)
                    ? orderLine.Properties[ctx.Settings.ProductTypePropertyAlias]?.Value
                    : null,
                TaxRate = (int)(orderLine.TaxRate.Value * 10000),
                UnitPrice = (int)AmountToMinorUnits(orderLine.UnitPrice.WithoutAdjustments.WithTax),
                Quantity = (int)orderLine.Quantity,
                TotalAmount = (int)AmountToMinorUnits(orderLine.TotalPrice.WithoutAdjustments.WithTax),
                TotalTaxAmount = (int)AmountToMinorUnits(orderLine.TotalPrice.WithoutAdjustments.Tax)
            }).ToList();

            // Add shipping method fee ctx.Orderline
            if (ctx.Order.ShippingInfo.ShippingMethodId.HasValue && ctx.Order.ShippingInfo.TotalPrice.WithoutAdjustments.WithTax > 0)
            {
                var shippingMethod = await Context.Services.ShippingMethodService.GetShippingMethodAsync(ctx.Order.ShippingInfo.ShippingMethodId.Value);

                orderLines.Add(new KlarnaOrderLine
                {
                    Reference = shippingMethod.Sku,
                    Name = shippingMethod.Name + " Fee",
                    Type = KlarnaOrderLine.Types.SHIPPING_FEE,
                    TaxRate = (int)(ctx.Order.ShippingInfo.TaxRate * 10000),
                    UnitPrice = (int)AmountToMinorUnits(ctx.Order.ShippingInfo.TotalPrice.WithoutAdjustments.WithTax),
                    Quantity = 1,
                    TotalAmount = (int)AmountToMinorUnits(ctx.Order.ShippingInfo.TotalPrice.WithoutAdjustments.WithTax),
                    TotalTaxAmount = (int)AmountToMinorUnits(ctx.Order.ShippingInfo.TotalPrice.WithoutAdjustments.Tax),
                });
            }

            // Add payment method fee (as surcharge) ctx.Orderline
            if (ctx.Order.PaymentInfo.TotalPrice.Value.WithTax > 0)
            {
                var paymentMethod = await Context.Services.PaymentMethodService.GetPaymentMethodAsync(ctx.Order.PaymentInfo.PaymentMethodId.Value);

                orderLines.Add(new KlarnaOrderLine
                {
                    Reference = paymentMethod.Sku,
                    Name = paymentMethod.Name + " Fee",
                    Type = KlarnaOrderLine.Types.SURCHARGE,
                    TaxRate = (int)(ctx.Order.PaymentInfo.TaxRate * 10000),
                    UnitPrice = (int)AmountToMinorUnits(ctx.Order.PaymentInfo.TotalPrice.WithoutAdjustments.WithTax),
                    Quantity = 1,
                    TotalAmount = (int)AmountToMinorUnits(ctx.Order.PaymentInfo.TotalPrice.WithoutAdjustments.WithTax),
                    TotalTaxAmount = (int)AmountToMinorUnits(ctx.Order.PaymentInfo.TotalPrice.WithoutAdjustments.Tax),
                });
            }

            // Add any discounts
            if (ctx.Order.TotalPrice.TotalAdjustment < 0)
            {
                orderLines.Add(new KlarnaOrderLine
                {
                    Reference = "DISCOUNT",
                    Name = "Discounts",
                    Type = KlarnaOrderLine.Types.DISCOUNT,
                    TaxRate = (int)(ctx.Order.TaxRate * 10000),
                    UnitPrice = 0,
                    Quantity = 1,
                    TotalDiscountAmount = (int)AmountToMinorUnits(ctx.Order.TotalPrice.TotalAdjustment.WithTax) * -1,
                    TotalAmount = (int)AmountToMinorUnits(ctx.Order.TotalPrice.TotalAdjustment.WithTax),
                    TotalTaxAmount = (int)AmountToMinorUnits(ctx.Order.TotalPrice.TotalAdjustment.Tax),
                });
            }
            else if (ctx.Order.TotalPrice.TotalAdjustment > 0)
            {
                orderLines.Add(new KlarnaOrderLine
                {
                    Reference = "FEE",
                    Name = "Additional Fees",
                    Type = KlarnaOrderLine.Types.SURCHARGE,
                    TaxRate = (int)(ctx.Order.TaxRate * 10000),
                    UnitPrice = 0,
                    Quantity = 1,
                    TotalAmount = (int)AmountToMinorUnits(ctx.Order.TotalPrice.TotalAdjustment.WithTax),
                    TotalTaxAmount = (int)AmountToMinorUnits(ctx.Order.TotalPrice.TotalAdjustment.Tax),
                });
            }

            // Add gift cards
            if (ctx.Order.TransactionAmount.Adjustment.Value < 0)
            {
                foreach (GiftCardAdjustment giftcard in ctx.Order.TransactionAmount.Adjustments)
                {
                    orderLines.Add(new KlarnaOrderLine
                    {
                        Reference = "Gift Card " + giftcard.GiftCardCode,
                        Name = "Discounts",
                        Type = KlarnaOrderLine.Types.GIFT_CARD,
                        TaxRate = (int)(ctx.Order.TaxRate * 10000),
                        UnitPrice = 0,
                        Quantity = 1,
                        TotalDiscountAmount = (int)AmountToMinorUnits(giftcard.Amount) * -1,
                        TotalAmount = (int)AmountToMinorUnits(giftcard.Amount),
                        TotalTaxAmount = 0,
                    });
                }
            }

            // Create a merchant session
            var resp1 = await client.CreateMerchantSessionAsync(
                new KlarnaCreateMerchantSessionOptions
                {
                    MerchantReference1 = ctx.Order.OrderNumber,
                    PurchaseCountry = billingCountryCode,
                    PurchaseCurrency = currencyCode,
                    Locale = ctx.Order.LanguageIsoCode, // TODO: Validate?

                    OrderLines = orderLines,
                    OrderAmount = (int)AmountToMinorUnits(ctx.Order.TransactionAmount.Value),
                    OrderTaxAmount = (int)AmountToMinorUnits(ctx.Order.TotalPrice.Value.Tax),

                    BillingAddress = new KlarnaAddress
                    {
                        GivenName = ctx.Order.CustomerInfo.FirstName,
                        FamilyName = ctx.Order.CustomerInfo.LastName,
                        Email = ctx.Order.CustomerInfo.Email,
                        StreetAddress = !string.IsNullOrWhiteSpace(ctx.Settings.BillingAddressLine1PropertyAlias)
                            ? ctx.Order.Properties[ctx.Settings.BillingAddressLine1PropertyAlias]?.Value : null,
                        StreetAddress2 = !string.IsNullOrWhiteSpace(ctx.Settings.BillingAddressLine2PropertyAlias)
                            ? ctx.Order.Properties[ctx.Settings.BillingAddressLine2PropertyAlias]?.Value : null,
                        City = !string.IsNullOrWhiteSpace(ctx.Settings.BillingAddressCityPropertyAlias)
                            ? ctx.Order.Properties[ctx.Settings.BillingAddressCityPropertyAlias]?.Value : null,
                        Region = !string.IsNullOrWhiteSpace(ctx.Settings.BillingAddressStatePropertyAlias)
                            ? ctx.Order.Properties[ctx.Settings.BillingAddressStatePropertyAlias]?.Value : null,
                        PostalCode = !string.IsNullOrWhiteSpace(ctx.Settings.BillingAddressZipCodePropertyAlias)
                            ? ctx.Order.Properties[ctx.Settings.BillingAddressZipCodePropertyAlias]?.Value : null,
                        Country = billingCountryCode
                    }
                },
                cancellationToken).ConfigureAwait(false);

            // Create a HPP session
            var resp2 = await client.CreateHppSessionAsync(
                new KlarnaCreateHppSessionOptions
                {
                    PaymentSessionUrl = $"{clientConfig.BaseUrl}/payments/v1/sessions/{resp1.SessionId}",
                    Options = new KlarnaHppOptions
                    {
                        PlaceOrderMode = ctx.Settings.Capture
                            ? KlarnaHppOptions.PlaceOrderModes.CAPTURE_ORDER
                            : KlarnaHppOptions.PlaceOrderModes.PLACE_ORDER,
                        LogoUrl = !string.IsNullOrWhiteSpace(ctx.Settings.PaymentPageLogoUrl)
                            ? ctx.Settings.PaymentPageLogoUrl.Trim()
                            : null,
                        PageTitle = !string.IsNullOrWhiteSpace(ctx.Settings.PaymentPagePageTitle)
                            ? ctx.Settings.PaymentPagePageTitle.Trim()
                            : null,
                        PaymentMethodCategories = !string.IsNullOrWhiteSpace(ctx.Settings.PaymentMethodCategories)
                            ? ctx.Settings.PaymentMethodCategories.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                .Select(x => x.Trim())
                                .Where(x => !string.IsNullOrWhiteSpace(x))
                                .ToArray()
                            : null,
                        PaymentMethodCategory = !string.IsNullOrWhiteSpace(ctx.Settings.PaymentMethodCategory)
                            ? ctx.Settings.PaymentMethodCategory.Trim()
                            : null,
                        PaymentFallback = ctx.Settings.EnableFallbacks
                    },
                    MerchantUrls = new KlarnaHppMerchantUrls
                    {
                        Success = ctx.Urls.ContinueUrl,
                        Cancel = AppendQueryString(ctx.Urls.CancelUrl, "reason=cancel"),
                        Back = AppendQueryString(ctx.Urls.CancelUrl, "reason=back"),
                        Failure = AppendQueryString(ctx.Urls.CancelUrl, "reason=failure"),
                        Error = AppendQueryString(ctx.Urls.CancelUrl, "reason=error"),
                        StatusUpdate = AppendQueryString(ctx.Urls.CallbackUrl, "sid={{session_id}}&token=" + klarnaSecretToken),
                    }
                },
                cancellationToken).ConfigureAwait(false);

            return new PaymentFormResult()
            {
                Form = new PaymentForm(resp2.RedirectUrl, PaymentFormMethod.Get),
                MetaData = new Dictionary<string, string>
                {
                    { "klarnaSessionId", resp2.SessionId },
                    { "klarnaSecretToken", klarnaSecretToken }
                }
            };
        }

        public override async Task<CallbackResult> ProcessCallbackAsync(PaymentProviderContext<KlarnaHppSettings> ctx, CancellationToken cancellationToken = default)
        {
            IQueryCollection qs = ctx.HttpContext.Request.Query;
            StringValues sessionId = qs["sid"];
            StringValues token = qs["token"];

            if (!string.IsNullOrWhiteSpace(sessionId) && ctx.Order.Properties["klarnaSessionId"] == sessionId
                && !string.IsNullOrWhiteSpace(token) && ctx.Order.Properties["klarnaSecretToken"] == token)
            {
                var clientConfig = GetKlarnaClientConfig(ctx.Settings);
                var client = new KlarnaClient(clientConfig);

                using (Stream stream = ctx.HttpContext.Request.Body)
                {
                    KlarnaSessionEvent evt = client.ParseSessionEvent(stream);
                    if (evt != null && evt.Session.Status == KlarnaSession.Statuses.COMPLETED)
                    {
                        KlarnaOrder klarnaOrder = await client.GetOrderAsync(evt.Session.OrderId, cancellationToken).ConfigureAwait(false);

                        return new CallbackResult
                        {
                            TransactionInfo = new TransactionInfo
                            {
                                AmountAuthorized = AmountFromMinorUnits(klarnaOrder.OriginalOrderAmount),
                                TransactionFee = 0m,
                                TransactionId = klarnaOrder.OrderId,
                                PaymentStatus = GetPaymentStatus(klarnaOrder)
                            },
                            MetaData = new Dictionary<string, string>
                            {
                                { "klarnaOrderId", evt.Session.OrderId },
                                { "klarnaReference", evt.Session.KlarnaReference },
                            },
                        };
                    }
                }
            }

            return CallbackResult.Ok();
        }

        public override async Task<ApiResult> FetchPaymentStatusAsync(PaymentProviderContext<KlarnaHppSettings> ctx, CancellationToken cancellationToken = default)
        {
            try
            {
                var orderId = ctx.Order.TransactionInfo.TransactionId;

                var clientConfig = GetKlarnaClientConfig(ctx.Settings);
                var client = new KlarnaClient(clientConfig);

                var klarnaOrder = await client.GetOrderAsync(orderId, cancellationToken).ConfigureAwait(false);
                if (klarnaOrder != null)
                {
                    return new ApiResult
                    {
                        TransactionInfo = new TransactionInfoUpdate
                        {
                            TransactionId = klarnaOrder.OrderId,
                            PaymentStatus = GetPaymentStatus(klarnaOrder)
                        }
                    };
                }

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error fetching Klarna payment status for ctx.Order {OrderNumber}", ctx.Order.OrderNumber);
            }

            return ApiResult.Empty;
        }

        public override async Task<ApiResult> CapturePaymentAsync(PaymentProviderContext<KlarnaHppSettings> ctx, CancellationToken cancellationToken = default)
        {
            try
            {
                var orderId = ctx.Order.TransactionInfo.TransactionId;

                var clientConfig = GetKlarnaClientConfig(ctx.Settings);
                var client = new KlarnaClient(clientConfig);

                await client.CaptureOrderAsync(orderId, new KlarnaCaptureOptions
                {
                    Description = $"Capture Order {ctx.Order.OrderNumber}",
                    CapturedAmount = (int)AmountToMinorUnits(ctx.Order.TransactionInfo.AmountAuthorized.Value)
                }, cancellationToken).ConfigureAwait(false);

                return new ApiResult
                {
                    TransactionInfo = new TransactionInfoUpdate
                    {
                        TransactionId = orderId,
                        PaymentStatus = PaymentStatus.Captured
                    }
                };

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error capturing Klarna payment for ctx.Order {OrderNumber}", ctx.Order.OrderNumber);
            }

            return ApiResult.Empty;
        }

        // TODO Dinh
        [Obsolete("Will be removed in v17. Use the overload that takes an order refund request instead.")]
        public override async Task<ApiResult?> RefundPaymentAsync(PaymentProviderContext<KlarnaHppSettings> context, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(context);

            StoreReadOnly store = await Context.Services.StoreService.GetStoreAsync(context.Order.StoreId);
            Amount refundAmount = store.CanRefundTransactionFee ? context.Order.TransactionInfo.AmountAuthorized + context.Order.TransactionInfo.TransactionFee : context.Order.TransactionInfo.AmountAuthorized;
            return await this.RefundPaymentAsync(
                context,
                new PaymentProviderOrderRefundRequest
                {
                    RefundAmount = refundAmount,
                    Orderlines = [],
                },
                cancellationToken);

        }

        public override async Task<ApiResult?> RefundPaymentAsync(
            PaymentProviderContext<KlarnaHppSettings> context,
            PaymentProviderOrderRefundRequest refundRequest,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(refundRequest);

            try
            {
                string orderId = context.Order.TransactionInfo.TransactionId;

                KlarnaClientConfig clientConfig = GetKlarnaClientConfig(context.Settings);
                KlarnaClient client = new(clientConfig);

                await client.RefundOrderAsync(
                    orderId,
                    new KlarnaRefundOptions
                    {
                        Description = $"Refund Order {context.Order.OrderNumber}",
                        RefundAmount = (int)AmountToMinorUnits(refundRequest.RefundAmount),
                    },
                    cancellationToken).ConfigureAwait(false);

                return new ApiResult
                {
                    TransactionInfo = new TransactionInfoUpdate
                    {
                        TransactionId = orderId,
                        PaymentStatus = PaymentStatus.Refunded
                    },
                };

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error refunding Klarna payment for ctx.Order {OrderNumber}", context.Order.OrderNumber);
            }

            return ApiResult.Empty;
        }


        public override async Task<ApiResult> CancelPaymentAsync(PaymentProviderContext<KlarnaHppSettings> ctx, CancellationToken cancellationToken = default)
        {
            try
            {
                var orderId = ctx.Order.TransactionInfo.TransactionId;

                var clientConfig = GetKlarnaClientConfig(ctx.Settings);
                var client = new KlarnaClient(clientConfig);

                await client.CancelOrderAsync(orderId, cancellationToken).ConfigureAwait(false);

                return new ApiResult
                {
                    TransactionInfo = new TransactionInfoUpdate
                    {
                        TransactionId = orderId,
                        PaymentStatus = PaymentStatus.Cancelled
                    }
                };

            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error canceling Klarna payment for ctx.Order {OrderNumber}", ctx.Order.OrderNumber);
            }

            return ApiResult.Empty;
        }
    }
}
