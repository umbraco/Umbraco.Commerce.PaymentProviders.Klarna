using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaHppOptions
    {
        public static class PlaceOrderModes
        {
            public const string NONE = "NONE";
            public const string PLACE_ORDER = "PLACE_ORDER";
            public const string CAPTURE_ORDER = "CAPTURE_ORDER";
        }

        [JsonPropertyName("place_order_mode")]
        public string PlaceOrderMode { get; set; }

        [JsonPropertyName("logo_url")]
        public string LogoUrl { get; set; }

        [JsonPropertyName("page_title")]
        public string PageTitle { get; set; }

        [JsonPropertyName("payment_method_categories")]
        public string[] PaymentMethodCategories { get; set; }

        [JsonPropertyName("payment_method_category")]
        public string PaymentMethodCategory { get; set; }

        [JsonPropertyName("payment_fallback")]
        public bool PaymentFallback { get; set; }
    }
}
