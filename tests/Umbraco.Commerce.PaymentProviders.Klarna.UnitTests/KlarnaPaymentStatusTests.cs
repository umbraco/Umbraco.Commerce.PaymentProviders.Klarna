using FluentAssertions;
using Umbraco.Commerce.Core.Models;
using Umbraco.Commerce.PaymentProviders.Klarna.Api.Models;

namespace Umbraco.Commerce.PaymentProviders.Klarna.UnitTests
{
    public class KlarnaPaymentStatusTests
    {
        private readonly KlarnaHppPaymentProvider _provider = new(null!, null!);

        [Theory]
        [InlineData(KlarnaOrder.Statuses.CAPTURED, 10000, 0, PaymentStatus.Captured)]
        [InlineData(KlarnaOrder.Statuses.PART_CAPTURED, 10000, 0, PaymentStatus.Captured)]
        [InlineData(KlarnaOrder.Statuses.CAPTURED, 10000, 2500, PaymentStatus.PartiallyRefunded)]
        [InlineData(KlarnaOrder.Statuses.PART_CAPTURED, 10000, 9999, PaymentStatus.PartiallyRefunded)]
        [InlineData(KlarnaOrder.Statuses.CAPTURED, 10000, 10000, PaymentStatus.Refunded)]
        [InlineData(KlarnaOrder.Statuses.PART_CAPTURED, 10000, 10000, PaymentStatus.Refunded)]
        [InlineData(KlarnaOrder.Statuses.REFUNDED, 10000, 10000, PaymentStatus.Refunded)]
        [InlineData(KlarnaOrder.Statuses.AUTHORIZED, 0, 0, PaymentStatus.Authorized)]
        public void GetPaymentStatus_Should_Map_Captured_And_Refunded_Amounts(string klarnaStatus, int capturedAmount, int refundedAmount, PaymentStatus expected)
        {
            // preparation
            KlarnaOrder order = new()
            {
                Status = klarnaStatus,
                CapturedAmount = capturedAmount,
                RefundedAmount = refundedAmount,
            };

            // execution
            PaymentStatus status = _provider.GetPaymentStatus(order);

            // asserts
            status.Should().Be(expected);
        }
    }
}
