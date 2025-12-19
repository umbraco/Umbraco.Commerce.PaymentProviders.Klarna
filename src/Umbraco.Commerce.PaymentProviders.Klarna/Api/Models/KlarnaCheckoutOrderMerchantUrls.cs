using System.Text.Json.Serialization;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaCheckoutOrderMerchantUrls
    {
        [JsonPropertyName("terms")]
        public string Terms { get; set; }

        [JsonPropertyName("checkout")]
        public string Checkout { get; set; }

        [JsonPropertyName("confirmation")]
        public string Confirmation { get; set; }

        [JsonPropertyName("push")]
        public string Push { get; set; }

        [JsonPropertyName("validation")]
        public string Validation { get; set; }

        [JsonPropertyName("notification")]
        public string Notification { get; set; }

        [JsonPropertyName("cancellation_terms")]
        public string CancellationTerms { get; set; }

        [JsonPropertyName("shipping_option_update")]
        public string ShippingOptionUpdate { get; set; }

        [JsonPropertyName("address_update")]
        public string AddressUpdate { get; set; }

        [JsonPropertyName("country_change")]
        public string CountryChange { get; set; }
    }
}
