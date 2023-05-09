using Newtonsoft.Json;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaMerchantSession
    {
        [JsonProperty("session_id")]
        public string SessionId { get; set; }
    }
}
