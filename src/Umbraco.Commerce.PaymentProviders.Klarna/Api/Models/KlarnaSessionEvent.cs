using System.Text.Json.Serialization;
using System;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaSessionEvent
    {
        [JsonPropertyName("event_id")]
        public string EventId { get; set; }

        [JsonPropertyName("session")]
        public KlarnaSession Session { get; set; }
    }

    public class KlarnaSession
    {
        public class Statuses
        {
            public const string WAITING = "WAITING";
            public const string IN_PROGRESS = "IN_PROGRESS";
            public const string COMPLETED = "COMPLETED";
            public const string FAILED = "FAILED";
            public const string CANCELLED = "CANCELLED";
            public const string BACK = "BACK";
            public const string ERROR = "ERROR";
            public const string DISABLED = "DISABLED";
        }

        [JsonPropertyName("session_id")]
        public string SessionId { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("authorization_token")]
        public string AuthorizationToken { get; set; }

        [JsonPropertyName("order_id")]
        public string OrderId { get; set; }

        [JsonPropertyName("klarna_reference")]
        public string KlarnaReference { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("expires_at")]
        public DateTime ExpiresAt { get; set; }
    }
}
