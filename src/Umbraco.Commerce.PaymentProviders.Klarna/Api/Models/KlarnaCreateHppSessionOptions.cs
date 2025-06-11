using System.Text.Json.Serialization;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaCreateHppSessionOptions
    {
        [JsonPropertyName("payment_session_url")]
        public string PaymentSessionUrl { get; set; }

        [JsonPropertyName("merchant_urls")]
        public KlarnaHppMerchantUrls MerchantUrls { get; set; }

        [JsonPropertyName("options")]
        public KlarnaHppOptions Options { get; set; }

        public KlarnaCreateHppSessionOptions()
        {
            MerchantUrls = new KlarnaHppMerchantUrls();
            Options = new KlarnaHppOptions();
        }
    }
}
