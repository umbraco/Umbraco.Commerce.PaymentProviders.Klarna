using System.Text.Json.Serialization;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaMerchantSession
    {
        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }
    }
}
