export default {
    paymentProviders: {
        'klarna-hppLabel': 'Klarna (HPP)',
        'klarna-hppDescription': 'Klarna payment provider using the Klarna Hosted Payment Page (HPP)',
        'klarna-hppSettingsContinueUrlLabel': 'Continue URL',
        'klarna-hppSettingsContinueUrlDescription': 'The URL to continue to after this provider has done processing. eg: /continue/',
        'klarna-hppSettingsCancelUrlLabel': 'Cancel URL',
        'klarna-hppSettingsCancelUrlDescription': 'The URL to return to if the payment attempt is canceled. eg: /cancel/',
        'klarna-hppSettingsErrorUrlLabel': 'Error URL',
        'klarna-hppSettingsErrorUrlDescription': 'The URL to return to if the payment attempt errors. eg: /error/',
        
        'klarna-hppSettingsBillingAddressLine1PropertyAliasLabel': 'Billing Address (Line 1) Property Alias',
        'klarna-hppSettingsBillingAddressLine1PropertyAliasDescription': '[Required] The order property alias containing line 1 of the billing address',
        
        'klarna-hppSettingsBillingAddressLine2PropertyAliasLabel': 'Billing Address (Line 2) Property Alias',
        'klarna-hppSettingsBillingAddressLine2PropertyAliasDescription': 'The order property alias containing line 2 of the billing address',

        'klarna-hppSettingsBillingAddressCityPropertyAliasLabel': 'Billing Address City Property Alias',
        'klarna-hppSettingsBillingAddressCityPropertyAliasDescription': '[Required] The order property alias containing the city of the billing address',

        'klarna-hppSettingsBillingAddressStatePropertyAliasLabel': 'Billing Address State Property Alias',
        'klarna-hppSettingsBillingAddressStatePropertyAliasDescription': 'The order property alias containing the state of the billing address',

        'klarna-hppSettingsBillingAddressZipCodePropertyAliasLabel': 'Billing Address ZipCode Property Alias',
        'klarna-hppSettingsBillingAddressZipCodePropertyAliasDescription': '[Required] The order property alias containing the zip code of the billing address',

        'klarna-hppSettingsApiRegionLabel': 'API Region',
        'klarna-hppSettingsApiRegionDescription': 'The Klarna API Region to use',

        'klarna-hppSettingsTestApiUsernameLabel': 'Test API Username',
        'klarna-hppSettingsTestApiUsernameDescription': 'The Username to use when connecting to the test Klarna API',

        'klarna-hppSettingsTestApiPasswordLabel': 'Test API Password',
        'klarna-hppSettingsTestApiPasswordDescription': 'The Password to use when connecting to the test Klarna API',

        'klarna-hppSettingsLiveApiUsernameLabel': 'Live API Username',
        'klarna-hppSettingsLiveApiUsernameDescription': 'The Username to use when connecting to the live Klarna API',

        'klarna-hppSettingsLiveApiPasswordLabel': 'Live API Password',
        'klarna-hppSettingsLiveApiPasswordDescription': 'The Password to use when connecting to the live Klarna API',

        'klarna-hppSettingsCaptureLabel': 'Capture',
        'klarna-hppSettingsCaptureDescription': 'Flag indicating whether to immediately capture the payment, or whether to just authorize the payment for later (manual) capture',

        'klarna-hppSettingsTestModeLabel': 'Test Mode',
        'klarna-hppSettingsTestModeDescription': 'Set whether to process payments in test mode',

        // ===================
        // Advanced
        // ===================

        'klarna-hppSettingsPaymentPageLogoUrlLabel': 'Payment Page Logo Url',
        'klarna-hppSettingsPaymentPageLogoUrlDescription': 'Fully qualified URL of a logo image to display on the payment page',

        'klarna-hppSettingsPaymentPagePageTitleLabel': 'Payment Page Page Title',
        'klarna-hppSettingsPaymentPagePageTitleDescription': 'A custom title to display on the payment page',

        'klarna-hppSettingsProductTypePropertyAliasLabel': 'Product Type Property Alias',
        'klarna-hppSettingsProductTypePropertyAliasDescription': 'The order line property alias containing the type of the product. Property value can be one of either \'physical\' or \'digital\'',

        'klarna-hppSettingsPaymentMethodCategoriesLabel': 'Payment Method Categories',
        'klarna-hppSettingsPaymentMethodCategoriesDescription': 'Comma separated list of payment method categories to show on the payment page. If empty, all allowable options will be presented. Options are DIRECT_DEBIT, DIRECT_BANK_TRANSFER, PAY_NOW, PAY_LATER and PAY_OVER_TIME',

        'klarna-hppSettingsPaymentMethodCategoryLabel': 'Payment Method Category',
        'klarna-hppSettingsPaymentMethodCategoryDescription': 'The payment method category to show on the payment page. Options are DIRECT_DEBIT, DIRECT_BANK_TRANSFER, PAY_NOW, PAY_LATER and PAY_OVER_TIME',

        'klarna-hppSettingsEnableFallbacksLabel': 'Enable Fallbacks',
        'klarna-hppSettingsEnableFallbacksDescription': 'Set whether to fallback to other payment options if the initial payment attempt fails before redirecting back to the site',
    },
};