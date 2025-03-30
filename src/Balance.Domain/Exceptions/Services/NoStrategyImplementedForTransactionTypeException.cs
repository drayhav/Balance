using Balance.Domain.ValueObjects;

namespace Balance.Domain.Exceptions.Services
{
    public class NoStrategyImplementedForTransactionTypeException(TransactionType transactionType) 
        : Exception($"No strategy implemented for transaction type {transactionType}")
    {
    }
}
