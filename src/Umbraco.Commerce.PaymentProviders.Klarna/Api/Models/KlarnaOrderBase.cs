using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public abstract class KlarnaOrderBase
    {
        [JsonPropertyName("merchant_reference1")]
        public string MerchantReference1 { get; set; }

        [JsonPropertyName("merchant_reference2")]
        public string MerchantReference2 { get; set; }

        [JsonPropertyName("purchase_country")]
        public string PurchaseCountry { get; set; }

        [JsonPropertyName("purchase_currency")]
        public string PurchaseCurrency { get; set; }

        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("order_lines")]
        public List<KlarnaOrderLine> OrderLines { get; set; }

        [JsonPropertyName("order_amount")]
        public int OrderAmount { get; set; }

        [JsonPropertyName("order_tax_amount")]
        public int? OrderTaxAmount { get; set; }

        [JsonPropertyName("billing_address")]
        public KlarnaAddress BillingAddress { get; set; }

        [JsonPropertyName("shipping_address")]
        public KlarnaAddress ShippingAddress { get; set; }

        public KlarnaOrderBase()
        {
            OrderLines = new List<KlarnaOrderLine>();
        }
    }
}
