namespace Balance.Domain.Exceptions.ValueObjects
{
    public class InvalidEntrySideException(string name) : Exception($"Invalid entry side {name}")
    {
    }
}
