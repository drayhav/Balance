namespace Balance.Domain.Exceptions.ValueObjects
{
    public class InvalidTransactionTypeException(string name) : Exception($"Invalid transaction type: {name})")
    {
    }
}
