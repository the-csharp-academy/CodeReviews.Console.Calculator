namespace CalculatorLibrary.Helpers;

internal class ConsoleColorHelper
{
    internal static void WriteLine(string text, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        Console.WriteLine(text);

        Console.ForegroundColor = originalColor;
    }

    internal static void Write(string text, ConsoleColor color)
    {
        var originalColor = Console.ForegroundColor;

        Console.ForegroundColor = color;
        Console.Write(text);

        Console.ForegroundColor = originalColor;
    }
}