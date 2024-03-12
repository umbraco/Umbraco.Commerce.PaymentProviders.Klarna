using System.Text.Json.Serialization;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaOrder : KlarnaOrderBase
    {
        public class Statuses
        {
            public const string AUTHORIZED = "AUTHORIZED";
            public const string CAPTURED = "CAPTURED";
            public const string PART_CAPTURED = "PART_CAPTURED";
            public const string EXPIRED = "EXPIRED";
            public const string CANCELLED = "CANCELLED";
            public const string REFUNDED = "REFUNDED";
            public const string CLOSED = "CLOSED";
        }

        public class FraudStatuses
        {
            public const string ACCEPTED = "ACCEPTED";
            public const string PENDING = "PENDING";
            public const string REJECTED = "REJECTED";
        }

        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }

        [JsonPropertyName("klarna_reference")]
        public string KlarnaReference { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("original_order_amount")]
        public int OriginalOrderAmount { get; set; }

        [JsonPropertyName("captured_amount")]
        public int CapturedAmount { get; set; }

        [JsonPropertyName("refunded_amount")]
        public int RefundedAmount { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("fraud_status")]
        public string FraudStatus { get; set; }

        public KlarnaOrder()
            : base()
        { }
    }
}
