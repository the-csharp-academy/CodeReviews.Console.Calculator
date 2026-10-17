using System.Text.RegularExpressions;
using CalculatorLibrary;


class Program
{
    static async Task Main(string[] args)
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

            Console.Write("Do you want to use speech recognition to input numbers? (y/n): ");
            string? useSpeech = Console.ReadLine();

            if (useSpeech != null && useSpeech.ToLower() == "y")
            {
                // SPEECH RECOGNITION MODE
                double number1 = calculator.GetNumber().Result;
                double number2 = calculator.GetNumber().Result;

                string operation = calculator.GetOperationMethod().Result;

                    if (string.IsNullOrEmpty(operation))
                {
                    Console.WriteLine("Error: No operation was heard. Try again.");
                    Console.WriteLine("------------------------\n");
                    continue; // Restarts the loop seamlessly
                }

                string cleanedInput = operation.Trim().ToLower();

                if (!Regex.IsMatch(cleanedInput, "^(add|sub|mul|div|a|s|m|d)"))
                {
                    Console.WriteLine("Error: Unrecognized operation.");
                    Console.WriteLine("Try again.");
                    
                }
                else
                {
                    // 3. Extract the very first letter ('a', 's', 'm', or 'd') 
                    // so your calculator knows exactly what operation to run.
                    operation = cleanedInput[..1];

                    try
                    {
                        result = calculator.DoOperation(number1, number2, operation);
                        if (double.IsNaN(result))
                        {
                            Console.WriteLine("This operation will result in a mathematical error.\n");
                        }
                        else 
                        {
                            Console.WriteLine("Your result: {0:0.##}\n", result);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                    }
                }

                Console.WriteLine("------------------------\n");
                Console.WriteLine("Do you want to continue? (yes/no): ");

                string? continueInput = await calculator.ContinueOrNot();
                if (continueInput == "no" || continueInput.StartsWith("n"))
                {
                    endApp = true;
                    Console.WriteLine("Goodbye!");
                }
                else if (continueInput == "yes" || continueInput.StartsWith("y"))
                {
                    Console.WriteLine("\n--- Starting Next Calculation ---");
                    // We don't set endApp to true, so the loop naturally cycles back up to the top!
                }
                else
                {
                    // Safety net: If speech timed out or was unreadable, let's ask manually or default to loop
                    Console.WriteLine("Unrecognized input or timeout. Continuing calculation loop by default...\n");
                }
                //END SPEECH RECOGNITION MODE
            }
            else
            {
                // MANUAL INPUT MODE
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

                // Ask the user to choose an operator.
                Console.WriteLine("Choose an operator from the following list:");
                Console.WriteLine("\ta - Add");
                Console.WriteLine("\ts - Subtract");
                Console.WriteLine("\tm - Multiply");
                Console.WriteLine("\td - Divide");
                Console.Write("Your option? ");

                string? op = Console.ReadLine();

                // Validate input is not null, and matches the pattern
                if (op == null || ! Regex.IsMatch(op, "^(a|s|m|d)$"))
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
                        else Console.WriteLine("Your result: {0:0.##}\n", result);
                        }
                        catch (Exception e)
                        {
                        Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                        }
                }
                Console.WriteLine("------------------------\n");

                Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
                if (Console.ReadLine() == "n") endApp = true;

                Console.WriteLine("\n");
            //END MANUAL INPUT MODE
            }
        }
        calculator.Finish();
    }
}