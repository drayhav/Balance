namespace Balance.Domain.Exceptions
{
    public class TransactionNotFoundException(Guid originId) 
        : Exception($"Transaction with origin:{originId} was not found")
    {
    }
}
