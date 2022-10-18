namespace Exchange.Lib;

public interface IValidator<T> where T : class
{
    ValidationResult Validate(T input);
}

public class CurrencyValidator : IValidator<CurrencyCollection>
{
    public ValidationResult Validate(CurrencyCollection collection)
    {
        var dict = new Dictionary<string, IEnumerable<string>>();
        ValidateCurrencies(collection.Currencies, dict);
        ValidateISO(collection.Currencies, dict);
        ValidateAmount(collection.Currencies, dict);
        return new ValidationResult(dict);
    }

    private void ValidateCurrencies(IEnumerable<Currency> currencies, Dictionary<string, IEnumerable<string>> results)
    {
        if (currencies.Count() == 0)
            results.Add("Currencies", new string[] { "No currencies were found" });
    }

    private void ValidateISO(IEnumerable<Currency> currencies, Dictionary<string, IEnumerable<string>> results)
    {
        var list = new List<string>();
        var duplicates = currencies
            .GroupBy(x => x.ISO)
            .Where(x => x.Count() > 1)
            .ToDictionary(x => x.Key)
            .Select(duplicate => $"Contains duplicate value \"{duplicate.Key}\"");

        if (duplicates.Count() > 0)
            list.AddRange(duplicates);
        
        var empties = currencies
            .Where(x => x.ISO.Length != 3 || x.ISO.Any(y => !char.IsUpper(y)))
            .Select(x => $"Value must be 3 uppercase letters long \"{x.ISO}\"");
        
        if (empties.Count() > 0)
            list.AddRange(empties);
        
        if (list.Count > 0)
            results.Add("ISO", list);
    }

    private void ValidateAmount(IEnumerable<Currency> currencies, Dictionary<string, IEnumerable<string>> results)
    {
        var list = new List<string>();

        var invalidAmounts = currencies
            .Where(x => x.Amount <= 0M)
            .Select(x => $"Value must be greater then zero \"{x.Amount}\"");
        
        if (invalidAmounts.Count() > 0)
            list.AddRange(invalidAmounts);
        
        if (list.Count > 0)
            results.Add("Amount", list);
    }
}