using CalculatorLibrary;
using System.Text.RegularExpressions;

namespace calculator_arjei_io
{

    class Program
    {
        static void Main(string[] args)
        {
            bool endApp = false;

            Console.WriteLine("Console Calculator in C#\r");
            Console.WriteLine("------------------------\n");

            Calculator calculator = new Calculator();
            while (!endApp)
            {

                string? numInput1 = "";
                string? numInput2 = "";
                double result = 0;
                Console.Clear();
                Console.Write("Type a number, and then press Enter: ");
                numInput1 = Console.ReadLine();

                double cleanNum1 = 0;
                while (!double.TryParse(numInput1, out cleanNum1))
                {
                    Console.Write("This is not valid input. Please enter an integer value: ");
                    numInput1 = Console.ReadLine();
                }

                Console.Write("Type another number, and then press Enter: ");
                numInput2 = Console.ReadLine();

                double cleanNum2 = 0;
                while (!double.TryParse(numInput2, out cleanNum2))
                {
                    Console.Write("This is not valid input. Please enter an integer value: ");
                    numInput2 = Console.ReadLine();
                }

                Console.Clear();
                Console.WriteLine($"Your first number: {cleanNum1}");
                Console.WriteLine($"Your second number: {cleanNum2}");
                Console.WriteLine("Enter operation to carry out...");
                Console.WriteLine($"\ta - Addition");
                Console.WriteLine($"\ts - Subtraction");
                Console.WriteLine($"\tm - Multiplication");
                Console.WriteLine($"\td - Division");
                Console.WriteLine($"\t--------------------------------------------");
                Console.WriteLine($"\tThese operations only use the first number");
                Console.WriteLine($"\tr - Square_root");
                Console.WriteLine($"\tx - X10");
                Console.WriteLine($"\tp - Taking the power");
                Console.WriteLine($"\tsin - Sine");
                Console.WriteLine($"\tcos - Cosine");
                Console.WriteLine($"\ttan - Tangent");
                Console.WriteLine($"\tasin - Arcsine");
                Console.WriteLine($"\tacos - Arccosine");
                Console.WriteLine($"\tatan - Arctangent");
                Console.WriteLine($"\n\tn - exit application\n");

                Console.Write("Your option? ");

                string? op = Console.ReadLine();

                // Validate input is not null, and matches the pattern
                if (op == null || !Regex.IsMatch(op, "[a|s|m|d|r|x|p|sin|cos|tan|asin|acos|atan|n]"))
                {
                    Console.WriteLine("Error: Unrecognized input.");
                }
                else
                {
                    try
                    {
                        result = calculator.DoOperation(cleanNum1, cleanNum2, op);
                        if (double.IsNaN(result))
                        {
                            Console.WriteLine("This operation will result in a mathematical error.\n");
                        }
                        else Console.WriteLine("Your result: {0:0.##}\n", result);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                    }
                }
                Console.WriteLine("------------------------\n");

                Console.WriteLine("Total calculations done: " + calculator.timesUsed);
                Console.WriteLine(@"
Enter 'h' to view history
Enter to continue");

                string? userInput = Console.ReadLine().Trim().ToLower();

                if (userInput == "h")
                {
                    Console.Clear();
                    calculator.DisplayList();

                }
                else if (userInput == "n") endApp = true;

                Console.WriteLine("\n");
            }
            calculator.Finish();
            return;
        }
    }
}
