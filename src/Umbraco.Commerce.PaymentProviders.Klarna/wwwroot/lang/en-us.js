export default {
    ucPaymentProviders: {
        'klarnaHppLabel': 'Klarna (HPP)',
        'klarnaHppDescription': 'Klarna payment provider using the Klarna Hosted Payment Page (HPP)',
        'klarnaHppSettingsContinueUrlLabel': 'Continue URL',
        'klarnaHppSettingsContinueUrlDescription': 'The URL to continue to after this provider has done processing. eg: /continue/',
        'klarnaHppSettingsCancelUrlLabel': 'Cancel URL',
        'klarnaHppSettingsCancelUrlDescription': 'The URL to return to if the payment attempt is canceled. eg: /cancel/',
        'klarnaHppSettingsErrorUrlLabel': 'Error URL',
        'klarnaHppSettingsErrorUrlDescription': 'The URL to return to if the payment attempt errors. eg: /error/',
        
        'klarnaHppSettingsBillingAddressLine1PropertyAliasLabel': 'Billing Address (Line 1) Property Alias',
        'klarnaHppSettingsBillingAddressLine1PropertyAliasDescription': '[Required] The order property alias containing line 1 of the billing address',
        
        'klarnaHppSettingsBillingAddressLine2PropertyAliasLabel': 'Billing Address (Line 2) Property Alias',
        'klarnaHppSettingsBillingAddressLine2PropertyAliasDescription': 'The order property alias containing line 2 of the billing address',

        'klarnaHppSettingsBillingAddressCityPropertyAliasLabel': 'Billing Address City Property Alias',
        'klarnaHppSettingsBillingAddressCityPropertyAliasDescription': '[Required] The order property alias containing the city of the billing address',

        'klarnaHppSettingsBillingAddressStatePropertyAliasLabel': 'Billing Address State Property Alias',
        'klarnaHppSettingsBillingAddressStatePropertyAliasDescription': 'The order property alias containing the state of the billing address',

        'klarnaHppSettingsBillingAddressZipCodePropertyAliasLabel': 'Billing Address ZipCode Property Alias',
        'klarnaHppSettingsBillingAddressZipCodePropertyAliasDescription': '[Required] The order property alias containing the zip code of the billing address',

        'klarnaHppSettingsApiRegionLabel': 'API Region',
        'klarnaHppSettingsApiRegionDescription': 'The Klarna API Region to use',

        'klarnaHppSettingsTestApiUsernameLabel': 'Test API Username',
        'klarnaHppSettingsTestApiUsernameDescription': 'The Username to use when connecting to the test Klarna API',

        'klarnaHppSettingsTestApiPasswordLabel': 'Test API Password',
        'klarnaHppSettingsTestApiPasswordDescription': 'The Password to use when connecting to the test Klarna API',

        'klarnaHppSettingsLiveApiUsernameLabel': 'Live API Username',
        'klarnaHppSettingsLiveApiUsernameDescription': 'The Username to use when connecting to the live Klarna API',

        'klarnaHppSettingsLiveApiPasswordLabel': 'Live API Password',
        'klarnaHppSettingsLiveApiPasswordDescription': 'The Password to use when connecting to the live Klarna API',

        'klarnaHppSettingsCaptureLabel': 'Capture',
        'klarnaHppSettingsCaptureDescription': 'Flag indicating whether to immediately capture the payment, or whether to just authorize the payment for later (manual) capture',

        'klarnaHppSettingsTestModeLabel': 'Test Mode',
        'klarnaHppSettingsTestModeDescription': 'Set whether to process payments in test mode',

        // ===================
        // Advanced
        // ===================

        'klarnaHppSettingsPaymentPageLogoUrlLabel': 'Payment Page Logo Url',
        'klarnaHppSettingsPaymentPageLogoUrlDescription': 'Fully qualified URL of a logo image to display on the payment page',

        'klarnaHppSettingsPaymentPagePageTitleLabel': 'Payment Page Page Title',
        'klarnaHppSettingsPaymentPagePageTitleDescription': 'A custom title to display on the payment page',

        'klarnaHppSettingsProductTypePropertyAliasLabel': 'Product Type Property Alias',
        'klarnaHppSettingsProductTypePropertyAliasDescription': 'The order line property alias containing the type of the product. Property value can be one of either \'physical\' or \'digital\'',

        'klarnaHppSettingsPaymentMethodCategoriesLabel': 'Payment Method Categories',
        'klarnaHppSettingsPaymentMethodCategoriesDescription': 'Comma separated list of payment method categories to show on the payment page. If empty, all allowable options will be presented. Options are DIRECT_DEBIT, DIRECT_BANK_TRANSFER, PAY_NOW, PAY_LATER and PAY_OVER_TIME',

        'klarnaHppSettingsPaymentMethodCategoryLabel': 'Payment Method Category',
        'klarnaHppSettingsPaymentMethodCategoryDescription': 'The payment method category to show on the payment page. Options are DIRECT_DEBIT, DIRECT_BANK_TRANSFER, PAY_NOW, PAY_LATER and PAY_OVER_TIME',

        'klarnaHppSettingsEnableFallbacksLabel': 'Enable Fallbacks',
        'klarnaHppSettingsEnableFallbacksDescription': 'Set whether to fallback to other payment options if the initial payment attempt fails before redirecting back to the site',

        'klarnaHppMetaDataKlarnaSessionIdLabel': 'Klarna Session ID',
        'klarnaHppMetaDataKlarnaOrderIdLabel': 'Klarna Order ID',
        'klarnaHppMetaDataKlarnaReferenceLabel': 'Klarna Reference',
    },
};
