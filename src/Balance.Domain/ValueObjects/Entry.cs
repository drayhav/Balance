using Balance.Domain.Exceptions.ValueObjects;

namespace Balance.Domain.ValueObjects
{
    public record Entry(Guid OriginId, EntrySide EntrySide, decimal Value, DateTime OperationDate, DateTime BookingDate)
    {
        public Entry(Guid originId, EntrySide entrySide, decimal value, DateTime operationDate)
            : this(originId, entrySide, value, operationDate, operationDate)
        {
            if (value <= 0)
            {
                throw new EntryValueMustBeGreaterThanException(value);
            }
        }
    }
}