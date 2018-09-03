# VAT Calculator

This library does two things:

1. Computes the VAT given a country and a VAT number.
2. Verifies VAT number with the EU Comission's SOAP app.

> When verifying VAT, don't view this as an on-demand service, since it might
fail occasionally due to errors or maintenance (see 
[this document](http://ec.europa.eu/taxation_customs/vies/help.html))

## Examples
```
[Fact]
public void TestCalculateVAT()
{
    // if it's a Swedish company (or any other EU member state)
    // but no VAT is given, then the tax should be 25%.
    Assert.Equal(2500, VATService.CalculateVAT("SE", null).Tax);

    // if it's a Swedish company (or any other EU member state)
    // and a valid VAT is provided, then no tax is applied and
    // it's instead the responsibility of the buyer  to pay it
    // (reversed tax liability)
    Assert.Equal(0, VATService.CalculateVAT("SE", "valid VAT").Tax);

    // if it's a US company (or any other non-EU country), then
    // it's export and no tax should be applied.
    Assert.Equal(0, VATService.CalculateVAT("US", null).Tax);
}

[Fact]
public void TestIsValidVAT()
{
    // before we can compute the tax, we need to check the VAT
    // with the EU comission's API. Note, this service is not
    // working 24/7, so we may need to check the VAT serveral times.
    Assert.True(VATService.IsValidVAT("SE559116174901").IsValid);
}
```
