using Balance.Domain.ValueObjects;

namespace Balance.Domain.Services
{
    public class CompensationTransactionStrategy : ITransactionStrategy
    {
        public void Execute(Balance balance, List<Transaction> transactions, Guid originId, decimal value, DateTime operationDate, DateTime bookingDate)
        {
            var transaction = transactions.FirstOrDefault(t => t.OriginId == originId) 
                ?? throw new InvalidOperationException("Transaction not found");

            // Compensate transaction
            transactions.Add(new Transaction(originId: originId, transactionType: TransactionType.Compensation, value: transaction.Value, entrySide: transaction.EntrySide.Opposite(),
                DateTime.UtcNow, DateTime.UtcNow));

            // Compensate components
            var interestComponent = balance.Components.First(c => c.ComponentType == ComponentType.Interest);
            var principalComponent = balance.Components.First(c => c.ComponentType == ComponentType.Principal);

            var interestEntries = interestComponent.Entries.Where(e => e.OriginId == originId).ToList();
            var principalEntries = principalComponent.Entries.Where(e => e.OriginId == originId).ToList();

            foreach (var interestEntry in interestEntries)
            {
                interestComponent.AddEntry(originId, interestEntry.EntrySide.Opposite(), interestEntry.Value, DateTime.UtcNow, DateTime.UtcNow);
            }

            foreach (var principalEntry in principalEntries)
            {
                principalComponent.AddEntry(originId, principalEntry.EntrySide.Opposite(), principalEntry.Value, DateTime.UtcNow, DateTime.UtcNow);
            }
        }
    }
}