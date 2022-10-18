namespace Exchange.UnitTests;

public class CurrencyConverter_Tests
{
    [Fact]
    public void Convert_MustReturnExpectedAmount()
    {
        // arrange
        var from = new Currency("EUR", 7.4394M);
        var to = new Currency("DKK", 1M);
        const decimal amount = 1M;
        var context = new ExchangeContext(from, to, amount);
        var convert = new CurrencyConverter();
        const decimal expected = 7.4394M;

        // act
        var result = convert.Convert(context);

        // assert
        Assert.Equal(expected, result);
    }
}