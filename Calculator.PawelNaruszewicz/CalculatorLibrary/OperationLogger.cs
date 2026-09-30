namespace CalculatorLibrary
{
    public class OperationLogger
    {
        private List<string> latestCalculations = new List<string>();
        private List<double> latestResults = new List<double>();
        public bool ListIsEmpty => latestCalculations.Count == 0;

        public void LogOperation(double leftNumber, double rightNumber, double result, IOperation operation)
        {
            latestResults.Add(result);
            latestCalculations.Add($"{operation.Name} of {leftNumber} and {rightNumber} equals {result}");
        }
        public void DeleteLatestCalculationList()
        {
            latestCalculations.Clear();
            latestResults.Clear();
        }

        public void DisplayLatestCalculation()
        {
            if (latestCalculations.Count == 0)
            {
                Console.WriteLine("List is empty");
                return;
            }

            for (int i = 0; i < latestCalculations.Count; i++)
            {
                Console.WriteLine($"{i} - {latestCalculations[i]}");
            }
        }
        public double GetLatestResult()
        {
            DisplayLatestCalculation();
            Console.WriteLine("Choose which one you want");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int Y))
                {
                    if (Y <= latestResults.Count - 1 && Y >= 0) return latestResults[Y];
                }
            }
        }
    }
}
