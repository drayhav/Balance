namespace Balance.Domain.Exceptions.ValueObjects
{
    public class TransactionIsAlreadyCompensatedException(Guid originId) : Exception($"Transaction with originId {originId} is already compensated")
    {
    }
}
