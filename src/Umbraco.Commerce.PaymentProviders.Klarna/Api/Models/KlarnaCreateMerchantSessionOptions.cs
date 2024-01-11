using System.Text.Json.Serialization;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaCreateMerchantSessionOptions : KlarnaOrderBase
    {
        [JsonPropertyName("acquiring_channel")]
        public string AcquiringChannel { get; set; }

        [JsonPropertyName("merchant_urls")]
        public KlarnaMerchantUrls MerchantUrls { get; set; }

        public KlarnaCreateMerchantSessionOptions()
            : base()
        {
            MerchantUrls = new KlarnaMerchantUrls();
        }
    }
}
