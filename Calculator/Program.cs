using System.ComponentModel.Design;
using System.Net;
using System.Text.RegularExpressions;
using CalculatorLibrary;


class Program
{
    static void Main(string[] args)
    {
        bool endApp = false;
        // Display title as the C# console calculator app.
        Console.WriteLine("Console Calculator in C#\r");
        Console.WriteLine("------------------------\n");
        Calculator calculator = new Calculator();

        while (!endApp)
        {
            // Declare variables and set to empty.
            // Use Nullable types (with ?) to match type of System.Console.ReadLine
            string? numInput1 = "";
            string? numInput2 = "";
            double result = 0;

            // Ask the user to type the first number.
            Console.Write("Type a number, and then press Enter: ");
            numInput1 = Console.ReadLine();

            double cleanNum1 = 0;
            while (!double.TryParse(numInput1, out cleanNum1))
            {
                Console.Write("This is not valid input. Please enter a numeric value: ");
                numInput1 = Console.ReadLine();
            }


            // Ask the user to choose an operator.
            Console.WriteLine("Choose a style from the following list:");
            Console.WriteLine("\ta - Advanced Math");
            Console.WriteLine("\tb - Basic Math");
            Console.Write("Your option? ");
            string? style = Console.ReadLine();

            while (style != "b" && style != "a")
            {
                Console.Write("This is not valid input.");
                style = Console.ReadLine();
            }

            if (style == "a")
            {
                AdvancedMath();
            }
            else if (style == "b")
            {
                BasicMath();
            }

            void BasicMath()
            {

                Console.WriteLine("Choose an Operator from the following list:");
                Console.WriteLine("\ta - Addition");
                Console.WriteLine("\ts - Subtraction");
                Console.WriteLine("\tm - Multiplication");
                Console.WriteLine("\td - Division");
                Console.Write("Your option? ");
                string? op = Console.ReadLine();

                if (op == null || !Regex.IsMatch(op, "^(a|s|m|d)$"))
                {
                    Console.WriteLine("Error: Unrecognized input.");
                    op = Console.ReadLine();
                }

                Console.Write("Type another number, and then press Enter: ");
                numInput2 = Console.ReadLine();

                double cleanNum2 = 0;
                while (!double.TryParse(numInput2, out cleanNum2))
                {
                    Console.Write("This is not valid input. Please enter a numeric value: ");
                    numInput2 = Console.ReadLine();
                }

                try
                {
                    result = calculator.Basic(cleanNum1, cleanNum2, op);
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


                Console.WriteLine("------------------------\n");

                // Wait for the user to respond before closing.
                Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
                if (Console.ReadLine() == "n") endApp = true;

                Console.WriteLine("\n"); // Friendly linespacing.
            }

            void AdvancedMath()
            {
                Console.WriteLine("Choose an operator from the following list:");
                Console.WriteLine("\tr - Sqaure Root");
                Console.WriteLine("\tl - Logarithm ");
                Console.WriteLine("\tp - Power");
                Console.Write("Your option? ");
                string? op = Console.ReadLine();

                if (op == null || !Regex.IsMatch(op, "^(r|l|p)$"))
                {
                    Console.WriteLine("Error: Unrecognized input.");
                }

                double cleanNum2 = 0;

                // Only power requires a second number
                if (op == "p")
                {
                    cleanNum2 = GetSecondNumber();
                }

                try
                {
                    result = calculator.Advanced(cleanNum1, cleanNum2, op);
                    if (double.IsFinite(cleanNum1))
                    {
                        Console.WriteLine("This operation will result in a mathematical error.\n");
                    }
                    else Console.WriteLine("Your result: {0:0.##}\n", result);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                }

                double GetSecondNumber()
                {
                    Console.Write("Type another number, and then press Enter: ");
                    string? numInput2 = Console.ReadLine();

                    double cleanNum2 = 0;

                    while (!double.TryParse(numInput2, out cleanNum2))
                    {
                        Console.Write("Invalid input. Enter a numeric value: ");
                        numInput2 = Console.ReadLine();
                    }

                    return cleanNum2;
                }

                Console.WriteLine("------------------------\n");

                // Wait for the user to respond before closing.
                Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
                if (Console.ReadLine() == "n") endApp = true;

                Console.WriteLine("\n"); // Friendly linespacing.
            }

        }

        calculator.Finish();
        return;
    }
}