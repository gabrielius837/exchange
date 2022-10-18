namespace Exchange.Lib;

public class ValidationResult
{
    private readonly Dictionary<string, IEnumerable<string>> _results;

    public ValidationResult(Dictionary<string, IEnumerable<string>> results)
    {
        _results = results;
    }

    public bool IsValid => _results.Count == 0;
    public IReadOnlyDictionary<string, IEnumerable<string>> Properties => _results;

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var result in _results)
        {
            sb.AppendLine($"{result.Key}:");
            foreach (var msg in result.Value)
            {
                sb.AppendLine($"\t{msg}");
            }
        }
        return sb.ToString();
    }
}