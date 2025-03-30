using Balance.Domain.ValueObjects;
using System.Collections.ObjectModel;

namespace Balance.Domain
{
    public class Balance
    {
        private List<Transaction> _transactions = [];
        private List<Component> _components = [];

        public Guid Id { get; private set; }

        public ReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();

        public ReadOnlyCollection<Component> Components => _components.AsReadOnly();

        public Balance()
        {
            Id = Guid.CreateVersion7();
        }

        public Balance(decimal interest, decimal principal, DateTime operationDate, DateTime bookingDate)
        {
            Id = Guid.CreateVersion7();
            CreateComponent(ComponentType.Interest);
            CreateComponent(ComponentType.Principal);

            var interestComponent = _components.First(c => c.ComponentType == ComponentType.Interest);
            var principalComponent = _components.First(c => c.ComponentType == ComponentType.Principal);

            interestComponent.AddEntry(Id, EntrySide.Debit, interest, operationDate, bookingDate);
            principalComponent.AddEntry(Id, EntrySide.Debit, principal, operationDate, bookingDate);
        }

        public void CreateComponent(ComponentType componentType)
        {
            if (_components.Any(c => c.ComponentType == componentType))
            {
                throw new InvalidOperationException($"Component {componentType} already exists");
            }

            _components.Add(new Component(componentType));
        }

        public void RegisterTransaction(Guid originId, TransactionType transactionType, decimal value, DateTime operationDate, DateTime bookingDate)
        {
            if (transactionType == TransactionType.Payment)
            {
                _transactions.Add(new Transaction(originId, transactionType, value, EntrySide.Credit, DateTime.UtcNow, DateTime.UtcNow));

                decimal interestAmount = value * 0.8m;
                decimal principalAmount = value * 0.2m;

                var interestComponent = _components.First(c => c.ComponentType == ComponentType.Interest);
                var principalComponent = _components.First(c => c.ComponentType == ComponentType.Principal);

                if (interestAmount > interestComponent.TotalDebit)
                {
                    principalAmount += (interestAmount - interestComponent.TotalDebit);
                    interestAmount = interestComponent.TotalDebit;
                }

                interestComponent.AddEntry(originId, EntrySide.Debit, interestAmount, operationDate, bookingDate);
                principalComponent.AddEntry(originId, EntrySide.Debit, principalAmount, operationDate, bookingDate);
            }
        }

        public void CompensateTransaction(Guid originId, DateTime compensationDate)
        {
            var transaction = _transactions.FirstOrDefault(t => t.OriginId == originId);

            if (transaction == null)
            {
                throw new InvalidOperationException("Transaction not found");
            }

            // Compensate transaction
            _transactions.Add(new Transaction(originId: originId, transactionType: TransactionType.Compensation, value: transaction.Value, entrySide: transaction.EntrySide.Opposite(),
                DateTime.UtcNow, DateTime.UtcNow));

            // Compensate components
            var interestComponent = _components.First(c => c.ComponentType == ComponentType.Interest);
            var principalComponent = _components.First(c => c.ComponentType == ComponentType.Principal);

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