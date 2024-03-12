using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Flurl.Http;
using Umbraco.Commerce.PaymentProviders.Klarna.Api.Models;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api
{
    public class KlarnaClient
    {
        public const string EuLiveApiUrl = "https://api.klarna.com";
        public const string NaLiveApiUrl = "https://api-na.klarna.com";
        public const string OcLiveApiUrl = "https://api-oc.klarna.com";

        public const string EuPlaygroundApiUrl = "https://api.playground.klarna.com";
        public const string NaPlaygroundApiUrl = "https://api-na.playground.klarna.com";
        public const string OcPlaygroundApiUrl = "https://api-oc.playground.klarna.com";

        private readonly KlarnaClientConfig _config;

        public KlarnaClient(KlarnaClientConfig config)
        {
            _config = config;
        }

        public async Task<KlarnaMerchantSession> CreateMerchantSessionAsync(KlarnaCreateMerchantSessionOptions opts, CancellationToken cancellationToken = default)
        {
            return await RequestAsync("/payments/v1/sessions", async (req, ct) => await req
                .PostJsonAsync(opts, cancellationToken: ct)
                .ReceiveJson<KlarnaMerchantSession>().ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task<KlarnaHppSession> CreateHppSessionAsync(KlarnaCreateHppSessionOptions opts, CancellationToken cancellationToken = default)
        {
            return await RequestAsync("/hpp/v1/sessions", async (req, ct) => await req
                .PostJsonAsync(opts, cancellationToken: ct)
                .ReceiveJson<KlarnaHppSession>().ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task<KlarnaOrder> GetOrderAsync(string orderId, CancellationToken cancellationToken = default)
        {
            return await RequestAsync($"/ordermanagement/v1/orders/{orderId}", async (req, ct) => await req
                .GetAsync(cancellationToken: ct)
                .ReceiveJson<KlarnaOrder>().ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task CancelOrderAsync(string orderId, CancellationToken cancellationToken = default)
        {
            await RequestAsync($"/ordermanagement/v1/orders/{orderId}/cancel", async (req, ct) => await req
                .PostAsync(null, cancellationToken: ct).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task CaptureOrderAsync(string orderId, KlarnaCaptureOptions opts, CancellationToken cancellationToken = default)
        {
            await RequestAsync($"/ordermanagement/v1/orders/{orderId}/captures", async (req, ct) => await req
                .PostJsonAsync(opts, cancellationToken: ct).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task RefundOrderAsync(string orderId, KlarnaRefundOptions opts, CancellationToken cancellationToken = default)
        {
            await RequestAsync($"/ordermanagement/v1/orders/{orderId}/refunds", async (req, ct) => await req
                .PostJsonAsync(opts, cancellationToken: ct).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
        }

        public KlarnaSessionEvent ParseSessionEvent(Stream stream)
        {
            ArgumentNullException.ThrowIfNull(stream);

            if (stream.CanSeek)
            {
                stream.Seek(0, 0);
            }

            return JsonSerializer.Deserialize<KlarnaSessionEvent>(stream);
        }

        private async Task<TResult> RequestAsync<TResult>(string url, Func<IFlurlRequest, CancellationToken, Task<TResult>> func, CancellationToken cancellationToken = default)
        {
            FlurlRequest req = new FlurlRequest(_config.BaseUrl + url)
                .WithSettings(x => x.JsonSerializer = new CustomFlurlJsonSerializer(new JsonSerializerOptions
                {
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                    PreferredObjectCreationHandling = JsonObjectCreationHandling.Replace,
                }))
                .WithHeader("Cache-Control", "no-cache")
                .WithBasicAuth(_config.Username, _config.Password);

            return await func.Invoke(req, cancellationToken).ConfigureAwait(false);
        }
    }
}
