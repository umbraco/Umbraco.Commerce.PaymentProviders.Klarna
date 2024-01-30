using FluentAssertions;

namespace Umbraco.Commerce.PaymentProviders.Klarna.UnitTests
{
    internal class TestHelpers
    {
        public static void AssertAllPropertiesAreNotNull(object obj)
        {
            System.Reflection.PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo property in properties)
            {
                var value = property.GetValue(obj);
                value.Should().NotBeNull("property '{0}' should not be null", property.Name);
            }
        }
    }
}
