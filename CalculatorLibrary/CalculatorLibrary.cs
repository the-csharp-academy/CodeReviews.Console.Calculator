using System.Diagnostics;
using Newtonsoft.Json;

namespace CalculatorLibrary;

public class Calculator
{
    int usage;
    JsonWriter writer;
    readonly List<(string equation, double result)> calculationHistory;
    public Calculator()
    {
        StreamWriter logFile = File.CreateText("calculator.log");
        logFile.AutoFlush = true;
        writer = new JsonTextWriter(logFile)
        {
            Formatting = Formatting.Indented
        };
        writer.WriteStartObject();
        writer.WritePropertyName("Operations");
        writer.WriteStartArray();

        usage = 0;
        calculationHistory = [];
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
                calculationHistory.Add(($"{num1} + {num2}", result));
                break;
            case "s":
                result = num1 - num2;
                writer.WriteValue("Subtract");
                calculationHistory.Add(($"{num1} - {num2}", result));
                break;
            case "m":
                result = num1 * num2;
                writer.WriteValue("Multiply");
                calculationHistory.Add(($"{num1} * {num2}", result));
                break;
            case "d":
                // Ask the user to enter a non-zero divisor.
                if (num2 != 0)
                {
                    result = num1 / num2;
                }
                writer.WriteValue("Divide");
                calculationHistory.Add(($"{num1} / {num2}", result));
                break;
            // Return text for an incorrect option entry.
            default:
                break;
        }
        writer.WritePropertyName("Result");
        writer.WriteValue(result);
        writer.WriteEndObject();

        usage++;

        return result;
    }

    public void ShowUsage()
    {
        Console.WriteLine(string.Format("\n\tCalculator was used {0} times", usage));
    }

    public void ShowHistory()
    {
        for (int i = 0; i < calculationHistory.Count; i++)
        {
            Console.WriteLine($"\t{i + 1}. {calculationHistory[i].equation} = {calculationHistory[i].result}");
        }
        Console.WriteLine();

        Console.Write("Press 'd' to delete history: ");
        switch (Console.ReadLine())
        {
            case "d":
                calculationHistory.Clear();
                break;
        }
    }

    public void Finish()
    {
        writer.WriteEndArray();
        writer.WriteEndObject();
        writer.Close();
    }
}
