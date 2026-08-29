using System.Diagnostics;
using Newtonsoft.Json;
namespace CalculatorLibrary
{
    public class Calculator
    {
        JsonWriter _writer;

        public Calculator()
        {
            StreamWriter logFile = File.CreateText("calcuator.json");
            logFile.AutoFlush = true;

            _writer = new JsonTextWriter(logFile);
            _writer.Formatting = Formatting.Indented;
            _writer.WriteStartObject();
            _writer.WritePropertyName("Operations");
            _writer.WriteStartArray();
        }
        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            
            _writer.WriteStartObject();

            _writer.WritePropertyName("Operand1");
            _writer.WriteValue(num1);
            
            _writer.WritePropertyName("Operand2");
            _writer.WriteValue(num2);

            double radians = num1 * (Math.PI / 180.00);


            char symbol = ' ';
            // Use a switch statement to do the math.
            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    symbol = '+';
                    break;
                case "s":
                    result = num1 - num2;
                    symbol = '-';
                    break;
                case "m":
                    result = num1 * num2;
                    symbol  = '*';
                    break;
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                        symbol = '/';
                    }
                    break;
                case "sqrt":
                    result = Math.Sqrt(num1);
                    symbol = '√';
                    break;
                case "pow":
                    result = Math.Pow(num1, num2);
                    symbol = '^';
                    break;
                case "10x":
                    result = Math.Pow(10, num1);
                    symbol = 'E';
                    break;
                case "sin":
                    result = Math.Sin(radians);
                    symbol = 'S';
                    break;
                case "cos":
                    result = Math.Cos(radians);
                    symbol = 'C';
                    break;
                case "tg":
                    if (num1 == 90)
                    {
                        Console.WriteLine("Undefined");
                        result = 0;
                    }
                    else
                    {
                        result = Math.Tan(radians);
                        symbol = 'T';
                    }
                    break;
                // Return text for an incorrect option entry.
                default:
                    break;
            }
            _writer.WritePropertyName("Operation");
            _writer.WriteValue(symbol);
            _writer.WritePropertyName("Result");
            _writer.WriteValue(result);
            _writer.WriteEndObject();

            Trace.WriteLine($"{num1} {symbol} {(double.IsNaN(num2) ? "" : num2)} = {result}");

            return result;
        }

        public void Finish()
        {
            _writer.WriteEndArray();
            _writer.WriteEndObject();
            _writer.Close();
        }
    }
}
