namespace CalculatorLibrary.Services;

internal class HistoryService
{
    private readonly List<double> _history = [];

    internal IReadOnlyList<double> Get()
    {
        return _history.AsReadOnly();
    }

    internal void Add(double entry)
    {
        if (_history.Count == 5)
        {
            _history.RemoveAt(4);
        }

        _history.Insert(0, entry);
    }

    internal bool Remove(int index)
    {
        if (index > 0 && index <= _history.Count)
        {
            _history.RemoveAt(index - 1);
            return true;
        }

        return false;
    }

    internal void Clear()
    {
        _history.Clear();
    }

    internal bool TryGetEntry(int position, out double result)
    {
        result = default;
        
        if (position < 1 || position > _history.Count)
        { 
            return false;
        }

        result = _history[position - 1];
        return true;
    }
}