using Balance.Domain.ValueObjects;

namespace Balance.Domain.Exceptions.ValueObjects
{
    public class ComponentExceptions
    {
        private ComponentExceptions() { }

        public class ComponentAlreadyExistsException(ComponentType componentType) 
            : Exception($"Component {componentType} already exists");

        public class ComponentNotFoundException(ComponentType componentType)
            : Exception($"Component of type {componentType} was not found");

        public class InvalidComponentTypeException(string name) 
            : Exception($"Invalid component type{name}");
    }
}
