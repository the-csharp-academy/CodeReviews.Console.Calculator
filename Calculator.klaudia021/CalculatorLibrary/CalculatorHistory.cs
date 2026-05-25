using System.Text.RegularExpressions;

namespace CalculatorLibrary;

public class CalculatorHistory
{
    public int Id { get; set; }
    public double Number1 { get; set; }
    public double Number2 { get; set; }
    public string Operation { get; set; }
    public double Result { get; set; }
    private string _operationLetter;

    public CalculatorHistory(int id, double num1, double num2, string operation, double result)
    {
        Id = id;
        Number1 = num1;
        Number2 = num2;
        _operationLetter = operation;
        Operation = AddSymbol(operation);
        Result = result;
    }

    private string AddSymbol(string op)
    {
        Dictionary<string, string> operationSigns = new Dictionary<string, string>()
        {
            {"a", "+"},
            {"s", "-"},
            {"m", "*"},
            {"d", "/"},
            {"q", "sqrt"},
            {"e", "^"},
            {"i", "sin"},
            {"o", "cos"},
            {"t", "tan"},
            {"c", "cot"},
        };

        string? symbol;
        operationSigns.TryGetValue(op, out symbol);

        return symbol?? " ";
       
    }

    public override string ToString()
    {
        if (Regex.IsMatch(_operationLetter, "[a|s|m|d|e]"))
            return $"{Id}.: {Number1} {Operation} {Number2} = {Result}";
        
        return $"{Id}.: {Operation}({Number1}) = {Result}"; 
    }
}