using System.Text.RegularExpressions;

namespace Core.ValueObjects;

public class ISBN
{
    public string Value { get; private set; }
    
    public ISBN(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("ISBN cannot be null or empty.", nameof(value));

        if (!IsValid(value))
            throw new ArgumentException("Invalid ISBN format.", nameof(value));

        Value = value;
    }

    private bool IsValid(string isbn)
        => Regex.IsMatch(isbn, @"^\d{3}-\d{1,5}-\d{1,7}-\d{1,7}-\d{1}$");
}