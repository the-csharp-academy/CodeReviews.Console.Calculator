// CalculatorLibrary.cs
using System.Diagnostics;
using Newtonsoft.Json;

namespace CalculatorLibrary
{
    public class Calculator
    {

        public static int DoOperationCallCount = 0;
        JsonWriter writer;
        public Calculator()
        {
            StreamWriter logfile = File.CreateText("calculatorlog.json");
            logfile.AutoFlush = true;
            writer = new JsonTextWriter(logfile);
            writer.Formatting = Formatting.Indented;

            writer.WriteStartObject();

            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }

        public double DoOperation(double num1, double num2, string op)
        {
            DoOperationCallCount++;

            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");
            // Use a switch statement to do the math.

            // for unar op
            double radians;

            switch (op)
            {
                case "a":
                    result = num1 + num2;
                    writer.WriteValue("Add");
                    break;
                case "s":
                    result = num1 - num2;
                    writer.WriteValue("Subtract");
                    break;
                case "m":
                    result = num1 * num2;
                    writer.WriteValue("Multiply");
                    break;
                case "d":
                    // Ask the user to enter a non-zero divisor.
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                        writer.WriteValue("Divide");
                    }
                    break;
                case "pow":
                    if (num2 == 0)
                    {
                        result = 1;
                        writer.WriteValue("Pow");
                    }
                    else
                    {
                        result = Math.Pow(num1, num2);
                        writer.WriteValue("Pow");
                    }
                    break;
                case "sqrt":
                    result = Math.Sqrt(num1);
                    writer.WriteValue("Sqrt");
                    break;
                case "10x":
                    result = Math.Pow(10, num1);
                    writer.WriteValue("10x");
                    break;
                case "sin":
                    radians = num1 * (Math.PI / 180.0);
                    result = Math.Sin(radians);
                    writer.WriteValue("Sin");
                    break;
                case "cos":
                    radians = num1 * (Math.PI / 180.0);
                    result = Math.Cos(radians);
                    writer.WriteValue("Cos");
                    break;
                case "tan":
                    radians = num1 * (Math.PI / 180.0);
                    result = Math.Tan(radians);
                    writer.WriteValue("Tan");
                    break;
                // Return text for an incorrect option entry.
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

            writer.WritePropertyName("TotalCallCount");
            writer.WriteValue(DoOperationCallCount);

            writer.WriteEndObject();
            writer.Close();
        }
    }

}
