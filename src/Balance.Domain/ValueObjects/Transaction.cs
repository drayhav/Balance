using Balance.Domain.Exceptions.ValueObjects;

namespace Balance.Domain.ValueObjects
{
    public record Transaction
    {
        public Transaction(Guid originId, TransactionType transactionType, decimal value, EntrySide entrySide, DateTime operationDate, DateTime bookingDate)
        {
            if (value < 0)
            {
                throw new TransactionValueMustBeGreaterThanException(value);
            }

            OriginId = originId;
            Value = value;
            EntrySide = entrySide;
            TransactionType = transactionType;
            OperationDate = operationDate;
            BookingDate = bookingDate;
        }

        public Guid OriginId { get; init; }

        public TransactionType TransactionType { get; init; }

        public EntrySide EntrySide { get; init; }

        public decimal Value { get; init; }

        public DateTime OperationDate { get; private set; }

        public DateTime BookingDate { get; private set; }
    }
}