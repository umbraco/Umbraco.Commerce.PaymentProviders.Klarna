using Umbraco.Commerce.Core.PaymentProviders;
using Umbraco.Commerce.PaymentProviders.Klarna.Api.Models;

namespace Umbraco.Commerce.PaymentProviders.Klarna
{
    public class KlarnaSettingsBase
    {
        [PaymentProviderSetting(SortOrder = 100)]
        public string ContinueUrl { get; set; }

        [PaymentProviderSetting(SortOrder = 200)]
        public string CancelUrl { get; set; }

        [PaymentProviderSetting(SortOrder = 250)]
        public string TermsUrl { get; set; }

        [PaymentProviderSetting(SortOrder = 300)]
        public string ErrorUrl { get; set; }

        [PaymentProviderSetting(SortOrder = 500)]
        public string BillingAddressLine1PropertyAlias { get; set; }

        [PaymentProviderSetting(SortOrder = 600)]
        public string BillingAddressLine2PropertyAlias { get; set; }

        [PaymentProviderSetting(SortOrder = 700)]
        public string BillingAddressCityPropertyAlias { get; set; }

        [PaymentProviderSetting(SortOrder = 800)]
        public string BillingAddressStatePropertyAlias { get; set; }

        [PaymentProviderSetting(SortOrder = 900)]
        public string BillingAddressZipCodePropertyAlias { get; set; }

        [PaymentProviderSetting(SortOrder = 1000)]
        public KlarnaApiRegion ApiRegion { get; set; }

        [PaymentProviderSetting(SortOrder = 1100)]
        public string TestApiUsername { get; set; }

        [PaymentProviderSetting(SortOrder = 1200)]
        public string TestApiPassword { get; set; }

        [PaymentProviderSetting(SortOrder = 1300)]
        public string LiveApiUsername { get; set; }

        [PaymentProviderSetting(SortOrder = 1400)]
        public string LiveApiPassword { get; set; }

        [PaymentProviderSetting(SortOrder = 1500)]
        public bool Capture { get; set; }

        [PaymentProviderSetting(SortOrder = 10000)]
        public bool TestMode { get; set; }

        // ============================
        // Advanced
        // ============================

        [PaymentProviderSetting(SortOrder = 100, IsAdvanced = true)]
        public string PaymentPageLogoUrl { get; set; }

        [PaymentProviderSetting(
            SortOrder = 150,
            IsAdvanced = true)]
        public string PaymentPagePageTitle { get; set; }

        [PaymentProviderSetting(
            SortOrder = 200,
            IsAdvanced = true)]
        public string ProductTypePropertyAlias { get; set; }

        [PaymentProviderSetting(
            SortOrder = 300,
            IsAdvanced = true)]
        public string PaymentMethodCategories { get; set; }

        [PaymentProviderSetting(
            SortOrder = 400,
            IsAdvanced = true)]
        public string PaymentMethodCategory { get; set; }

        [PaymentProviderSetting(
            SortOrder = 500,
            IsAdvanced = true)]
        public bool EnableFallbacks { get; set; }
    }
}
