using CalculatorLibrary;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        bool endApp = false;
        bool calculFromList = false;
        double currentChoiceFromPlayerResult = 0;
        // Display title as the C# console calculator app.
        Console.WriteLine("Console Calculator in C#\r");
        Console.WriteLine("------------------------\n");

        Calculator calculator = new Calculator();

        List<double> latestCalculationResults = new List<double>();

        while (!endApp)
        {
            // Declare variables and set to empty.
            // Use Nullable types (with ?) to match type of System.Console.ReadLine
            string? numInput1 = "";
            string? numInput2 = "";
            double result = 0;

            // Ask the user to type the first number.

            double cleanNum1 = 0;
            double cleanNum2 = 0;

            if (calculFromList)
            {
                cleanNum1 = currentChoiceFromPlayerResult;
            }
            else
            {
                Console.Write("Type a number, and then press Enter: ");
                numInput1 = Console.ReadLine();

                
                while (!double.TryParse(numInput1, out cleanNum1))
                {
                    Console.Write("This is not valid input. Please enter a numeric value: ");
                    numInput1 = Console.ReadLine();
                }
            }

            calculFromList = false;





            // Ask the user to choose an operator.
            Console.WriteLine("Choose an operator from the following list:");
            Console.WriteLine("\ta - Add");
            Console.WriteLine("\ts - Subtract");
            Console.WriteLine("\tm - Multiply");
            Console.WriteLine("\td - Divide");
            Console.WriteLine("\tpow - Raising to a power");
            Console.WriteLine("\tsqrt - Square root");
            Console.WriteLine("\t10x - TenToPower");
            Console.WriteLine("\tsin - Sin");
            Console.WriteLine("\tcos - Cos");
            Console.WriteLine("\ttan - Tan");



            Console.Write("Your option? ");

            string? op = Console.ReadLine();

            // Validate input is not null, and matches the pattern
            if (op == null || !Regex.IsMatch(op, "^(a|s|m|d|pow|sqrt|10x|sin|cos|tan)$"))
            {
                Console.WriteLine("Error: Unrecognized input.");
            }
            else
            {
                try
                {
                    if (Regex.IsMatch(op, "^(10x|sqrt|sin|cos|tan)"))
                    {
                        Console.WriteLine("The expression you selected is a unary operation and works only with a single number. The first digit will be selected for the calculation.\n");
                    }
                    else
                    {
                        // Ask the user to type the second number.
                        Console.Write("Type another number, and then press Enter: ");
                        numInput2 = Console.ReadLine();

                        while (!double.TryParse(numInput2, out cleanNum2))
                        {
                            Console.Write("This is not valid input. Please enter a numeric value: ");
                            numInput2 = Console.ReadLine();
                        }
                    }



                    result = calculator.DoOperation(cleanNum1, cleanNum2, op);
                    if (double.IsNaN(result))
                    {
                        Console.WriteLine("This operation will result in a mathematical error.\n");
                    }
                    else
                    {
                        Console.WriteLine("Your result: {0:0.##}\n", result);
                        latestCalculationResults.Add(result);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                }
            }
            Console.WriteLine("------------------------\n");

            // Wait for the user to respond before closing.
            Console.Write("Press 'n' and Enter to close the app, or enter \"burmalda\" to see the list of last results or press any other key and Enter to continue: ");
            string endChoice = Console.ReadLine();
            if (endChoice == "n") endApp = true;
            else if (endChoice.ToLower() == "burmalda")
            {
                Console.WriteLine();
                Console.WriteLine("Last calculation results: ");
                for (int i = 0; i < latestCalculationResults.Count; i++)
                {
                    Console.WriteLine($"{i + 1} - {latestCalculationResults[i]}");
                }
                Console.WriteLine("If you want to delete the list of recent results, write \"del\", or if you want to use last results, write the position of number from the list. ");
                string? userChoice = Console.ReadLine();

                if (int.TryParse(userChoice, out int userIntChoice))
                {
                    if(userIntChoice > 0 && userIntChoice<= latestCalculationResults.Count())
                    {
                        int targetIndex = userIntChoice - 1;
                        currentChoiceFromPlayerResult = latestCalculationResults[targetIndex];
                        Console.WriteLine("The first num was filled");
                        calculFromList = true;
                    }
                    else
                    {
                        Console.WriteLine("Index error. SKIP");
                    }
                }
                else if (userChoice == "del")
                {
                    latestCalculationResults.Clear();
                    Console.WriteLine("The list was cleared!");
                }
                
                
                //if(userChoice == "del")
                //{
                //    latestCalculationResults.Clear();
                //    Console.WriteLine();
                //    Console.WriteLine("All elements be deleted. ");
                //    Console.ReadKey();
                //}
                //else if (choice >= 0 && )
            }

            Console.WriteLine("\n"); // Friendly linespacing.
        }

        calculator.Finish();
        return;
    }
}