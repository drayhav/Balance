namespace Balance.Domain.Exceptions.ValueObjects
{
    public class TransactionExceptions
    {
        private TransactionExceptions() { }

        public class TransactionAlreadyExistsException(Guid guid) 
            : Exception($"Transaction with originId {guid} already exists");

        public class TransactionIsAlreadyCompensatedException(Guid originId) 
            : Exception($"Transaction with originId {originId} is already compensated");

        public class TransactionValueMustBeGreaterThanException(decimal value)
            : Exception($"Transaction value {value} must be greater than -1");

        public class InvalidTransactionTypeException(string name) 
            : Exception($"Invalid transaction type: {name})");
    }
}
