namespace Balance.Domain.Exceptions.ValueObjects
{
    public class TransactionValueMustBeGreaterThanException(decimal value) 
        : Exception($"Transaction value {value} must be greater than -1")
    {
    }
}
