using Newtonsoft.Json;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaMerchantUrls
    {
        [JsonProperty("confirmation")]
        public string Confirmation { get; set; }

        [JsonProperty("notification")]
        public string Notification { get; set; }

        [JsonProperty("push")]
        public string Push { get; set; }
    }
}
