namespace Exchange.Lib;

public interface ICurrencyConverter
{
    decimal Convert(ExchangeContext context);
}

public class CurrencyConverter : ICurrencyConverter
{
    public decimal Convert(ExchangeContext context)
    {
        var ratio = context.From.Amount / context.To.Amount;
        var result = context.Amount * ratio;
        return decimal.Round(result, 4);
    }
}