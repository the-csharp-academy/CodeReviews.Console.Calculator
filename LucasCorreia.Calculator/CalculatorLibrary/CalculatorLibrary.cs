using Newtonsoft.Json;

namespace CalculatorLibrary
{
    public class Calculator : IDisposable
    {

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

        public void Dispose()
        {
            Finish();
        }

        public double DoOperation(double num1, double num2, string op)
        {
            double result = double.NaN; 
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operand2");
            writer.WriteValue(num2);
            writer.WritePropertyName("Operation");
            switch (op)
            {
                case "1":
                    result = num1 + num2;
                    writer.WriteValue("Add");
                    break;
                case "2":
                    result = num1 - num2;
                    writer.WriteValue("Subtract");
                    break;
                case "3":
                    result = num1 * num2;
                    writer.WriteValue("Multiply");
                    break;
                case "6":
                    result = Math.Pow(num1,num2);
                    writer.WriteValue("Pow");
                    break;
                case "4":
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    writer.WriteValue("Divide");
                    break;
                default:
                    writer.WriteValue("Unknown");
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();

            return result;
        }

        public double DoOtherOperation(double num1, string op)
        {
            double result = double.NaN;
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(num1);
            writer.WritePropertyName("Operation");
            switch (op)
            {
                case "5":
                    result = Math.Sqrt(num1);
                    writer.WriteValue("Square Root");
                    break;
                case "7":
                    result = Math.Pow(10,num1);
                    writer.WriteValue("Pow 10");
                    break;
                case "8":
                    result = Math.Sin(num1 * Math.PI / 180);
                    writer.WriteValue("Seno");
                    break;
                case "9":
                    result = Math.Cos(num1 * Math.PI / 180);
                    writer.WriteValue("Cosseno");
                    break;
                case "10":
                    result = Math.Tan(num1 * Math.PI / 180);
                    writer.WriteValue("Tangente");
                    break;
                default:
                    writer.WriteValue("Unknown");
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WriteEndObject();

            return result;
        }

        public void ListNumbers(List<double> numbers)
        {
            int index = 1;
            foreach (double results in numbers)
            {
                Console.WriteLine($"{index} - {results}");
                index++;
            }
        }


        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }
}