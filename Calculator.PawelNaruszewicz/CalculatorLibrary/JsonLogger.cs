using CalculatorLibrary;
using Newtonsoft.Json;

namespace Calculator.PawelNaruszewicz
{
    public class JsonLogger
    {
        JsonWriter writer;
        public JsonLogger()
        {
            StreamWriter logFile = File.CreateText("calculatorlog.json");
            logFile.AutoFlush = true;
            writer = new JsonTextWriter(logFile);
            writer.Formatting = Formatting.Indented;
            writer.WriteStartObject();
            writer.WritePropertyName("Operations");
            writer.WriteStartArray();
        }
        public void LogOperation(double result, IOperation operation, params double[] numbers)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Operand1");
            writer.WriteValue(numbers[0]);
            if(operation.RequiredArguments >1)
            {
                writer.WritePropertyName("Operand2");
                writer.WriteValue(numbers[1]);
            }
            writer.WritePropertyName("Result");
            writer.WriteValue(result);
            writer.WritePropertyName("Operation");
            writer.WriteValue(operation.Name);
            writer.WriteEndObject();
        }
        public void Finish()
        {
            writer.WriteEndArray();
            writer.WriteEndObject();
            writer.Close();
        }
    }
}
