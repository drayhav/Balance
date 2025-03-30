using Balance.Domain.Exceptions.ValueObjects;

namespace Balance.Domain.ValueObjects
{
    public record EntrySide
    {
        private readonly string _name;

        private EntrySide(string name)
        {
            if (!Enum.TryParse<AvailableTypes>(name, out _))
            {
                throw new InvalidEntrySideException(name);
            }

            _name = name;
        }

        public static EntrySide Credit => new EntrySide(nameof(AvailableTypes.Credit));

        public static EntrySide Debit => new EntrySide(nameof(AvailableTypes.Debit));

        public static IEnumerable<string> GetAvailableTypes()
        {
            return Enum.GetNames(typeof(AvailableTypes));
        }

        public override string ToString()
        {
            return _name;
        }

        public EntrySide Opposite()
        {
            return _name == nameof(AvailableTypes.Credit) ? Debit : Credit;
        }

        private enum AvailableTypes
        {
            Credit,
            Debit
        }
    }
}
