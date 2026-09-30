using Calculator.PawelNaruszewicz;

namespace CalculatorLibrary
{
    public class CalculatorApp
    {
        public int numberOfCalculations { get; private set; }
        private double _leftNumber = 0;
        private double _rightNumber = 0;
        public double Result = 0;
        public JsonLogger JsonLogger = new JsonLogger();
        public OperationLogger OperationLogger = new OperationLogger();
        public CalculatorApp()
        {
            numberOfCalculations = 0;
        }
        public void DoOperation(char operation)
        {
            _leftNumber = ValidateNumberFromInput("Type a number, and then press Enter: ");
            _rightNumber = ValidateNumberFromInput("Type a number, and then press Enter: ");
            IOperation chosenOperation = GetChoiceOfOperation(operation);
            Result = chosenOperation.Execute(_leftNumber, _rightNumber);

            OperationLogger.LogOperation(_leftNumber, _rightNumber, Result, chosenOperation);
            JsonLogger.LogOperation(_leftNumber, _rightNumber, Result, chosenOperation);
            numberOfCalculations++;

            Console.WriteLine($"Result of {chosenOperation.Name} is {Result}");
        }
        public void DoOperation(double leftNumber, char operation)
        {
            _leftNumber = leftNumber;
            _rightNumber = ValidateNumberFromInput("Type a number, and then press Enter: ");
            IOperation chosenOperation = GetChoiceOfOperation(operation);
            Result = chosenOperation.Execute(_leftNumber, _rightNumber);

            OperationLogger.LogOperation(_leftNumber, _rightNumber, Result, chosenOperation);
            JsonLogger.LogOperation(_leftNumber, _rightNumber, Result, chosenOperation);
            numberOfCalculations++;

            Console.WriteLine($"Result of {chosenOperation.Name} is {Result}");
        }

        private double ValidateNumberFromInput(string text)
        {
            double numberToReturn = 0;

            Console.WriteLine(text);
            string? numberInput = Console.ReadLine();

            while (!double.TryParse(numberInput, out numberToReturn))
            {
                Console.Write("This is not valid input. Please enter a numeric value: ");
                numberInput = Console.ReadLine();
            }

            return numberToReturn;
        }
        private IOperation GetChoiceOfOperation(char chosenChar)
        {
            IOperation chosenOperation = chosenChar switch
            {
                'a' => new Add(),
                's' => new Subtract(),
                'm' => new Multiply(),
                'd' => new Divide()
            };
            return chosenOperation;
        }
    }
}
