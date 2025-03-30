using Balance.Domain.Services;
using Balance.Domain.ValueObjects;

namespace Balance.Domain.Factories
{
    public static class TransactionStrategyFactory
    {
        public static ITransactionStrategy GetStrategy(TransactionType transactionType)
        {
            return transactionType switch
            {
                var t when t == TransactionType.Payment => new PaymentTransactionStrategy(),
                var t when t == TransactionType.Compensation => new CompensationTransactionStrategy(),
                _ => throw new NotImplementedException($"No strategy implemented for transaction type {transactionType}")
            };
        }
    }
}