namespace Exchange.UnitTests;

public class CurrencyValidator_Tests
{
    [Fact]
    public void Validate_MustContainerError_ForCurrencies()
    {
        // arrange
        var collection = new CurrencyCollection
        (
            Array.Empty<Currency>()
        );
        var validator = new CurrencyValidator();

        // act
        var result = validator.Validate(collection);

        // assert
        Assert.False(result.IsValid);
        Assert.True(result.Properties.ContainsKey("Currencies"));
        Assert.Equal(1, (int)result.Properties["Currencies"].Count());
    }

    [Fact]
    public void Validate_MustContainerError_ForISO()
    {
        // arrange
        var collection = new CurrencyCollection
        (
            new Currency[]
            {
                new Currency("wrong", 1.6868M),
            }
        );
        var validator = new CurrencyValidator();

        // act
        var result = validator.Validate(collection);

        // assert
        Assert.False(result.IsValid);
        Assert.True(result.Properties.ContainsKey("ISO"));
        Assert.Equal(1, (int)result.Properties["ISO"].Count());
    }

    [Fact]
    public void Validate_MustContainerError_ForISODuplicate()
    {
        // arrange
        var collection = new CurrencyCollection
        (
            new Currency[]
            {
                new Currency("EUR", 1.6868M),
                new Currency("EUR", 1.6868M)
            }
        );
        var validator = new CurrencyValidator();

        // act
        var result = validator.Validate(collection);

        // assert
        Assert.False(result.IsValid);
        Assert.True(result.Properties.ContainsKey("ISO"));
        Assert.Equal(1, (int)result.Properties["ISO"].Count());
    }

    [Fact]
    public void Validate_MustContainerError_ForAmount()
    {
        // arrange
        var collection = new CurrencyCollection
        (
            new Currency[]
            {
                new Currency("EUR", 0M),
            }
        );
        var validator = new CurrencyValidator();

        // act
        var result = validator.Validate(collection);

        // assert
        Assert.False(result.IsValid);
        Assert.True(result.Properties.ContainsKey("Amount"));
        Assert.Equal(1, (int)result.Properties["Amount"].Count());
    }
}