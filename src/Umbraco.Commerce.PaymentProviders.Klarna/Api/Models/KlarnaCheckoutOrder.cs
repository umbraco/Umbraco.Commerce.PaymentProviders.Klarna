using System.Text.Json.Serialization;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaCheckoutOrder
    {
        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }
    }
}
