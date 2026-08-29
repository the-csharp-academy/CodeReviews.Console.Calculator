using System.ComponentModel;
using System.Text.RegularExpressions;
using CalculatorLibrary;
namespace SimpleCalculator
{
    class Program
    {
        static void Main(string[] args)
        {
            var calculations = new List<Calculation>();
            var calculator = new Calculator();
            CalculatorInterface(calculations, calculator);
            calculator.Finish();
            return;
        }

        static void CalculatorInterface(List<Calculation> calculations, Calculator calculator)
        {
            while (true)
            {
                // Display title as the C# console calculator app.
                Console.WriteLine("Console Calculator in C#\r");
                Console.WriteLine("------------------------\n");
                Console.WriteLine($"Total Calculations Performed {calculations.Count}");

                Console.WriteLine("What do you want to do?");
                Console.WriteLine("\tc - Calculator");
                Console.WriteLine("\ts - See previous results");
                Console.WriteLine("\tdel - Delete all previous results");

                string? userChoice = Console.ReadLine();
                if (userChoice is null)
                {
                    Console.WriteLine("Error: Option is null");
                    continue;
                }
                
                switch (userChoice)
                {
                    case "c":
                        CalculatorLoop(calculator, calculations);
                        break;
                    case "s":
                        if (calculations.Count == 0) Console.WriteLine("No calculations available");
                        else
                        {
                            for (int i = 0; i < calculations.Count; i++)
                            {
                                Console.WriteLine($"[{i}] -> Type {calculations[i].Operation}: {calculations[i].Operand1} and {calculations[i].Operand2} = {calculations[i].Result}");
                            }

                            Console.WriteLine("Select a result from the previous list: ");
                            string? chosenResultIndex = Console.ReadLine();
                            int chosenResultIndexClean; 
                            if (chosenResultIndex is null || !int.TryParse(chosenResultIndex, out chosenResultIndexClean) || chosenResultIndexClean > calculations.Count - 1)
                            {
                                Console.WriteLine("Invalid index");
                                continue;
                            }

                            CalculatorLoop(calculator, calculations, calculations[chosenResultIndexClean].Result);
                        }
                            break;
                    case "del":
                        Console.WriteLine("Deleting All History of Calculations");
                        calculations.Clear();
                        break;
                    default:
                        Console.WriteLine("Invalid Option");
                        break;
                }

            }

            
        }



        private static void CalculatorLoop(Calculator calculator, List<Calculation> calculations, double previousResult = double.NaN)
        {
            bool endApp = false;
            int calculatorUses = calculations.Count();


            while (!endApp)
            {
                // Declare variables and set to empty.
                // Use Nullable types (with ?) to match type of System.Console.ReadLine
                Console.Clear();
                string? numInput1 = "";
                string? numInput2 = "";
                double result = 0;
                // Ask the user to type the first number.
                double cleanNum1;
                if (double.IsNaN(previousResult))
                {
                    Console.Write("Type a number, and then press Enter: ");
                    Console.WriteLine("For trig functions, input the angle in degrees");

                    numInput1 = Console.ReadLine();

                    cleanNum1 = 0;
                    while (!double.TryParse(numInput1, out cleanNum1))
                    {
                        Console.Write("This is not valid input. Please enter a numeric value: ");
                        numInput1 = Console.ReadLine();
                    }

                }
                else
                {
                    endApp = true;
                    Console.WriteLine($"Using {previousResult} as Operand 1");
                    cleanNum1 = previousResult;
                }


                // Ask the user to choose an operator.
                Console.WriteLine("Choose an operator from the following list:");
                Console.WriteLine("\ta - Add");
                Console.WriteLine("\ts - Subtract");
                Console.WriteLine("\tm - Multiply");
                Console.WriteLine("\td - Divide");
                Console.WriteLine("\tsqrt - Square Root");
                Console.WriteLine("\tpow - Exopnent");
                Console.WriteLine("\t10x - Power of 10");
                Console.WriteLine("\tsin - Sine");
                Console.WriteLine("\tcos - Cosine");
                Console.WriteLine("\ttg - Tangent");
                Console.Write("Your option? ");

                string? op = Console.ReadLine();

                // Ask the user to type the second number.
                double cleanNum2 = double.NaN;
                if (op is null || !Regex.IsMatch(op, "^(a|s|m|d|sqrt|pow|10x|sin|cos|tg)$"))
                {
                    Console.WriteLine("Error Invalid Input");
                    continue;
                }
                else if (!Regex.IsMatch(op, "^(sqrt|10x|sin|cos|tg)$"))
                {
                    Console.Write("Type another number, and then press Enter: ");
                    numInput2 = Console.ReadLine();

                    cleanNum2 = 0;
                    while (!double.TryParse(numInput2, out cleanNum2))
                    {
                        Console.Write("This is not valid input. Please enter a numeric value: ");
                        numInput2 = Console.ReadLine();
                    }
                }
                
                // Validate input is not null, and matches the pattern
                    try
                    {
                        result = calculator.DoOperation(cleanNum1, cleanNum2, op);
                        if (double.IsNaN(result))
                        {
                            Console.WriteLine("This operation will result in a mathematical error.\n");
                        }
                        else
                        {
                            Console.WriteLine("Your result: {0:0.##}\n", result);
                            string operation = op switch
                            {
                                "a" => "Add",
                                "s" => "Subtract",
                                "m" => "Multiply",
                                "d" => "Divide",
                                "sqrt" => "Square Root",
                                "pow" => "Exponentiation",
                                "10x" => "Power of 10",
                                "sin" => "Sine",
                                "cos" => "Cosine",
                                "tg" => "Tangent",
                                _ => throw new ArgumentException()
                            };

                            calculations.Add(
                                new Calculation()
                                {
                                    Operand1 = cleanNum1,
                                    Operand2 = cleanNum2,
                                    Result = result,
                                    Operation = operation
                                }
                                );
                            calculatorUses = calculations.Count();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                    }
                
                Console.WriteLine("------------------------\n");

                // Wait for the user to respond before closing.

                string finalMessage = endApp ? "Press Any Key To Continue" : "Press 'n' and Enter to close the app, or press any other key and Enter to continue: ";
                Console.Write(finalMessage);
                if (Console.ReadLine() == "n") endApp = true;

                Console.WriteLine("\n"); // Friendly linespacing.
            }

            return;
        }
    }

}
