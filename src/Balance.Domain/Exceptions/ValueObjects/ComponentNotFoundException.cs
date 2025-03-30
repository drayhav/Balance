using Balance.Domain.ValueObjects;

namespace Balance.Domain.Exceptions.ValueObjects
{
    public class ComponentNotFoundException(ComponentType componentType) 
        : Exception($"Component of type {componentType} was not found")
    {
    }
}
