using Balance.Domain.Exceptions.ValueObjects;
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

        public decimal CurrentValue => _components.Sum(c => c.Difference);

        public Balance(decimal interest, decimal principal, DateTime operationDate, DateTime bookingDate)
        {
            Id = Guid.CreateVersion7();
            CreateComponent(ComponentType.Interest);
            CreateComponent(ComponentType.Principal);

            var interestComponent = _components.First(c => c.ComponentType == ComponentType.Interest);
            var principalComponent = _components.First(c => c.ComponentType == ComponentType.Principal);

            interestComponent.AddEntry(Id, EntrySide.Credit, interest, operationDate, bookingDate);
            principalComponent.AddEntry(Id, EntrySide.Credit, principal, operationDate, bookingDate);

            _transactions.Add(new Transaction(originId: Id, transactionType: TransactionType.Opening, value: interest + principal, EntrySide.Debit, operationDate, bookingDate));
        }

        public void CreateComponent(ComponentType componentType)
        {
            if (_components.Any(c => c.ComponentType == componentType))
            {
                throw new ComponentExceptions.ComponentAlreadyExistsException(componentType);
            }

            _components.Add(new Component(componentType));
        }

        public void RegisterTransaction(Guid originId, TransactionType transactionType, decimal value, DateTime operationDate, DateTime bookingDate)
        {
            if (_transactions.Any(t => t.OriginId == originId))
            {
                throw new TransactionExceptions.TransactionAlreadyExistsException(originId);
            }

            // Need to add a logic to check whether that transaction happened in the past and we need to compensate potential transactions (interests?) that happened after it
            // and calculate remaining interests.

            var strategy = TransactionStrategyFactory.GetStrategy(transactionType);
            strategy.Execute(this, _transactions, originId, value, operationDate, bookingDate);
        }

        public void CompensateTransaction(Guid originId, DateTime compensationDate)
        {
            if (_transactions.Any(t => t.OriginId == originId && t.TransactionType == TransactionType.Compensation))
            {
                throw new TransactionExceptions.TransactionIsAlreadyCompensatedException(originId);
            }

            // Need to add a logic that will handle compensating also other transactions that happened after the compensation date (?)
            // Possibly only of interest type, but need to check that.

            var strategy = TransactionStrategyFactory.GetStrategy(transactionType: TransactionType.Compensation);
            strategy.Execute(this, _transactions, originId, value: 0, operationDate: compensationDate, bookingDate: compensationDate);
        }
    }
}