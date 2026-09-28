using CalculatorLibrary.Models;

namespace CalculatorLibrary
{
    public class OperationRepository
    {
        private readonly List<MathOperation> _operations = new();

        public IReadOnlyList<MathOperation> Operations => _operations;

        public void Add(MathOperation op)
        {
            _operations.Add(op);
            Console.WriteLine($"Operation: {op}\n");
        }

        public void Clear() => _operations.Clear();

        public int Count => _operations.Count;

        public void PrintAll()
        {
            var countTimes = _operations.Count == 1 ? "time" : "times";
            Console.WriteLine($"\nThe calculator was used {_operations.Count} {countTimes}.\n");

            if (_operations.Count == 0)
            {
                Console.WriteLine("\nThere are no calculations to display:\n");
                return;
            }

            Console.WriteLine("Latest calculations:\n");
            foreach (var (index, operation) in _operations.Select((o, i) => (i, o)))
            {
                Console.WriteLine($"{index + 1}. {operation}");
            }
        }
    }
}
