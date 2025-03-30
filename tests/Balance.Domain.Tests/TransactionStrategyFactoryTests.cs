using Balance.Domain.Factories;
using Balance.Domain.Services;
using Balance.Domain.ValueObjects;

namespace Balance.Domain.Tests
{
    public class TransactionStrategyFactoryTests
    {
        [Fact]
        public void GetStrategy_ShouldReturnPaymentTransactionStrategy_WhenTransactionTypeIsPayment()
        {
            // Arrange
            var transactionType = TransactionType.Payment;

            // Act
            var strategy = TransactionStrategyFactory.GetStrategy(transactionType);

            // Assert
            Assert.IsType<PaymentTransactionStrategy>(strategy);
        }

        [Fact]
        public void GetStrategy_ShouldReturnCompensationTransactionStrategy_WhenTransactionTypeIsCompensation()
        {
            // Arrange
            var transactionType = TransactionType.Compensation;

            // Act
            var strategy = TransactionStrategyFactory.GetStrategy(transactionType);

            // Assert
            Assert.IsType<CompensationTransactionStrategy>(strategy);
        }
    }
}
