namespace Balance.Domain.ValueObjects
{
    public record Entry(Guid OriginId, EntrySide EntrySide, decimal Value, DateTime OperationDate, DateTime BookingDate);
}