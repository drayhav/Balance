using Balance.Domain.Factories;
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

            _transactions.Add(new Transaction(originId: Id, transactionType: TransactionType.Opening, value: interest + principal, EntrySide.Debit, operationDate, bookingDate));
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
            var strategy = TransactionStrategyFactory.GetStrategy(transactionType);
            strategy.Execute(this, _transactions, originId, value, operationDate, bookingDate);
        }

        public void CompensateTransaction(Guid originId, DateTime compensationDate)
        {
            var strategy = TransactionStrategyFactory.GetStrategy(transactionType: TransactionType.Compensation);
            strategy.Execute(this, _transactions, originId, value: 0, operationDate: compensationDate, bookingDate: compensationDate);
        }
    }    
}