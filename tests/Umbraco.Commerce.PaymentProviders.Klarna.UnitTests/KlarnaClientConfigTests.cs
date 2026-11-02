using FluentAssertions;
using Umbraco.Commerce.PaymentProviders.Klarna.Api;
using Umbraco.Commerce.PaymentProviders.Klarna.Api.Models;

namespace Umbraco.Commerce.PaymentProviders.Klarna.UnitTests
{
    public class KlarnaClientConfigTests
    {
        [Theory]
        [InlineData(KlarnaApiRegion.Europe, KlarnaClient.EuLiveApiUrl)]
        [InlineData(KlarnaApiRegion.NorthAmerica, KlarnaClient.NaLiveApiUrl)]
        [InlineData(KlarnaApiRegion.Oceania, KlarnaClient.OcLiveApiUrl)]
        public void Live_Config_Uses_Region_Url(KlarnaApiRegion region, string expected)
        {
            new LiveKlarnaClientConfig("u", "p", region).BaseUrl.Should().Be(expected);
        }

        [Theory]
        [InlineData(KlarnaApiRegion.Europe, KlarnaClient.EuPlaygroundApiUrl)]
        [InlineData(KlarnaApiRegion.NorthAmerica, KlarnaClient.NaPlaygroundApiUrl)]
        [InlineData(KlarnaApiRegion.Oceania, KlarnaClient.OcPlaygroundApiUrl)]
        public void Playground_Config_Uses_Region_Url(KlarnaApiRegion region, string expected)
        {
            new PlaygroundKlarnaClientConfig("u", "p", region).BaseUrl.Should().Be(expected);
        }

        [Theory]
        [InlineData(true, typeof(PlaygroundKlarnaClientConfig), "test-user", "test-pass")]
        [InlineData(false, typeof(LiveKlarnaClientConfig), "live-user", "live-pass")]
        public void Provider_Picks_Credentials_By_Test_Mode(bool testMode, Type expectedType, string expectedUser, string expectedPassword)
        {
            KlarnaHppSettings settings = new()
            {
                TestMode = testMode,
                TestApiUsername = "test-user",
                TestApiPassword = "test-pass",
                LiveApiUsername = "live-user",
                LiveApiPassword = "live-pass",
                ApiRegion = KlarnaApiRegion.NorthAmerica,
            };

            KlarnaClientConfig config = new TestableProvider().ClientConfig(settings);

            config.Should().BeOfType(expectedType);
            config.Username.Should().Be(expectedUser);
            config.Password.Should().Be(expectedPassword);
            config.ApiRegion.Should().Be(KlarnaApiRegion.NorthAmerica);
        }

        [Theory]
        [InlineData("https://shop.example.com/cancel", "reason=back", "https://shop.example.com/cancel?reason=back")]
        [InlineData("https://shop.example.com/cancel?x=1", "reason=back", "https://shop.example.com/cancel?x=1&reason=back")]
        public void AppendQueryString_Picks_Separator(string url, string qs, string expected)
        {
            new TestableProvider().Append(url, qs).Should().Be(expected);
        }

        private sealed class TestableProvider : KlarnaHppPaymentProvider
        {
            public TestableProvider()
                : base(null!, null!)
            {
            }

            public KlarnaClientConfig ClientConfig(KlarnaSettingsBase settings) => GetKlarnaClientConfig(settings);

            public string Append(string url, string qs) => AppendQueryString(url, qs);
        }
    }
}
