using System;
using Xunit;

using Cryptolens.VATCalculator;

namespace XUnitTestProject1
{
    public class TaxCalculatorTest
    {
        [Fact]
        public void TestCalculateVAT()
        {
            Assert.Equal(2500, VATService.CalculateVAT("SE", null).Tax);

            Assert.Equal(0, VATService.CalculateVAT("SE", "valid VAT").Tax);

            Assert.Equal(0, VATService.CalculateVAT("US", null).Tax);
        }

        [Fact]
        public void TestIsValidVAT()
        {
            Assert.True(VATService.IsValidVAT("SE559116174901").IsValid);
        }
    }
}
