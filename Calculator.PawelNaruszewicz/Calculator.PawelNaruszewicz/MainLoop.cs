using CalculatorLibrary;

namespace Calculator.PawelNaruszewicz
{
    public class MainLoop
    {
        MenuDisplay menuDisplay = new MenuDisplay();
        public bool _endApp = false;

        public void Run()
        {
            Console.WriteLine("Console Calculator in C#\r");
            Console.WriteLine("------------------------\n");

            CalculatorApp calculator = new CalculatorApp();

            while (!_endApp)
            {
                char chosenAction = menuDisplay.DisplayMainMenu();
                switch (chosenAction)
                {
                    case '1':
                        char chosenOperation = menuDisplay.GetChoiceOfOperation();
                        calculator.DoOperation(chosenOperation);
                        break;
                    case '2':
                        calculator.OperationLogger.DisplayLatestCalculation();
                        break;
                    case '3':
                        calculator.OperationLogger.DeleteLatestCalculationList();
                        break;
                    case '4':
                        if (calculator.OperationLogger.ListIsEmpty == true)
                        {
                            Console.WriteLine("List is empty");
                            break;
                        }
                        else
                        {
                            double numberToUse = calculator.OperationLogger.GetLatestResult();
                            char chosenOption = menuDisplay.GetChoiceOfOperation();

                            calculator.DoOperation(numberToUse, chosenOption);
                            break;
                        }

                    case 'n':
                        _endApp = true;
                        break;
                    default:
                        break;
                }
                Console.WriteLine("\nPress any button to continue");
                Console.ReadKey();

                Console.WriteLine("\n------------------------\n");
            }
            calculator.JsonLogger.Finish();
            Console.WriteLine($"You've used the calculator {calculator.numberOfCalculations} amount of times!");
            return;
        }

    }
}
