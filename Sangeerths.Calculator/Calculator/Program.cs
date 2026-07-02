using System.Text.RegularExpressions;
using static CalculatorLibrary.CalculatorLibrary;

class Program
{
    static void Main(string[] args)
    {
        bool endApp = false;
        int usageCount = 0;
        List<double> history = new List<double>();

        // Display title as the C# console calculator app.
        Console.WriteLine("Console Calculator in C#\r");
        Console.WriteLine("------------------------\n");

        Calculator calculator = new Calculator();

        

        while (!endApp)
        {
            
            double result = 0;

            // Ask the user to choose an operator.
            Console.WriteLine("Choose an operator from the following list:");
            Console.WriteLine("\ta - Add");
            Console.WriteLine("\ts - Subtract");
            Console.WriteLine("\tm - Multiply");
            Console.WriteLine("\td - Divide");
            Console.WriteLine("\tr - Root");
            Console.WriteLine("\tp - Power");
            Console.WriteLine("\tsin - Sine");
            Console.WriteLine("\tcos - Cosine");
            Console.WriteLine("\ttan - Tan");


            Console.WriteLine("\th - Show calculation history");
            Console.WriteLine("\tc - Clear calculation history");
            Console.Write("Your option? ");

            string? op = Console.ReadLine();

            // Validate input is not null, and matches the pattern
            if (op == null || !Regex.IsMatch(op, "^(a|s|m|d|r|p|h|c|sin|cos|tan)$"))
            {
                Console.WriteLine("Error: Unrecognized input.");
            }
            else
            {
                try
                
                {
                    if (Regex.IsMatch(op, "^(r|p)$"))
                    {
                        string? numInput = "";
                        Console.Write("Type a number, and then press Enter: ");
                        numInput = Console.ReadLine();

                        double cleanNum = 0;
                        while (!double.TryParse(numInput, out cleanNum))
                        {
                            Console.Write("This is not valid input. Please enter a numeric value: ");
                            numInput = Console.ReadLine();
                        }

                        result = calculator.DoOperation(cleanNum, op);
                        if (double.IsNaN(result))
                        {
                            Console.WriteLine("This operation will result in a mathematical error.\n");
                        }

                        else
                        {
                            Console.WriteLine("Your result: {0:0.##}\n", result);
                            usageCount++;
                            history.Add(result);
                        }
                    }
                    else if (op == "h")
                    {
                        if (history.Count == 0)
                        {
                            Console.WriteLine("No calculations have been performed yet.\n");
                        }
                        else
                        {
                            Console.WriteLine("Calculation history:");
                            foreach (double value in history)
                            {
                                Console.WriteLine(value);
                            }
                            Console.WriteLine();
                        }
                    }
                    else if (op == "c")
                    {
                        history.Clear();
                        Console.WriteLine("Calculation history cleared.\n");
                    }
                    else if(op == "sin" || op == "cos" || op == "tan")
                    {
                        string? numInput3 = "";
                        Console.Write("Type a number (in degrees), and then press Enter: ");
                        numInput3 = Console.ReadLine();
                        double cleanNum3 = 0;
                        while (!double.TryParse(numInput3, out cleanNum3))
                        {
                            Console.Write("This is not valid input. Please enter a numeric value: ");
                            numInput3 = Console.ReadLine();
                        }
                        result = calculator.DoOperation(cleanNum3, op);
                        if (double.IsNaN(result))
                        {
                            Console.WriteLine("This operation will result in a mathematical error.\n");
                        }
                        else
                        {
                            Console.WriteLine("Your result: {0:0.##}\n", result);
                            usageCount++;
                            history.Add(result);
                        }
                    }
                    else
                    {
                        string? numInput1 = "";
                        string? numInput2 = "";

                        // Ask the user to type the first number.
                        Console.Write("Type a number, and then press Enter: ");
                        numInput1 = Console.ReadLine();

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
                        result = calculator.DoOperation(cleanNum1, cleanNum2, op);
                        if (double.IsNaN(result))
                        {
                            Console.WriteLine("This operation will result in a mathematical error.\n");
                        }

                        else
                        {
                            Console.WriteLine("Your result: {0:0.##}\n", result);
                            usageCount++;
                            history.Add(result);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                }
            }
            Console.WriteLine("------------------------\n");

            // Wait for the user to respond before closing.
            Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
            if (Console.ReadLine() == "n") endApp = true;

            Console.WriteLine("\n"); // Friendly linespacing.
        }
        calculator.Finish();

        return;
    }
}