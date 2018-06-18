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
            //TaxCalculator.CalculateTax();
        }

        [Fact]
        public void TestIsValidVAT()
        {
            Assert.True(Cryptolens.VATCalculator.VATCalculator.IsValidVAT("SE559116174901").IsValid);
        }
    }
}
