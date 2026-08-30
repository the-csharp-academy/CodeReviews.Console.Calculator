using CalculatorLibrary;
using System.Text.RegularExpressions;

namespace ConsoleCalculator.Brandonscodedata
{
    internal class Program
    {
        private static List<CalcData> calculationHistory = new List<CalcData>();
        static void Main(string[] args)
        {
            bool usePreviousResult = false;
            double savedResult = 0;
            bool endApp = false;
            // Display title as the C# console calculator app.
            Console.WriteLine("Console Calculator in C#\r");
            Console.WriteLine("------------------------\n");
            // Program.cs
            Calculator calculator = new Calculator();
            while (!endApp)
            {
                // Declare variables and set to empty.
                // Use Nullable types (with ?) to match type of System.Console.ReadLine
                string? numInput1 = "";
                string? numInput2 = "";
                double result = 0;

                // Ask the user to type the first number.
                if (usePreviousResult)
                {
                    numInput1 = savedResult.ToString();
                    Console.WriteLine($"Using previous result: {numInput1}");
                }
                else
                {
                    Console.Write("Type a number, and then press Enter: ");
                    numInput1 = Console.ReadLine();
                }

                double cleanNum1 = 0;
                while (!double.TryParse(numInput1, out cleanNum1))
                {
                    Console.Write("This is not valid input. Please enter a numeric value: ");
                    numInput1 = Console.ReadLine();
                }

                // Ask the user to type the second number.
                Console.Write("Type another number, and then press Enter: ");
                numInput2 = Console.ReadLine();

                double cleanNum2 = 0;
                while (!double.TryParse(numInput2, out cleanNum2))
                {
                    Console.Write("This is not valid input. Please enter a numeric value: ");
                    numInput2 = Console.ReadLine();
                }

                // Ask the user to choose an operator.
                Console.WriteLine("Choose an operator from the following list:");
                Console.WriteLine("\ta - Add");
                Console.WriteLine("\ts - Subtract");
                Console.WriteLine("\tm - Multiply");
                Console.WriteLine("\td - Divide");
                Console.WriteLine("\tsq - Square Root of number1");
                Console.WriteLine("\tsq2 - number2 Root of number1");
                Console.WriteLine("\t10 - 10x");
                Console.WriteLine("\tcos - Cosine of number1");
                Console.WriteLine("\tsin - Sine of number1");
                Console.WriteLine("\t tan - Tangent of number1");
                Console.WriteLine("\tlog - Logarithm base 10 of number1");
                Console.Write("Your option? ");

                string? op = Console.ReadLine();

                // Validate input is not null, and matches the pattern
                if (op == null || !Regex.IsMatch(op, "^(a|s|m|d|sq|sq2|10|cos|sin|tan|log)$"))
                {
                    Console.WriteLine("Error: Unrecognized input.");
                }
                else
                {
                    try
                    {
                        // Program.cs
                        result = calculator.DoOperation(cleanNum1, cleanNum2, op);
                        if (double.IsNaN(result))
                        {
                            Console.WriteLine("This operation will result in a mathematical error.\n");
                        }
                        else
                        {
                            CalcData currentCalcData = new CalcData(calculationHistory.Count + 1, cleanNum1, cleanNum2, op.Trim(), result);
                            calculationHistory.Add(currentCalcData);

                            Console.WriteLine("Your result: {0:0.##}\n", result);
                            Console.WriteLine($"The Calculator was used {calculationHistory.Count} times.");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                    }
                }
                Console.WriteLine("------------------------\n");

                // Wait for the user to respond before closing.
                Console.WriteLine("Press 'h' and Enter to view calculation history:");
                Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
                string userMenuInput = Console.ReadLine();
                if (userMenuInput == "n") endApp = true;
                if (userMenuInput == "h")
                {
                    Console.WriteLine("\nCalculation History:");
                    foreach (var calcData in calculationHistory)
                    {
                        Console.WriteLine($"Round {calcData.CalculationRound}: {calcData.CalculationNumberOne} {calcData.CalculationOperator} {calcData.CalculationNumberTwo} = {calcData.CalculationResult}");
                    }
                    Console.WriteLine("Do you want to delete the calculation history? Press 'd' to delete.");
                    Console.WriteLine("Do you want to use the Result for another calculation? Press 'u' to use.");

                    string historyInput = Console.ReadLine();
                    if (historyInput == "d")
                    {
                        calculationHistory.Clear();
                        Console.WriteLine("Calculation history deleted.");
                    }
                    if (historyInput == "u")
                    {
                        // Implement logic for using the result in another calculation
                        Console.WriteLine("Which calculation do you want to use? Enter the round number.");
                        if (int.TryParse(Console.ReadLine(), out int roundNumber))
                        {
                            var selectedCalc = calculationHistory.FirstOrDefault(c => c.CalculationRound == roundNumber);
                            if (selectedCalc != null)
                            {
                                Console.WriteLine($"Using result {selectedCalc.CalculationResult} from round {roundNumber}.");
                                // You can now use selectedCalc.CalculationResult for further calculations
                                // For example, you could set it as numInput1 for the next calculation
                                savedResult = selectedCalc.CalculationResult;
                                usePreviousResult = true;
                            }
                            else
                            {
                                usePreviousResult=false;
                                Console.WriteLine("Invalid round number.");
                            }

                        }
                        else
                        {
                            usePreviousResult = false;
                            Console.WriteLine("Invalid input. Please enter a valid round number.");
                        }
                    }
                    else
                    {
                        usePreviousResult = false;
                    }

                    Console.WriteLine("\n"); // Friendly linespacing.
                }

            }
            calculator.Finish();
            return;
        }
    }    
}
