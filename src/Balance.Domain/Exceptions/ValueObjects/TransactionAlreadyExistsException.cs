namespace Balance.Domain.Exceptions.ValueObjects
{
    public class TransactionAlreadyExistsException(Guid guid) : Exception($"Transaction with originId {guid} already exists")
    {
    }
}
