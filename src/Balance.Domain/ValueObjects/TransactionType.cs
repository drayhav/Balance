using Balance.Domain.Exceptions;
using Balance.Domain.Exceptions.ValueObjects;

namespace Balance.Domain.ValueObjects
{
    public record TransactionType
    {
        private readonly string _name;

        private TransactionType(string name)
        {
            if (!Enum.TryParse<AvailableTypes>(name, out _))
            {
                throw new InvalidTransactionTypeException(name);
            }

            _name = name;
        }

        public static TransactionType Payment => new TransactionType(nameof(AvailableTypes.Payment));
        public static TransactionType Interest => new TransactionType(nameof(AvailableTypes.Interest));
        public static TransactionType Compensation => new TransactionType(nameof(AvailableTypes.Compensation));
        public static TransactionType Opening => new TransactionType(nameof(AvailableTypes.Opening));

        public static IEnumerable<string> GetAvailableTypes()
        {
            return Enum.GetNames(typeof(AvailableTypes));
        }

        public override string ToString()
        {
            return _name;
        }

        private enum AvailableTypes
        {
            Opening,
            Payment,
            Interest,
            Compensation
        }
    }
}