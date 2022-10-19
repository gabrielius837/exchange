using Microsoft.Extensions.Configuration;

namespace Exchange.Cli;

public class Program
{
    private static int Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                services.AddSingleton<CurrencyCollection>(GetCurrencyCollection());
                services.AddSingleton<ICurrencyConverter, CurrencyConverter>();
                services.AddSingleton<IValidator<CurrencyCollection>, CurrencyValidator>();
            })
            .Build();
        return HandleExchange(args, host);
    }

    public static int HandleExchange(string[] args, IHost host)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Expecting: Exchange <ISO/ISO> <Amount>"); 
            return 0;
        }
        var collection = host.Services.GetRequiredService<CurrencyCollection>();
        var converter = host.Services.GetRequiredService<ICurrencyConverter>();
        var validator = host.Services.GetRequiredService<IValidator<CurrencyCollection>>();
        var ctx = BuildContext(args, collection, validator);
        if (ctx is null)
            return 1;
        var amount = converter.Convert(ctx);
        Console.WriteLine(amount);
        return 0;
    }

    public static ExchangeContext? BuildContext
    (
        string[] args,
        CurrencyCollection collection,
        IValidator<CurrencyCollection> validator
    )
    {
        var validationResult = validator.Validate(collection);
        if (!validationResult.IsValid)
        {
            Console.Error.WriteLine(validationResult.ToString());
            return null;
        }

        if (args.Length != 2)
        {
            Console.Error.WriteLine("Invalid input");
            Console.Error.WriteLine("Expecting: Exchange <ISO/ISO> <Amount>");
            return null;
        }

        bool error = false;
        var pairs = args[0].Split('/');
        var dict = collection.Currencies.ToDictionary(x => x.ISO, x => x);
        if (pairs.Length != 2)
        {
            error = true;
            Console.Error.WriteLine($"Unable to parse currency pair, expected: ISO/ISO got: {args[0]}");
        }
        else
        {
            if (!dict.ContainsKey(pairs[0]))
            {
                error = true;
                Console.Error.WriteLine($"Could not find currency: {pairs[0]}");
            }

            if (!dict.ContainsKey(pairs[1]))
            {
                error = true;
                Console.Error.WriteLine($"Could not find currency: {pairs[1]}");
            }
        }

        if (args[1].Contains(','))
            args[1] = args[1].Replace(',', '.');
        var validAmount = decimal.TryParse(args[1], out decimal amount);
        if (!validAmount || amount <= 0)
        {
            error = true;
            Console.Error.WriteLine($"Expecting amount as positive decimal, got: {args[1]}");
        }

        if (error)
            return null;

        return new ExchangeContext(dict[pairs[0]], dict[pairs[1]], amount);
    }

    private static CurrencyCollection GetCurrencyCollection()
    {
        return new CurrencyCollection
        (
            new Currency[]
            {
                new Currency("EUR", 7.4394M),
                new Currency("USD", 6.6311M),
                new Currency("GBP", 8.5285M),
                new Currency("SEK", 0.7610M),
                new Currency("NOK", 0.7840M),
                new Currency("CHF", 6.8358M),
                new Currency("JPY", 0.5974M),
                new Currency("DKK", 1M),
            }
        );
        /*
        var root = configuration.GetSection(nameof(CurrencyCollection)).GetSection("Currencies");
        var currencies = new List<Currency>();
        foreach (var child in root.GetChildren())
        {
            var iso = child.GetSection("ISO").Value;
            decimal.TryParse(child.GetSection("Amount").Value, out decimal amount);
            currencies.Add(new Currency(iso, amount));
        }
        return new CurrencyCollection(currencies);
        */
    }
}
