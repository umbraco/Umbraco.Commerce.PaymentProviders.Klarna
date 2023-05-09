using Newtonsoft.Json;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaRefundOptions
    {
        [JsonProperty("refunded_amount")]
        public int RefundAmount { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("reference")]
        public string Reference { get; set; }
    }
}
