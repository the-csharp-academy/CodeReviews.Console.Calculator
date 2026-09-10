using Newtonsoft.Json;
namespace CalculatorLibrary
{
    public class Calculator
    {
        public int calculations = 0;
        public List<string> calculationsList = new();
        readonly JsonWriter writer;

        public Calculator()
        {
            StreamWriter logFile = File.CreateText("calculator.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();

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
                    if (num2 != 0)
                    {
                        result = num1 / num2;
                    }
                    writer.WriteValue("Divide");
                    break;
                case "p":
                    result = Math.Pow(num1, num2);
                    writer.WriteValue("Power");
                    break;
                case "x10":
                    result = num1 * 10;
                    writer.WriteValue("Times Ten");
                    break;
                default:
                    break;
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);

            calculations++;
            writer.WriteEndObject();

            return result;
            
        }

        public void storeCalculations(string num1, string num2, string op, double result)
        {
            string calculationToString = "";
            switch (op)
            {
                case "a":
                    op = "+";
                    break;
                case "s":
                    op = "-";
                    break;
                case "m":
                    op = "*";
                    break;
                case "d":
                    op = "/";
                    break;
                case "sr":
                    op = "√";
                    break;
                case "p":
                    op = "^";
                    break;
                case "x10":
                    op = "x10";
                    break;
            }

            if (op == "√")
            {
                calculationToString = $"{op}{num1} = {result}";

                writer.WriteStartObject();
                writer.WritePropertyName("Operand1");
                if (double.TryParse(num1, out double cleanNum)) writer.WriteValue(cleanNum);
                else writer.WriteValue(num1);

                writer.WritePropertyName("Operand2");
                writer.WriteValue(0);

                writer.WritePropertyName("Operation");
                writer.WriteValue("SquareRoot");

                writer.WritePropertyName("Result");
                writer.WriteValue(result);
                writer.WriteEndObject();

                calculations++;
            }
            
            else calculationToString = $"{num1} {op} {num2} = {result}";

            calculationsList.Add(calculationToString);
        }

        public void DeleteCalculations()
        {
                calculationsList.Clear();
        }

        public void Finish()
        {
            writer.WriteEndArray();
            writer.WritePropertyName("Total Calculations");
            writer.WriteValue(calculations);
            writer.WriteEndObject();
            writer.Close();
        }
    }
}
