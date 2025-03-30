using Balance.Domain.Exceptions.ValueObjects;

namespace Balance.Domain.ValueObjects
{
    public record ComponentType
    {
        private readonly string _name;

        private ComponentType(string name)
        {
            if (!Enum.TryParse<AvailableTypes>(name, out _))
            {
                throw new InvalidComponentTypeException(name);
            }

            _name = name;
        }

        public static ComponentType Interest => new ComponentType(nameof(AvailableTypes.Interest));

        public static ComponentType Principal => new ComponentType(nameof(AvailableTypes.Principal));

        public static ComponentType Overpayment => new ComponentType(nameof(AvailableTypes.Overpayment));

        public static IEnumerable<string> GetAvailableTypes()
        {
            return Enum.GetNames(typeof(AvailableTypes));
        }

        public override string ToString()
        {
            return _name;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode();
        }

        private enum AvailableTypes
        {
            Interest,
            Principal,
            Overpayment
        }
    }
}
