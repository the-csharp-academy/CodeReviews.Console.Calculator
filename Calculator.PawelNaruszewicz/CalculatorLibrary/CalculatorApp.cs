using Calculator.PawelNaruszewicz;

namespace CalculatorLibrary
{
    public class CalculatorApp
    {
        public int numberOfCalculations { get; private set; }
        public double Result = 0;
        public JsonLogger JsonLogger = new JsonLogger();
        public OperationLogger OperationLogger = new OperationLogger();
        public CalculatorApp()
        {
            numberOfCalculations = 0;
        }
        public void DoOperation(char operation)
        {
            IOperation chosenOperation = GetChoiceOfOperation(operation);

            double[] inputs = new double[chosenOperation.RequiredArguments];

            for (int i = 0; i < chosenOperation.RequiredArguments; i++)
            {
                inputs[i] = ValidateNumberFromInput($"Type number {i + 1} and then press Enter");
            }

            Result = chosenOperation.Execute(inputs);

            OperationLogger.LogOperation(Result, chosenOperation, inputs);
            JsonLogger.LogOperation(Result, chosenOperation, inputs);
            numberOfCalculations++;

            Console.WriteLine($"Result of {chosenOperation.Name} is {Result}");
        }
        public void DoOperation(double leftNumber, char operation)
        {
            IOperation chosenOperation = GetChoiceOfOperation(operation);

            double[] inputs = new double[chosenOperation.RequiredArguments];
            inputs[0] = leftNumber;

            for (int i = 1; i < chosenOperation.RequiredArguments; i++)
            {
                inputs[i] = ValidateNumberFromInput($"Type number {i + 1} and then press Enter");
            }

            Result = chosenOperation.Execute(inputs);

            OperationLogger.LogOperation(Result, chosenOperation, inputs);
            JsonLogger.LogOperation(Result, chosenOperation, inputs);
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
                'd' => new Divide(),
                'r' => new SquareRoot(),
                'p' => new PowerOf()
            };
            return chosenOperation;
        }
    }
}
