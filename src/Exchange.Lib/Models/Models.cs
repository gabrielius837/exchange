namespace Exchange.Lib;

public record Currency(string ISO, decimal Amount);
public record ExchangeContext(Currency From, Currency To, decimal Amount);
public record CurrencyCollection(IEnumerable<Currency> Currencies);