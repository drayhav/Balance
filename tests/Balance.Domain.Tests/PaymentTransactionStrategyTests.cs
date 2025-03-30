using Balance.Domain.Services;
using Balance.Domain.ValueObjects;

namespace Balance.Domain.Tests
{
    public class PaymentTransactionStrategyTests
    {
        [Fact]
        public void Execute_GivenPaymentThatDoesntExeedCurrentDebt_ShouldAllocateInterestAndPrincipal()
        {
            // Arrange
            var balance = new Balance(1000, 500, DateTime.UtcNow, DateTime.UtcNow);
            var transactions = new List<Transaction>();
            var strategy = new PaymentTransactionStrategy();
            var originId = Guid.NewGuid();
            var value = 100m;
            var operationDate = DateTime.UtcNow;
            var bookingDate = DateTime.UtcNow;

            // Act
            strategy.Execute(balance, transactions, originId, value, operationDate, bookingDate);

            // Assert
            var interestComponent = balance.Components.First(c => c.ComponentType == ComponentType.Interest);
            var principalComponent = balance.Components.First(c => c.ComponentType == ComponentType.Principal);

            Assert.Equal(80m, interestComponent.Entries.Last().Value);
            Assert.Equal(20m, principalComponent.Entries.Last().Value);
        }

        [Theory]
        [InlineData(100, 100, 1000)]
        public void Execute_GivenPaymentThatExeedsCurrentInterestsAndPrincipal_ShouldHandleOverpaymentAndCreateOverpaymentComponent(decimal interest, decimal principal, decimal paymentAmount)
        {
            // Arrange
            var balance = new Balance(interest, principal, DateTime.UtcNow, DateTime.UtcNow);
            var transactions = new List<Transaction>();
            var strategy = new PaymentTransactionStrategy();
            var originId = Guid.NewGuid();
            var operationDate = DateTime.UtcNow;
            var bookingDate = DateTime.UtcNow;

            // Act
            strategy.Execute(balance, transactions, originId, paymentAmount, operationDate, bookingDate);

            // Assert
            var overpaymentComponent = balance.Components.FirstOrDefault(c => c.ComponentType == ComponentType.Overpayment);
            Assert.NotNull(overpaymentComponent);
            Assert.Equal(-800, overpaymentComponent.Entries.Last().Value);

            Assert.Equal(0, balance.Components.First(c => c.ComponentType == ComponentType.Principal).Difference);
            Assert.Equal(0, balance.Components.First(c => c.ComponentType == ComponentType.Interest).Difference);
            Assert.Equal(-800, balance.Components.First(c => c.ComponentType == ComponentType.Overpayment).Difference);
        }
    }
}
