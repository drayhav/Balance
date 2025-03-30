namespace Balance.Domain.Exceptions.ValueObjects
{
    public class InvalidComponentTypeException(string name) : Exception($"Invalid component type{name}")
    {
    }
}
