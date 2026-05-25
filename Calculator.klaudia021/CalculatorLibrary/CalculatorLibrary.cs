using Newtonsoft.Json;

namespace CalculatorLibrary;

public class Calculator
{
    private JsonWriter writer;

    private static int TimesUsed = 0;
    public List<CalculatorHistory> History { get; } = new List<CalculatorHistory>();

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
        double radians = 0.0d;
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
            case "q":
                result = Math.Sqrt(num1);
                writer.WriteValue("Square root");
                break;
            case "e":
                result = Math.Pow(num1, num2);
                writer.WriteValue("Exponentiation");
                break;
            case "i":
                radians = num1 * (Math.PI / 180.0);
                result = Math.Sin(radians);
                result = Math.Round(result, 4);

                writer.WriteValue("Sine");
                break;
            case "o":
                radians = num1 * (Math.PI / 180.0);
                result = Math.Cos(radians);
                result = Math.Round(result, 4);

                writer.WriteValue("Cosine");
                break;
            case "t":
                writer.WriteValue("Tangent");

                if (num1 % 90 == 0)
                {
                   result = Double.NaN;
                   break;
                }

                radians = num1 * (Math.PI / 180.0);
                result = Math.Tan(radians);
                result = Math.Round(result, 4);

                break;
            case "c":
                writer.WriteValue("Cotangent");

                if (num1 == 0)
                {
                   result = Double.NaN;
                   break;
                }

                radians = num1 * (Math.PI / 180.0);
                result = 1 / Math.Tan(radians);
                result = Math.Round(result, 4);
                break;
            default:
                break;
        }

        TimesUsed++;
        History.Add(new CalculatorHistory(TimesUsed, num1, num2, op, result));

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

        Console.WriteLine($"The calculator was used {TimesUsed} time(s).");
    }

    public void ListHistory()
    {
        Console.WriteLine("\nCalculator history:");
        Console.WriteLine("---------------------");

        if (!HistoryHasEnoughResults(1))
        {
            Console.WriteLine("History is empty!");
        }
        else
        {
            foreach (var listItem in History)
                Console.WriteLine(listItem.ToString());
        }

        Console.WriteLine("---------------------\n");
    }

    public void ClearList()
    {
        History.Clear();
        Console.WriteLine("History is cleared!");
    }

    public CalculatorHistory? GetHistoryByIndex(int index)
    {
        if (index < 0 || History.Count - 1 < index)
            return null;
        
        return History[index];
    }

    public bool HistoryHasEnoughResults(int count)
    {
        if (History.Count < count) 
            return false;

        return true;
    }
}