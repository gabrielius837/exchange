
namespace Exchange.UnitTests;

public class Program_Tests
{
    [Fact]
    public void BuildContext_MustReturnNotNull()
    {
        // arrange
        const decimal amount = 23.72M;
        var firstCurrency = new Currency("EUR", 2);
        var secondCurrency = new Currency("DKK", 3);
        var args = new string[] { "EUR/DKK", amount.ToString() };
        var collection = new CurrencyCollection
        (
            new Currency[]
            {
                firstCurrency,
                secondCurrency
            }
        );
        var validator = new Mock<IValidator<CurrencyCollection>>();
        var validationResult = new ValidationResult(new Dictionary<string, IEnumerable<string>>());
        validator.Setup(x => x.Validate(collection)).Returns(validationResult);

        // act
        var context = Program.BuildContext(args, collection, validator.Object);

        // assert
        Assert.NotNull(context);
        Assert.Equal(firstCurrency, context?.From);
        Assert.Equal(secondCurrency, context?.To);
        Assert.Equal(amount, context?.Amount);
    }

    [Fact]
    public void BuildContext_MustReturnNull_WhenCurrenciesValidationFails()
    {
        // arrange
        const decimal amount = 23.72M;
        var firstCurrency = new Currency("EUR", 2);
        var secondCurrency = new Currency("DKK", 3);
        var args = new string[] { "EUR/DKK", amount.ToString() };
        var collection = new CurrencyCollection
        (
            new Currency[]
            {
                firstCurrency,
                secondCurrency
            }
        );
        var validator = new Mock<IValidator<CurrencyCollection>>();
        var validationResult = new ValidationResult(new Dictionary<string, IEnumerable<string>>()
        {
            { "random", new string[] { "error" }}
        });
        validator.Setup(x => x.Validate(collection)).Returns(validationResult);

        // act
        var context = Program.BuildContext(args, collection, validator.Object);

        // assert
        Assert.Null(context);
    }

    [Fact]
    public void BuildContext_MustReturnNull_WhenRecieveUnexpectedArgs()
    {
        // arrange
        const decimal amount = 23.72M;
        var firstCurrency = new Currency("EUR", 2);
        var secondCurrency = new Currency("DKK", 3);
        var args = new string[] { "EUR/DKK", amount.ToString(), "extra" };
        var collection = new CurrencyCollection
        (
            new Currency[]
            {
                firstCurrency,
                secondCurrency
            }
        );
        var validator = new Mock<IValidator<CurrencyCollection>>();
        var validationResult = new ValidationResult(new Dictionary<string, IEnumerable<string>>());
        validator.Setup(x => x.Validate(collection)).Returns(validationResult);

        // act
        var context = Program.BuildContext(args, collection, validator.Object);

        // assert
        Assert.Null(context);
    }

    [Fact]
    public void BuildContext_MustReturnNull_WhenCouldNotValidatePairs()
    {
        // arrange
        const decimal amount = 23.72M;
        var firstCurrency = new Currency("EUR", 2);
        var secondCurrency = new Currency("DKK", 3);
        var args = new string[] { "test", amount.ToString(), "extra" };
        var collection = new CurrencyCollection
        (
            new Currency[]
            {
                firstCurrency,
                secondCurrency
            }
        );
        var validator = new Mock<IValidator<CurrencyCollection>>();
        var validationResult = new ValidationResult(new Dictionary<string, IEnumerable<string>>());
        validator.Setup(x => x.Validate(collection)).Returns(validationResult);

        // act
        var context = Program.BuildContext(args, collection, validator.Object);

        // assert
        Assert.Null(context);
    }

    [Fact]
    public void BuildContext_MustReturnNull_WhenPairWasNotFound()
    {
        // arrange
        const decimal amount = 23.72M;
        var firstCurrency = new Currency("EUR", 2);
        var args = new string[] { "EUR/DKK", amount.ToString(), "extra" };
        var collection = new CurrencyCollection
        (
            new Currency[]
            {
                firstCurrency
            }
        );
        var validator = new Mock<IValidator<CurrencyCollection>>();
        var validationResult = new ValidationResult(new Dictionary<string, IEnumerable<string>>());
        validator.Setup(x => x.Validate(collection)).Returns(validationResult);

        // act
        var context = Program.BuildContext(args, collection, validator.Object);

        // assert
        Assert.Null(context);
    }

    [Fact]
    public void BuildContext_MustReturnNull_WhenInvalidAmount()
    {
        // arrange
        const decimal amount = -1M;
        var firstCurrency = new Currency("EUR", 2);
        var args = new string[] { "EUR/DKK", amount.ToString(), "extra" };
        var collection = new CurrencyCollection
        (
            new Currency[]
            {
                firstCurrency
            }
        );
        var validator = new Mock<IValidator<CurrencyCollection>>();
        var validationResult = new ValidationResult(new Dictionary<string, IEnumerable<string>>());
        validator.Setup(x => x.Validate(collection)).Returns(validationResult);

        // act
        var context = Program.BuildContext(args, collection, validator.Object);

        // assert
        Assert.Null(context);
    }
}