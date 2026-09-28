using CalculatorLibrary.Enums;
using CalculatorLibrary.Logging;

namespace CalculatorLibrary
{
    public class CalculatorEngine
    {
        private readonly CalculatorLogWriter _log;
        private readonly OperationRepository _operationRepository;

        public CalculatorEngine()
        {
            _log = new CalculatorLogWriter();
            _operationRepository = new OperationRepository();
        }

        public double DoOperation(double num1, string op, double num2 = 0)
        {
            double result = double.NaN;
            char operation = default;
            bool successfullOperation = true;

            switch (op)
            {
                case "a":
                    operation = '+';
                    result = num1 + num2;
                    var sum = OperationFactory.CreateBasic(num1, num2, operation, result);
                    _operationRepository.Add(sum);
                    break;
                case "s":
                    operation = '-';
                    result = num1 - num2;
                    var subtraction = OperationFactory.CreateBasic(num1, num2, operation, result);
                    _operationRepository.Add(subtraction);
                    break;
                case "m":
                    operation = '*';
                    result = num1 * num2;
                    var multiplication = OperationFactory.CreateBasic(num1, num2, operation, result);
                    _operationRepository.Add(multiplication);
                    break;
                case "d":
                    operation = '/';
                    if (CalculatorMath.TryDivide(num1, num2, out result))
                    {
                        var division = OperationFactory.CreateBasic(num1, num2, operation, result);
                        _operationRepository.Add(division);
                    }
                    else
                        successfullOperation = false;
                    break;
                case "r":
                    operation = '√';
                    if (CalculatorMath.TryCalculateSquareRoot(num1, out result))
                    {
                        var squareRoot = OperationFactory.CreateSquareRoot(num1, operation, result);
                        _operationRepository.Add(squareRoot);
                    }
                    else
                        successfullOperation = false;
                    break;
                case "p":
                    operation = '^';
                    if (CalculatorMath.TryCalculateExponent(num1, num2, out result))
                    {
                        var power = OperationFactory.CreateExponentiation(num1, num2, operation, result);
                        _operationRepository.Add(power);
                    }
                    else
                        successfullOperation = false;
                    break;
                default:
                    Console.WriteLine("This operation does not exist");
                    successfullOperation = false;
                    break;
            }

            if (successfullOperation)
            {
                if (operation == '√')
                    _log.WriteOperation(num1, operation, result);
                else
                    _log.WriteOperation(num1, num2, operation, result);
            }

            return result;
        }

        public double DoTrigonometryOperation(double degrees, TrigonometryFunctions function, string op)
        {
            double result = double.NaN;
            char operation = '°';

            if (CalculatorMath.TryCalculateTrigonometry(degrees, function, out result))
            {
                _log.WriteTrigOperation(degrees, operation, function, result);
                var trigOperation = OperationFactory.CreateTrigonometry(degrees, function, operation, result);
                _operationRepository.Add(trigOperation);
            }

            return result;
        }

        public void Finish()
        {
            _log.Dispose();
            _operationRepository.PrintAll();
        }

        public void PrintAllOperations()
        {
            _operationRepository.PrintAll();
        }

        public void DeleteOperations()
        {
            Console.WriteLine("\nDeleting all current operations:\n");
            _operationRepository.Clear();
            _log.DeleteOperations();
        }
    }
}
