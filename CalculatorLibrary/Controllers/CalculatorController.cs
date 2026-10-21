using CalculatorLibrary.Models;
using CalculatorLibrary.Services;
using CalculatorLibrary.Views;

namespace CalculatorLibrary.Controllers;

internal class CalculatorController(CalculatorService calculatorService, ExpressionParserService parserService, HistoryService historyService, LogService logService, CalculatorView calculatorView)
{
    private readonly CalculatorService _calculatorService = calculatorService;
    private readonly ExpressionParserService _parserService = parserService;
    private readonly HistoryService _historyService = historyService;
    private readonly LogService _logService = logService;
    private readonly CalculatorView _calculatorView = calculatorView;

    internal void Run()
    {
        string? error = null;
        var angleMode = AngleMode.Degrees;
        var showHelp = false;

        while (true)
        {
            Console.Clear();

            _calculatorView.DisplayHelpHint();
            _calculatorView.DisplayAngleMode(angleMode);
            _calculatorView.DisplayHeader();

            var history = _historyService.Get();
            _calculatorView.DisplayHistory(history);

            _calculatorView.DisplayError(error);

            if (showHelp)
            {
                _calculatorView.DisplayHelp();
                showHelp = false;
            }

            var input = _calculatorView.ReadInput() ?? "";
            string[] command = input.ToLowerInvariant().Split((char[]?)null, StringSplitOptions.RemoveEmptyEntries);

            if (command.Length == 0) continue;

            switch (command[0])
            {
                case "help":
                    showHelp = true;
                    break;

                case "history":
                    error = HandleHistoryCommand(command);
                    break;

                case "angle":
                    error = HandleAngleCommand(command, ref angleMode);
                    break;

                case "exit":
                    return;

                default: // parse expression
                    CalculationLogEntry logEntry;

                    if (!_parserService.TryParse(input, out ParsedExpression? expression, out error))
                    {
                        logEntry = new CalculationLogEntry(DateTime.Now, input, null, null, error);
                        _logService.Log(logEntry);
                        break;
                    }

                    if (!_calculatorService.TryCalculate(expression, angleMode, out double result, out error))
                    {
                        logEntry = new CalculationLogEntry(DateTime.Now, input, expression.NormalizedExpression, null, error);
                        _logService.Log(logEntry);
                        break;
                    }

                    _historyService.Add(result);

                    logEntry = new CalculationLogEntry(DateTime.Now, input, expression.NormalizedExpression, result, null);
                    _logService.Log(logEntry);
                    
                    break;
            }
        }
    }

    private string? HandleHistoryCommand(string[] command)
    {
        if (command.Length == 2 && command[1] == "clear")
        {
            _historyService.Clear();
            return null;
        }

        if (command.Length == 3 && command[1] == "remove")
        {
            if (int.TryParse(command[2], out int index))
            {
                if (_historyService.Remove(index))
                {
                    return null;
                }

                return "Index out of range. No history entry was removed.";
            }

            return "Invalid remove argument. No history entry was removed.";
        }

        return "Invalid history argument. No action was taken.";
    }

    private string? HandleAngleCommand(string[] command, ref AngleMode angleMode)
    {
        if (command.Length == 2 && command[1] == "deg")
        {
            angleMode = AngleMode.Degrees;
            return null;
        }

        if (command.Length == 2 && command[1] == "rad")
        {
            angleMode = AngleMode.Radians;
            return null;
        }

        return "Invalid angle argument. No changes were made.";
    }
}