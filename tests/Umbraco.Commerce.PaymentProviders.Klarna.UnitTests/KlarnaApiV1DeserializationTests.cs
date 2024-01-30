using System.Text.Json;
using FluentAssertions;
using Umbraco.Commerce.PaymentProviders.Klarna.Api;
using Umbraco.Commerce.PaymentProviders.Klarna.Api.Models;

namespace Umbraco.Commerce.PaymentProviders.Klarna.UnitTests
{
    public class KlarnaApiV1DeserializationTests
    {
        /// <summary>
        /// Test json deserialization for <see cref="KlarnaClient.CreateMerchantSessionAsync(KlarnaCreateMerchantSessionOptions, CancellationToken)"/>.
        /// </summary>
        /// <param name="json"></param>
        [Theory]
        [InlineData("{\"client_token\":\"eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.ewogICJzZXNzaW9uX2lkIiA6ICIw\",\"payment_method_categories\":[{\"asset_urls\":{\"descriptive\":\"https://x.klarnacdn.net/payment-method/assets/badges/generic/klarna.svg\",\"standard\":\"https://x.klarnacdn.net/payment-method/assets/badges/generic/klarna.svg\"},\"identifier\":\"klarna\",\"name\":\"Pay with Klarna\"}],\"session_id\":\"0b1d9815-165e-42e2-8867-35bc03789e00\"}")]
        public void CreateMerchantSessionAsync_Deserialization_Should_Succeed(string json)
        {
            // preparation
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web);

            // execution
            KlarnaMerchantSession? deserialized = JsonSerializer.Deserialize<KlarnaMerchantSession>(json, options);

            // asserts
            deserialized.Should().NotBeNull();
            deserialized!.SessionId.Should().Be("0b1d9815-165e-42e2-8867-35bc03789e00");
        }

        /// <summary>
        /// Test json deserialization for <see cref="KlarnaClient.CreateHppSessionAsync(KlarnaCreateHppSessionOptions, CancellationToken)"/>.
        /// </summary>
        /// <param name="json"></param>
        [Theory]
        [InlineData("{\"distribution_module\":{\"generation_url\":\"string\",\"standalone_url\":\"string\",\"token\":\"string\"},\"distribution_url\":\"https://api.klarna.com/hpp/v1/sessions/9cbc9884-1fdb-45a8-9694-9340340d0436/distribution\",\"expires_at\":\"2038-01-19T03:14:07.000Z\",\"manual_identification_check_url\":\"https://api.klarna.com/hpp/v1/sessions/9cbc9884-1fdb-45a8-9694-9340340d0436/manual-id-check\",\"qr_code_url\":\"https://pay.klarna.com/eu/hpp/payments/a94e7760-d135-2721-a538-d6294ea254ed/qr\",\"redirect_url\":\"https://pay.klarna.com/eu/hpp/payments/2OCkffK\",\"session_id\":\"9cbc9884-1fdb-45a8-9694-9340340d0436\",\"session_url\":\"https://api.klarna.com/hpp/v1/sessions/9cbc9884-1fdb-45a8-9694-9340340d0436\"}")]
        public void CreateHppSessionAsync_Deserialization_Should_Succeed(string json)
        {
            // preparation
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web);

            // execution
            KlarnaHppSession? deserialized = JsonSerializer.Deserialize<KlarnaHppSession>(json, options);

            // asserts
            deserialized.Should().NotBeNull();
        }

        /// <summary>
        /// Test json deserialization for <see cref="KlarnaClient.GetOrderAsync(string, CancellationToken)"/>.
        /// </summary>
        /// <param name="json"></param>
        [Theory]
        [InlineData("{\"order_id\":\"7849fd84-47dc-4919-a7ce-xxxxxxxxxx\",\"status\":\"CAPTURED\",\"fraud_status\":\"ACCEPTED\",\"order_amount\":200,\"original_order_amount\":200,\"captured_amount\":200,\"refunded_amount\":0,\"remaining_authorized_amount\":0,\"purchase_currency\":\"PLN\",\"locale\":\"pl-PL\",\"order_lines\":[{\"reference\":\"\",\"type\":\"physical\",\"quantity\":1,\"quantity_unit\":\"\",\"name\":\"dress\",\"total_amount\":200,\"unit_price\":200,\"total_discount_amount\":0,\"tax_rate\":0,\"total_tax_amount\":0}],\"klarna_reference\":\"20S0XXXX\",\"billing_address\":{\"given_name\":\"Test\",\"family_name\":\"Person-pl\",\"title\":\"\",\"street_address\":\"Ul. Górczewska 124\",\"street_address2\":\"\",\"postal_code\":\"01-460\",\"city\":\"Warszawa\",\"region\":\"\",\"country\":\"PL\",\"email\":\"customer@email.pl\",\"phone\":\"+48795222223\"},\"created_at\":\"2022-07-08T09:48:16.680Z\",\"purchase_country\":\"PL\",\"expires_at\":\"2022-08-05T00:00:00.000Z\",\"captures\":[{\"capture_id\":\"32904e4c-61d2-400e-9b77-XXXXXX\",\"klarna_reference\":\"20S0XXXX-1\",\"captured_amount\":200,\"captured_at\":\"2022-07-08T09:48:17.738Z\",\"description\":\"\",\"order_lines\":[],\"refunded_amount\":0,\"billing_address\":{\"given_name\":\"Test\",\"family_name\":\"Person-pl\",\"title\":\"\",\"street_address\":\"Ul. Górczewska 124\",\"street_address2\":\"\",\"postal_code\":\"01-460\",\"city\":\"Warszawa\",\"region\":\"\",\"country\":\"PL\",\"email\":\"customer@email.pl\",\"phone\":\"+48795222223\"}}],\"refunds\":[],\"initial_payment_method\":{\"type\":\"INVOICE\",\"description\":\"Pay later\"}}")]
        public void GetOrderAsync_Deserialization_Should_Succeed(string json)
        {
            // preparation
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web);

            // execution
            KlarnaOrder? deserialized = JsonSerializer.Deserialize<KlarnaOrder>(json, options);

            // asserts
            deserialized.Should().NotBeNull();
        }

        /// <summary>
        /// Test json deserialization for <see cref="KlarnaClient.ParseSessionEvent(Stream)"/>.
        /// </summary>
        /// <param name="json"></param>
        [Theory]
        [InlineData("{\"event_id\":\"cd7e1171-25b1-41ff-97d3-b0dd5e6f9a82\",\"session\":{\"session_id\":\"39a1c773-bafd-754d-af1f-b30c592f1267\",\"status\":\"COMPLETED\",\"order_id\":\"a1a8f727-2756-6058-bd3c-40069be0994b\",\"klarna_reference\":\"X438HG0Q\",\"updated_at\":\"2019-05-13T14:54:04.675Z\",\"expires_at\":\"2019-05-15T13:51:43.507Z\",\"authorization_token\":\"a1a8f727-2756-6058-bd3c-40069be0994b\"}}")]
        public void ParseSessionEvent_Deserialization_Should_Succeed(string json)
        {
            // preparation
            JsonSerializerOptions options = new(JsonSerializerDefaults.Web);

            // execution
            KlarnaSessionEvent? deserialized = JsonSerializer.Deserialize<KlarnaSessionEvent>(json, options);

            // asserts
            deserialized.Should().NotBeNull();
            TestHelpers.AssertAllPropertiesAreNotNull(deserialized!);
            TestHelpers.AssertAllPropertiesAreNotNull(deserialized!.Session);
        }

    }
}
