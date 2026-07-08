using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace CalculatorLibrary
{
    public class Calculator
    {
        public int timesUsed = 0;

        JsonWriter writer;

        public Calculator()
        {
            StreamWriter logFile = File.CreateText("calculatorlog.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }

        public double DoOperation(double num1, double num2, string op)
        {
            timesUsed++;
            double result = double.NaN;
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");

            switch (op)
            {
                case "a":

                    result = num1 + num2;
                    writer.WriteValue("Add");
                    AddToList(timesUsed, OperationType.Addition, num1, num2, result);
                    break;
                case "s":
                    result = num1 - num2;
                    writer.WriteValue("Subtract");
                    AddToList(timesUsed, OperationType.Subtraction, num1, num2, result);
                    break;
                case "m":
                    result = num1 * num2;
                    writer.WriteValue("Multiply");
                    AddToList(timesUsed, OperationType.Multiplication, num1, num2, result);
                    break;
                case "d":
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    writer.WriteValue("Divide");
                    AddToList(timesUsed, OperationType.Division, num1, num2, result);
                    break;
                case "r":
                    result = Math.Sqrt(num1);
                    writer.WriteValue("Square Root");
                    AddToList(timesUsed, OperationType.Square_root, num1, num2, result);
                    break;
                case "x":
                    result = num1 * 10;
                    writer.WriteValue("10X");
                    AddToList(timesUsed, OperationType.X10, num1, num2, result);
                    break;
                case "p":
                    result = Math.Pow(num1, num2);
                    writer.WriteValue("Taking the power");
                    AddToList(timesUsed, OperationType.Taking_the_power, num1, num2, result);
                    break;
                case "sin":
                    result = Math.Sin(num1 * Math.PI / 180.0);
                    writer.WriteValue("Sine");
                    AddToList(timesUsed, OperationType.Sine, num1, num2, result);
                    break;
                case "cos":
                    result = Math.Cos(num1 * Math.PI / 180.0);
                    writer.WriteValue("Cosine");
                    AddToList(timesUsed, OperationType.Cosine, num1, num2, result);
                    break;
                case "tan":
                    result = Math.Tan(num1 * Math.PI / 180.0);
                    writer.WriteValue("Tangent");
                    AddToList(timesUsed, OperationType.Tangent, num1, num2, result);
                    break;
                case "asin":
                    result = Math.Asin(num1) * 180.0 / Math.PI;
                    writer.WriteValue("Arcsine");
                    AddToList(timesUsed, OperationType.Arcsine, num1, num2, result);
                    break;
                case "acos":
                    result = Math.Acos(num1) * 180.0 / Math.PI;
                    writer.WriteValue("Arccosine");
                    AddToList(timesUsed, OperationType.Arccosine, num1, num2, result);
                    break;
                case "atan":
                    result = Math.Atan(num1) * 180.0 / Math.PI;
                    writer.WriteValue("Arctangent");
                    AddToList(timesUsed, OperationType.Arctangent, num1, num2, result);
                    break;
                case "n":
                    Console.Clear();
                    Console.WriteLine("See you later!");
                    Environment.Exit(0);
                    break;
                default:
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();

            return result;
        }

        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }

        internal static List<Calculation> calculations = new List<Calculation>() { };
        internal static void AddToList(int timesUsed, OperationType operationType, double num1, double num2, double result)
        {
            calculations.Add(new Calculation
            {
                CalculationId = timesUsed,
                Type = operationType,
                Operand1 = num1,
                Operand2 = num2,
                Result = result,
            });
        }

        public void DisplayList()
        {
            PrintList();

            int index1;
            int index2;

            while (true)
            {
                Console.WriteLine(@"
Enter 'd' to delete history
Enter 'r' to reuse a results
Enter 'n' to return to main menu");

                string? select = Console.ReadLine();

                switch (select.Trim().ToLower())
                {
                    case "d":
                        calculations.Clear();
                        Console.Clear();
                        Console.WriteLine("List is cleared. Press Enter to return to calculations.");
                        Console.ReadLine();
                        break;
                    case "r":
                        Console.Clear();
                        PrintList();
                        Console.WriteLine($"Give ID of the first result you want to reuse:");
                        while (!int.TryParse(Console.ReadLine(), out index1) || index1 < 1 || index1 > calculations.Count)
                        {
                            Console.WriteLine($"Invalid input. Please enter a valid ID:");
                        }
                        Console.WriteLine($"Give ID of the second result you want to reuse:");
                        while (!int.TryParse(Console.ReadLine(), out index2) || index2 < 1 || index2 > calculations.Count)
                        {
                            Console.WriteLine($"Invalid input. Please enter a valid ID:");
                        }
                        ReuseResults(calculations[index1 - 1].Result, calculations[index2 - 1].Result);
                        Console.WriteLine("Press Enter to return to calculations.");
                        Console.ReadLine();
                        break;
                    case "n":
                        break;
                    default:
                        Console.WriteLine("Invalid input. Please enter 'd' to delete an operation or 'r' to reuse");
                        Console.ReadLine();
                        break;
                }
                return;
            }
        }

        public void ReuseResults(double operand1, double operand2)
        {
            Console.Clear();
            Console.WriteLine($"Your first number: {operand1}");
            Console.WriteLine($"Your second number: {operand2}");
            Console.WriteLine("Enter operation to carry out...");
            Console.WriteLine($"\ta - {OperationType.Addition}");
            Console.WriteLine($"\ts - {OperationType.Subtraction}");
            Console.WriteLine($"\tm - {OperationType.Multiplication}");
            Console.WriteLine($"\td - {OperationType.Division}");
            Console.WriteLine($"\t--------------------------------------------");
            Console.WriteLine($"\tThese operations only use the first number");
            Console.WriteLine($"\tr - {OperationType.Square_root}");
            Console.WriteLine($"\tx - {OperationType.X10}");
            Console.WriteLine($"\tp - {OperationType.Taking_the_power}");
            Console.WriteLine($"\tsin - {OperationType.Sine}");
            Console.WriteLine($"\tcos - {OperationType.Cosine}");
            Console.WriteLine($"\ttan - {OperationType.Tangent}");
            Console.WriteLine($"\tasin - {OperationType.Arcsine}");
            Console.WriteLine($"\tacos - {OperationType.Arccosine}");
            Console.WriteLine($"\tatan - {OperationType.Arctangent}");
            Console.WriteLine($"\n\tn - exit application\n");
            Console.Write("Your option? ");

            string? op = Console.ReadLine();
            if (op == null || !Regex.IsMatch(op, "[a|s|m|d|r|x|p|sin|cos|tan|asin|acos|atan]"))
            {
                Console.WriteLine("Error: Unrecognized input.");
            }
            else
            {
                try
                {
                    double result = DoOperation(operand1, operand2, op);
                    if (double.IsNaN(result))
                    {
                        Console.WriteLine("This operation will result in a mathematical error.\n");
                    }
                    else Console.WriteLine($"Your result: {result:0.##}\n");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
                }
            }

        }

        public static void PrintList()
        {
            Console.WriteLine("Your session history:");
            Console.WriteLine("------------------------------------------------");
            int i = 1;
            foreach (Calculation calculation in calculations)
            {
                Console.WriteLine($@"
ID: {i} {calculation.Type}
Operands: {calculation.Operand1} & {calculation.Operand2}
Result: {calculation.Result}");
                i++;
            }
            Console.WriteLine("------------------------------------------------");
        }


    }
}