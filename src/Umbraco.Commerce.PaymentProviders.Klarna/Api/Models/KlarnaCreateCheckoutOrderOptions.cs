using System.Text.Json.Serialization;

namespace Umbraco.Commerce.PaymentProviders.Klarna.Api.Models
{
    public class KlarnaCreateCheckoutOrderOptions : KlarnaOrderBase
    {
        [JsonPropertyName("merchant_urls")]
        public KlarnaCheckoutOrderMerchantUrls MerchantUrls { get; set; }

        public KlarnaCreateCheckoutOrderOptions()
            : base()
        {
            MerchantUrls = new KlarnaCheckoutOrderMerchantUrls();
        }
    }
}
