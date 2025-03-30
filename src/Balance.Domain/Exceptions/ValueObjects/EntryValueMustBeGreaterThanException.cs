namespace Balance.Domain.Exceptions.ValueObjects
{
    public class EntryValueMustBeGreaterThanException(decimal value) 
        : Exception($"Entry value {value} must be greater than 0")
    {
    }
}
