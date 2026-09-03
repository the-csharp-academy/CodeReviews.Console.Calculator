using CalculatorLibrary.Models;
using Newtonsoft.Json;

namespace CalculatorLibrary;

public class Calculator
{
    public double DoOperation(string op, double num1, double num2 = 0)
    {
        double
            result = double.NaN; // Default value is "not-a-number" if an operation, such as division, could result in an error.

        // Use a switch statement to do the math.
        switch (op)
        {
            case "a":
                result = num1 + num2;
                break;
            case "s":
                result = num1 - num2;
                break;
            case "m":
                result = num1 * num2;
                break;
            case "d":
                if (num2 != 0)
                {
                    result = num1 / num2;
                }
                break;
            case "p":
                result = Math.Pow(num1, num2);
                break;
            case "r":
                result = Math.Sqrt(num1);
                break;
            case "x":
                result = num1 * 10;
                break;
            case "sin":
                double radians = num1 * Math.PI / 180;
                result = Math.Sin(radians);
                break;
            case "cos":
                radians = num1 * Math.PI / 180;
                result = Math.Cos(radians);
                break;
            case "tan":
                radians = num1 * Math.PI / 180;
                result = Math.Tan(radians);
                break;
        }
        
        Calculation calculation = new Calculation();
        calculation.Operand1 = num1;
        calculation.Operand2 = num2;
        calculation.Operation = op;
        calculation.Result = result;
        
        var history = GetHistory();
        history.Operations.Add(calculation);
        string json = JsonConvert.SerializeObject(history);
        File.WriteAllText("calculationlog.json", json);
        
        return result;
    }

    public CalculationHistory GetHistory()
    {
        var historyObject = new CalculationHistory();
        if (File.Exists("calculationlog.json"))
        {
            using StreamReader streamReader = new StreamReader("calculationlog.json");
            var history = streamReader.ReadToEnd();
            historyObject = JsonConvert.DeserializeObject<CalculationHistory>(history);
        }
        return historyObject;
    }
    
    public void ClearHistory()
    {
        File.Delete("calculationlog.json");
    }
}