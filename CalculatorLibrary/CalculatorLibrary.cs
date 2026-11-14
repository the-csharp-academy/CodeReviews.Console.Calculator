using System.Diagnostics;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace CalculatorLibrary;

public class Calculator
{
    int usage;
    JsonWriter writer;
    readonly List<(string equation, double result)> calculationHistory;
    double num1;
    double num2;
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

        num1 = 0;
        num2 = 0;
    }
    // CalculatorLibrary.cs
    public double DoOperation(string op)
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

    public void EnterNumbers()
    {
        // Ask the user to type the first number.
        Console.Write("Type a number, and then press Enter: ");
        string? numInput1 = Console.ReadLine();

        while (!double.TryParse(numInput1, out num1))
        {
            Console.Write("This is not valid input. Please enter a numeric value: ");
            numInput1 = Console.ReadLine();
        }

        // Ask the user to type the second number.
        Console.Write("Type another number, and then press Enter: ");
        string? numInput2 = Console.ReadLine();

        while (!double.TryParse(numInput2, out num2))
        {
            Console.Write("This is not valid input. Please enter a numeric value: ");
            numInput2 = Console.ReadLine();
        }
    }

    public void ChooseNumbersFromHistoryList()
    {
        if (calculationHistory.Count == 0)
        {
            throw new IndexOutOfRangeException("No history yet");
        }
        for (int i = 0; i < calculationHistory.Count; i++)
        {
            Console.WriteLine($"\t{i + 1}. {calculationHistory[i].equation} = {calculationHistory[i].result}");
        }
        Console.WriteLine();

        int indexNum1 = 0;
        int indexNum2 = 0;

        Console.Write("Enter equation number to use result => ");
        string? numInput1 = Console.ReadLine();

        while (!int.TryParse(numInput1, out indexNum1) || indexNum1 > calculationHistory.Count || indexNum1 <= 0)
        {
            Console.Write("This is not valid input. Please enter a numeric value: ");
            numInput1 = Console.ReadLine();
        }

        // Ask the user to type the second number.
        Console.Write("Enter equation number to use result => ");
        string? numInput2 = Console.ReadLine();

        while (!int.TryParse(numInput2, out indexNum2) || indexNum2 > calculationHistory.Count || indexNum2 <= 0)
        {
            Console.Write("This is not valid input. Please enter a numeric value: ");
            numInput2 = Console.ReadLine();
        }

        num1 = calculationHistory[indexNum1 - 1].result;
        num2 = calculationHistory[indexNum2 - 1].result;
    }
    public void Finish()
    {
        writer.WriteEndArray();
        writer.WriteEndObject();
        writer.Close();
    }
}
