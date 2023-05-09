using Flurl.Http;
using Flurl.Http.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
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

        private KlarnaClientConfig _config;

        public KlarnaClient(KlarnaClientConfig config)
        {
            _config = config;
        }

        public async Task<KlarnaMerchantSession> CreateMerchantSessionAsync(KlarnaCreateMerchantSessionOptions opts, CancellationToken cancellationToken = default)
        {
            return await RequestAsync("/payments/v1/sessions", async (req, ct) => await req
                .PostJsonAsync(opts, ct)
                .ReceiveJson<KlarnaMerchantSession>().ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task<KlarnaHppSession> CreateHppSessionAsync(KlarnaCreateHppSessionOptions opts, CancellationToken cancellationToken = default)
        {
            return await RequestAsync("/hpp/v1/sessions", async (req, ct) => await req
                .PostJsonAsync(opts, ct)
                .ReceiveJson<KlarnaHppSession>().ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task<KlarnaOrder> GetOrderAsync(string orderId, CancellationToken cancellationToken = default)
        {
            return await RequestAsync($"/ordermanagement/v1/orders/{orderId}", async (req, ct) => await req
                .GetAsync(ct)
                .ReceiveJson<KlarnaOrder>().ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task CancelOrderAsync(string orderId, CancellationToken cancellationToken = default)
        {
            await RequestAsync($"/ordermanagement/v1/orders/{orderId}/cancel", async (req, ct) => await req
                .PostAsync(null, ct).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task CaptureOrderAsync(string orderId, KlarnaCaptureOptions opts, CancellationToken cancellationToken = default)
        {
            await RequestAsync($"/ordermanagement/v1/orders/{orderId}/captures", async (req, ct) => await req
                .PostJsonAsync(opts, ct).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
        }

        public async Task RefundOrderAsync(string orderId, KlarnaRefundOptions opts, CancellationToken cancellationToken = default)
        {
            await RequestAsync($"/ordermanagement/v1/orders/{orderId}/refunds", async (req, ct) => await req
                .PostJsonAsync(opts, ct).ConfigureAwait(false),
                cancellationToken).ConfigureAwait(false);
        }

        public KlarnaSessionEvent ParseSessionEvent(Stream stream)
        {
            var serializer = new JsonSerializer();

            if (stream.CanSeek)
            {
                stream.Seek(0, 0);
            }

            using (var sr = new StreamReader(stream))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<KlarnaSessionEvent>(jsonTextReader);
            }
        }

        private async Task<TResult> RequestAsync<TResult>(string url, Func<IFlurlRequest, CancellationToken, Task<TResult>> func, CancellationToken cancellationToken = default)
        {
            var req = new FlurlRequest(_config.BaseUrl + url)
                .ConfigureRequest(x => x.JsonSerializer = new NewtonsoftJsonSerializer(new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ObjectCreationHandling = ObjectCreationHandling.Replace,
                })) 
                .WithHeader("Cache-Control", "no-cache")
                .WithBasicAuth(_config.Username, _config.Password);

            return await func.Invoke(req, cancellationToken).ConfigureAwait(false);
        }
    }
}
