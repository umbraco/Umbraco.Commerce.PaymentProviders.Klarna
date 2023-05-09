using Newtonsoft.Json;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaCreateMerchantSessionOptions : KlarnaOrderBase
    {
        [JsonProperty("acquiring_channel")]
        public string AcquiringChannel { get; set; }

        [JsonProperty("merchant_urls")]
        public KlarnaMerchantUrls MerchantUrls { get; set; }

        public KlarnaCreateMerchantSessionOptions()
            : base()
        {
            MerchantUrls = new KlarnaMerchantUrls();
        }
    }
}
