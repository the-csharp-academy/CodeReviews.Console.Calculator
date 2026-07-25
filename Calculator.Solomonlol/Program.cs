using CalculatorLibrary;
using System.Text.RegularExpressions;

namespace CalculatorProgram
{

    class Program
    {
        static void Main(string[] args)
        {
            int countOfUse = 0;
            bool endApp = false;
            // Declare variables and set to empty.
            // Use Nullable types (with ?) to match type of System.Console.ReadLine
            
            
            // Display title as the C# console calculator app.
            Console.WriteLine("Console Calculator in C#\r");
            Console.WriteLine("------------------------\n");

            Calculator calculator = new Calculator();
            while (!endApp)
            {
                

                // Ask the user to choose an operator.
                Console.WriteLine("Choose an operator from the following list:");
                Console.WriteLine("\ta - Add (number1 + number2)");
                Console.WriteLine("\ts - Subtract (number1 - number2)");
                Console.WriteLine("\tm - Multiply (number1 * number2)");
                Console.WriteLine("\td - Divide( number1 / number2)");
                Console.WriteLine("\tp - Power (number1^number2)");
                Console.WriteLine("\tq - Square Root (sqrt(number1))");
                Console.WriteLine("\tt - 10x (number1 * 10)");
                Console.WriteLine("\th - Show History");
                Console.WriteLine("\tz - Delete History");
                Console.Write("Your option? ");

                string? op = Console.ReadLine();

                // Validate input is not null, and matches the pattern
                if (op == null || !Regex.IsMatch(op, "^[asmdhzqpt]$"))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Unrecognized input.");
                    Console.ResetColor();
                }
                else if(Regex.IsMatch(op, "^[h]$"))
                {
                    calculator.ShowHistory();
                }
                else if (Regex.IsMatch(op, "^[z]$"))
                {
                    calculator.DeleteHistory();
                }
                else
                {
                    try
                    {
                        Operation(op, calculator, ref countOfUse);
                    }
                    catch (Exception e)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                        Console.ResetColor();
                    }
                }
                Console.WriteLine("------------------------\n");

                // Wait for the user to respond before closing.
                Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
                if (Console.ReadLine() == "n") endApp = true;
                Console.Clear();
                Console.WriteLine("\n"); // Friendly linespacing.
            }
            calculator.Finish();
            return;
        }
        static void Operation(string op, Calculator calculator, ref int countOfUse)
        {
            
            string? numInput1 = "";
            string? numInput2 = "";
            double result = 0;
            double cleanNum1 = 0;
            double cleanNum2 = 0;
            // Ask the user to type the first number.

            Console.Write("Type a number, and then press Enter: ");
            numInput1 = Console.ReadLine();


            while (!double.TryParse(numInput1, out cleanNum1))
            {
                Console.Write("This is not valid input. Please enter an integer value: ");
                numInput1 = Console.ReadLine();
            }
            if (!Regex.IsMatch(op, "^[qt]$"))
                {
                // Ask the user to type the second number.
                Console.Write("Type another number, and then press Enter: ");
                numInput2 = Console.ReadLine();


                while (!double.TryParse(numInput2, out cleanNum2))
                {
                    Console.Write("This is not valid input. Please enter an integer value: ");
                    numInput2 = Console.ReadLine();
                }
                result = calculator.DoOperation(cleanNum1, cleanNum2, op);
            }
            else
            {
                result = calculator.DoOperation(cleanNum1, op);
            }
            if (double.IsNaN(result))
            {
                Console.WriteLine("This operation will result in a mathematical error.\n");
            }
            else
            {
                countOfUse++;
                Console.WriteLine($"Your result: {result:0.##}\n" +
                    $"Calcularor was used {countOfUse} times");
            }
        }
    }
   
}