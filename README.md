# VAT Calculator

This library does two things:

1. Computes the VAT given a country and VAT.
2. Verifies VAT with the EU Comission's SOAP app.

> When verifying VAT, don't view this as an on-demand service, since it might
fail occasionally due to errors or maintanance (see 
[this document](http://ec.europa.eu/taxation_customs/vies/help.html))

## Examples
```
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
```