using System.Collections.ObjectModel;

namespace Balance.Domain.ValueObjects
{
    public record Component(ComponentType ComponentType)
    {
        private List<Entry> _entries = [];

        public ReadOnlyCollection<Entry> Entries => _entries.AsReadOnly();

        public decimal TotalDebit => _entries.Where(e => e.EntrySide == EntrySide.Debit).Sum(e => e.Value);

        public decimal TotalCredit => _entries.Where(e => e.EntrySide == EntrySide.Credit).Sum(e => e.Value);

        public void AddEntry(Guid originId, EntrySide entrySide, decimal value, DateTime operationDate, DateTime bookingDate)
        {
            _entries.Add(new Entry(originId, entrySide, value, operationDate, bookingDate));
        }
    }
}