namespace CalculatorLibrary
{
    // CalculatorLibrary.cs
    using System.Diagnostics;
    // Program.cs
    using Newtonsoft.Json;
    public class Calculator
    {
        // CalculatorLibrary.cs
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

        // CalculatorLibrary.cs
        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");
            
            // Use a switch statement to do the math.
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
                    }
                    writer.WriteValue("Divide");
                    break;
                case "sq":
                    if(num1 >= 0)
                    {
                        result = Math.Sqrt(num1);
                    }
                    writer.WriteValue("Square Root");
                    break;
                case "sq2":
                    if(num1 >= 0 && num2 != 0)
                    {
                        result = Math.Pow(num1, 1/num2);
                    }
                    writer.WriteValue("Nth Root");
                    break;
                case "10":
                    result = Math.Pow(10, num1);
                    writer.WriteValue("10^x");
                    break;
                case "cos":
                    result = Math.Cos(num1);
                    writer.WriteValue("Cosine");
                    break;
                case "sin":
                    result = Math.Sin(num1);
                    writer.WriteValue("Sine");
                    break;
                case "tan":
                    result = Math.Tan(num1);
                    writer.WriteValue("Tangent");
                    break;
                case "log":
                    if (num1 > 0)
                    {
                        result = Math.Log10(num1);
                    }
                    writer.WriteValue("Logarithm base 10");
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
        // CalculatorLibrary.cs
        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }
    
}
