using Balance.Domain.ValueObjects;

namespace Balance.Domain.Services
{
    public class PaymentTransactionStrategy : ITransactionStrategy
    {
        public void Execute(Balance balance, List<Transaction> transactions, Guid originId, decimal value, DateTime operationDate, DateTime bookingDate)
        {
            transactions.Add(new Transaction(originId, TransactionType.Payment, value, EntrySide.Credit, DateTime.UtcNow, DateTime.UtcNow));

            // Allocate interest and principal amounts.
            decimal interestAmount = value * 0.8m;
            decimal principalAmount = value * 0.2m;

            var interestComponent = balance.Components.First(c => c.ComponentType == ComponentType.Interest);
            var principalComponent = balance.Components.First(c => c.ComponentType == ComponentType.Principal);

            // If the interest amount is higher than the interest component, allocate the difference to the principal component.  
            if (interestComponent.Difference - interestAmount < 0)
            {
                var difference = interestComponent.Difference - interestAmount;
                interestAmount += difference;
                principalAmount -= difference;
            }

            // If the principal amount is higher than the principal component, allocate the difference to the overpayment component.  
            if (principalComponent.Difference - principalAmount < 0)
            {
                var overpaymentAmount = principalComponent.Difference - principalAmount;
                principalAmount += overpaymentAmount;

                var overpaymentComponent = balance.Components.FirstOrDefault(c => c.ComponentType == ComponentType.Overpayment);
                if (overpaymentComponent == null)
                {
                    balance.CreateComponent(ComponentType.Overpayment);
                    overpaymentComponent = balance.Components.First(c => c.ComponentType == ComponentType.Overpayment);
                }

                overpaymentComponent.AddEntry(originId, EntrySide.Credit, overpaymentAmount, operationDate, bookingDate);
            }

            interestComponent.AddEntry(originId, EntrySide.Debit, interestAmount, operationDate, bookingDate);
            principalComponent.AddEntry(originId, EntrySide.Debit, principalAmount, operationDate, bookingDate);
        }
    }
}
