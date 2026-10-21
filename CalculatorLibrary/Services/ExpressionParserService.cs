using CalculatorLibrary.Models;

namespace CalculatorLibrary.Services;

internal class ExpressionParserService(HistoryService historyService)
{
    private readonly HistoryService _historyService = historyService;

    internal bool TryParse(string input, out ParsedExpression? expression, out string? error)
    {
        if (!TryNormalizeInput(input, out string normalizedInput))
        {
            expression = null;
            error = "Unbalanced parantheses usage.";
            return false;
        }

        if (TryParseOperand(normalizedInput, out double firstOperand))
        {
            expression = new(null, firstOperand, null, $"{firstOperand}");
            error = null;
            return true;
        }

        if (TryParseUnaryFunction(normalizedInput, out OperationDefinition operation, out firstOperand))
        {
            expression = new(operation.Operation, firstOperand, null, $"{operation.Symbol}({firstOperand})");
            error = null;
            return true;
        }

        if (TryParseBinaryExpression(normalizedInput, out operation, out firstOperand, out double secondOperand))
        {
            expression = new(operation.Operation, firstOperand, secondOperand, $"{firstOperand} {operation.Symbol} {secondOperand}");
            error = null;
            return true;
        }

        expression = null;
        error = "Invalid expression.";
        return false;
    }

    private bool TryNormalizeInput(string input, out string normalizedInput)
    {
        input = input.ToLowerInvariant();

        var trimmedInput = RemoveWhiteSpace(input);

        return TryRemoveParantheses(trimmedInput, out normalizedInput);
    }

    private string RemoveWhiteSpace(string input)
    {
        return new string([.. input.Where(c => !Char.IsWhiteSpace(c))]);
    }

    private bool TryRemoveParantheses(string input, out string result)
    {
        result = default;

        int balanceCounter = 0;

        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '(')
            {
                balanceCounter++;
            }

            if (input[i] == ')')
            {
                balanceCounter--;
            }

            if (balanceCounter < 0)
            {
                return false;
            }
        }

        if (balanceCounter != 0) return false;

        result = input.Replace("(", "").Replace(")", "");
        return true;
    }

    private bool TryParseUnaryFunction(string input, out OperationDefinition operation, out double firstOperand)
    {
        firstOperand = default;

        operation = SupportedOperations.All
                .FirstOrDefault(definition => definition.Symbol.Length > 1 &&
                input.StartsWith(definition.Symbol, StringComparison.OrdinalIgnoreCase));

        if (operation is null) return false;

        if (!TryParseOperand(input[operation.Symbol.Length..], out firstOperand)) return false;

        return true;
    }

    private bool TryParseBinaryExpression(string input, out OperationDefinition operation, out double firstOperand, out double secondOperand)
    {
        operation = default;
        firstOperand = default;
        secondOperand = default;

        var operatorIndex = FindBinaryOperatorIndex(input);

        if (operatorIndex == -1) return false;

        operation = SupportedOperations.All
                .FirstOrDefault(definition => definition.Symbol == input[operatorIndex].ToString());

        (string firstOperand, string secondOperand) operands = (input[..operatorIndex], input[(operatorIndex + 1)..]);

        if (!TryParseOperand(operands.firstOperand, out firstOperand)) return false;
        if (!TryParseOperand(operands.secondOperand, out secondOperand)) return false;

        return true;
    }

    private int FindBinaryOperatorIndex(string input)
    {
        for (int i = 0; i < input.Length; i++)
        {
            if (input[i] == '-' && (i == 0)) continue;

            if (SupportedOperations.All.Any(definition => definition.Symbol == input[i].ToString()))
            {
                return i;
            }
        }

        return -1;
    }

    private bool TryParseOperand(string token, out double value)
    {
        if (double.TryParse(token, out value))
        {
            return true;
        }

        return token switch
        {
            "ans" or "r1" => _historyService.TryGetEntry(1, out value),
            "r2" => _historyService.TryGetEntry(2, out value),
            "r3" => _historyService.TryGetEntry(3, out value),
            "r4" => _historyService.TryGetEntry(4, out value),
            "r5" => _historyService.TryGetEntry(5, out value),
            _ => false
        };
    }
}