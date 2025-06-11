using System.Text.Json.Serialization;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaOrderLine
    {
        public class Types
        {
            public const string PHYSICAL = "physical";
            public const string DIGITAL = "digital";
            public const string GIFT_CARD = "gift_card";
            public const string DISCOUNT = "discount";
            public const string SHIPPING_FEE = "shipping_fee";
            public const string SALES_TAX = "sales_tax";
            public const string STORE_CREDIT = "store_credit";
            public const string SURCHARGE = "surcharge";
        }

        [JsonPropertyName("reference")]
        public string Reference { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("tax_rate")]
        public int? TaxRate { get; set; }

        [JsonPropertyName("unit_price")]
        public int? UnitPrice { get; set; }

        [JsonPropertyName("quantity")]
        public int? Quantity { get; set; }

        [JsonPropertyName("total_discount_amount")]
        public int? TotalDiscountAmount { get; set; }

        [JsonPropertyName("total_tax_amount")]
        public int? TotalTaxAmount { get; set; }

        [JsonPropertyName("total_amount")]
        public int? TotalAmount { get; set; }
    }
}
