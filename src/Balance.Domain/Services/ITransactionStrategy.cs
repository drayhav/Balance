using Balance.Domain.ValueObjects;

namespace Balance.Domain.Services
{
    public interface ITransactionStrategy
    {
        void Execute(Balance balance, List<Transaction> transactions, Guid originId, decimal value, DateTime operationDate, DateTime bookingDate);
    }
}