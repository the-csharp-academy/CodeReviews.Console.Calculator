using CalculatorLibrary.Helpers;
using CalculatorLibrary.Models;

namespace CalculatorLibrary.Views;

internal class CalculatorView
{
    internal string? ReadInput()
    {
        ConsoleColorHelper.Write("\t> ", ConsoleColor.Cyan);
        return Console.ReadLine();
    }

    internal void DisplayHeader()
    {
        ConsoleColorHelper.WriteLine("========== CALCULATOR ==========", ConsoleColor.DarkYellow);
    }
    
    internal void DisplayHelpHint()
    {
        ConsoleColorHelper.WriteLine("Type 'help' for commands.", ConsoleColor.Yellow);
    }

    internal void DisplayHistory(IReadOnlyList<double> history)
    {
        if (history.Count != 0)
        {
            for (int i = history.Count; i > 0; i--)
            {
                ConsoleColorHelper.WriteLine($"\tr{i}: {history[i - 1]}", ConsoleColor.DarkCyan);
            }
        }
    }

    internal void DisplayHelp()
    {
        DisplayExpressionInfo();
        DisplayHistoryInfo();
        DisplayCommands();
    }

    internal void DisplayError(string? message)
    {
        if (!string.IsNullOrWhiteSpace(message))
        {
            ConsoleColorHelper.WriteLine($"ERROR: {message}", ConsoleColor.Red);
        }
    }

    internal void DisplayAngleMode(AngleMode mode)
    {
        if (mode == AngleMode.Radians)
        {
            ConsoleColorHelper.WriteLine("Angle Mode: RADIANS", ConsoleColor.DarkYellow);
        }
        else if (mode == AngleMode.Degrees)
        {
            ConsoleColorHelper.WriteLine("Angle Mode: DEGREES", ConsoleColor.DarkYellow);
        }
    }

    private void DisplayExpressionInfo()
    {
        ConsoleColorHelper.WriteLine("Expressions:", ConsoleColor.Yellow);
        ConsoleColorHelper.WriteLine("\tBinary expressions consists of a binary operator and two values.", ConsoleColor.Yellow);
        ConsoleColorHelper.WriteLine("\tBinary Operators: '+', '-', '*', '/', '^'", ConsoleColor.Yellow);
        ConsoleColorHelper.WriteLine("\tUnary expressions consists of a unary operator and one value.", ConsoleColor.Yellow);
        ConsoleColorHelper.WriteLine("\tUnary Operators: 'sqrt', 'sin', 'cos', 'tan'", ConsoleColor.Yellow);
    }

    private void DisplayHistoryInfo()
    {
        ConsoleColorHelper.WriteLine("History:", ConsoleColor.Yellow);
        ConsoleColorHelper.WriteLine("History consists of the five latest results.", ConsoleColor.Yellow);
        ConsoleColorHelper.WriteLine("Results can be reused as values in the input by typing 'r1' to 'r5, or simply 'ans' for the latest result.", ConsoleColor.Yellow);
    }

    private void DisplayCommands()
    {
        ConsoleColorHelper.WriteLine("Available Commands:", ConsoleColor.Yellow);
        ConsoleColorHelper.WriteLine("\t'help' displays this list of commands.", ConsoleColor.Yellow);
        ConsoleColorHelper.WriteLine("\t'history clear' clears the history.", ConsoleColor.Yellow);
        ConsoleColorHelper.WriteLine("\t'history remove <number>' removes the corresponding result from history.", ConsoleColor.Yellow);
        ConsoleColorHelper.WriteLine("\t'angle deg' changes trigonometric calculations to degrees.", ConsoleColor.Yellow);
        ConsoleColorHelper.WriteLine("\t'angle rad' changes trigonometric calculations to radians.", ConsoleColor.Yellow);
        ConsoleColorHelper.WriteLine("\t'exit' to exit the program.", ConsoleColor.Yellow);
    }
}
