using System.Text.Json.Serialization;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaMerchantUrls
    {
        [JsonPropertyName("confirmation")]
        public string Confirmation { get; set; }

        [JsonPropertyName("notification")]
        public string Notification { get; set; }

        [JsonPropertyName("push")]
        public string Push { get; set; }
    }
}
