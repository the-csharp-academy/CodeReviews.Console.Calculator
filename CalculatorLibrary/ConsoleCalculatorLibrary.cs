using System.Diagnostics;
using Newtonsoft.Json;

namespace CalculatorLibrary;

public class Calculator
{
    private JsonWriter writer;
    private StreamWriter logFile;

    public Calculator()
    {
        logFile = File.CreateText("calculator.log");
        logFile.AutoFlush = true;
        writer = new JsonTextWriter(logFile);
        writer.Formatting = Formatting.Indented;
        
        // Start the root structure
        writer.WriteStartObject();
        writer.WritePropertyName("operations");
        writer.WriteStartArray();
    }
    
    public double DoOperation(double num1, double num2, string op)
    {
        double result = double.NaN; 

        // 1. Start an object block for this specific math operation
        writer.WriteStartObject();
        writer.WritePropertyName("Operand1");
        writer.WriteValue(num1);
        writer.WritePropertyName("Operand2");
        writer.WriteValue(num2);
        writer.WritePropertyName("Operation");
        writer.WriteValue(op);

        switch (op)
        {
            case "a":
                result = num1 + num2;
                Trace.WriteLine(String.Format("{0} + {1} = {2}", num1, num2, result));
                break;
            case "s":
                result = num1 - num2;
                Trace.WriteLine(String.Format("{0} - {1} = {2}", num1, num2, result));
                break;
            case "m":
                result = num1 * num2;
                Trace.WriteLine(String.Format("{0} * {1} = {2}", num1, num2, result));
                break;
            case "d":
                if (num2 != 0)
                {
                    result = num1 / num2;
                    Trace.WriteLine(String.Format("{0} / {1} = {2}", num1, num2, result));
                }
                break;
            default:
                break;
        }
        
        // 2. Write the result and close ONLY this operation's object block
        writer.WritePropertyName("Result");
        writer.WriteValue(result);
        writer.WriteEndObject();
        
        return result;
    }
    
    // 3. Call this method from Program.cs ONLY when the application is closing
    public void Finish()
    {
        writer.WriteEndArray();
        writer.WriteEndObject();
        writer.Close();
        logFile.Close();
    }
}
