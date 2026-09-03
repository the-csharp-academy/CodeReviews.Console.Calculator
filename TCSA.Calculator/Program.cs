using CalculatorLibrary;
using System.Text.RegularExpressions;
using CalculatorLibrary.Models;
using Spectre.Console;

namespace CalculatorProgram;

class Program
{
    static void Main(string[] args)
    {
        Calculator calculator = new Calculator();
        Menu(calculator);
    }


    static void Menu(Calculator calculator)
    {
        int calculationCount = 0;
        bool endApp = false;
        while (!endApp)
        {
            AnsiConsole.Clear();

            string menuOp = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[yellow]Choose an option:[/]")
                    .AddChoices(
                        "Calculate",
                        "View history",
                        "Clear history",
                        "Exit"));

            switch (menuOp)
            {
                case "Calculate":
                    AnsiConsole.Clear();

                    bool isCalculated = Calculate(calculator);

                    if (isCalculated) calculationCount++;

                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[grey]Press Enter to return to the menu...[/]");
                    Console.ReadLine();
                    break;

                case "View history":
                    AnsiConsole.Clear();

                    PrintHistory(calculator.GetHistory());

                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[grey]Press Enter to return to the menu...[/]");
                    Console.ReadLine();
                    break;

                case "Clear history":
                    calculator.ClearHistory();

                    AnsiConsole.Clear();
                    AnsiConsole.MarkupLine("[green]History cleared.[/]");

                    AnsiConsole.WriteLine();
                    AnsiConsole.MarkupLine("[grey]Press Enter to return to the menu...[/]");
                    Console.ReadLine();
                    break;

                case "Exit":
                    endApp = true;
                    break;
            }
        }

        AnsiConsole.Clear();
        AnsiConsole.MarkupLine(
            $"[green]Calculator uses this session: {calculationCount}[/]");
    }

    public static bool Calculate(Calculator calculator)
    {
        bool isCalculated = false;
        double result;

        // Ask the user to choose an operator.
        string operation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title("[yellow]Choose an operation:[/]")
                .AddChoices(
                    "Add",
                    "Subtract",
                    "Multiply",
                    "Divide",
                    "Power",
                    "Sqrt",
                    "x10",
                    "Sin",
                    "Cos",
                    "Tan"));

        string op = operation switch
        {
            "Add" => "a",
            "Subtract" => "s",
            "Multiply" => "m",
            "Divide" => "d",
            "Power" => "p",
            "Sqrt" => "r",
            "x10" => "x",
            "Sin" => "sin",
            "Cos" => "cos",
            "Tan" => "tan",
            _ => throw new InvalidOperationException("Unknown operation.")
        };


        // Ask the user to type the first number.
        double cleanNum1 = GetNumberFromUser(calculator, "First number");
        double cleanNum2 = 0;

        if (!Regex.IsMatch(op, "^(r|x|sin|cos|tan)$"))
        {
            // Ask the user to type the second number.
            cleanNum2 = GetNumberFromUser(calculator, "Second number");
        }

        try
        {
            result = calculator.DoOperation(op, cleanNum1, cleanNum2);
            isCalculated = true;
            if (double.IsNaN(result))
            {
                AnsiConsole.MarkupLine("[red]This operation will result in a mathematical error.[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[green]Your result: {result:0.##}[/]");
            }
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"[red]Oh no! An exception occurred trying to do the math.[/]");
            AnsiConsole.MarkupLine($"[grey]Details: {e.Message}[/]");
        }
        
        return isCalculated;
    }

    public static double GetNumberFromUser(Calculator calculator, string input)
    {
        while (true)
        {
            string inputChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title($"[yellow]{input}[/]")
                    .AddChoices(
                        "Enter a new number",
                        "Use result from history"));

            switch (inputChoice)
            {
                case "Enter a new number":
                    return AnsiConsole.Prompt(
                        new TextPrompt<double>("Type a number:"));

                case "Use result from history":
                    var history = calculator.GetHistory();

                    if (history.Operations.Count == 0)
                    {
                        AnsiConsole.MarkupLine("[red]No operations found.[/]");
                        continue;
                    }

                    var historyChoices = new List<string>();

                    foreach (var calculation in history.Operations)
                    {
                        historyChoices.Add(FormatCalculation(calculation));
                    }

                    string selectedCalculation = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[yellow]Choose a calculation:[/]")
                            .AddChoices(historyChoices));

                    int selectedIndex = historyChoices.IndexOf(selectedCalculation);

                    return history.Operations[selectedIndex].Result;
            }
        }
    }

    public static void PrintHistory(CalculationHistory history)
    {
        if (history.Operations.Count == 0)
        {
            AnsiConsole.MarkupLine("[yellow]No operations found.[/]");
            return;
        }

        var table = new Table();

        table.AddColumn("#");
        table.AddColumn("Calculation");

        for (int i = 0; i < history.Operations.Count; i++)
        {
            table.AddRow(
                (i + 1).ToString(),
                FormatCalculation(history.Operations[i]));
        }

        AnsiConsole.Write(table);
    }

    public static string FormatCalculation(Calculation calculation)
    {
        string formattedCalculation;

        switch (calculation.Operation)
        {
            case "a":
                formattedCalculation = ($"{calculation.Operand1} + {calculation.Operand2}  = {calculation.Result}");
                break;
            case "s":
                formattedCalculation = ($"{calculation.Operand1} - {calculation.Operand2}  = {calculation.Result}");
                break;
            case "m":
                formattedCalculation = ($"{calculation.Operand1} * {calculation.Operand2}  = {calculation.Result}");
                break;
            case "d":
                formattedCalculation = ($"{calculation.Operand1} / {calculation.Operand2}  = {calculation.Result}");
                break;
            case "p":
                formattedCalculation = ($"{calculation.Operand1} ^ {calculation.Operand2}  = {calculation.Result}");
                break;
            case "r":
                formattedCalculation = ($"√{calculation.Operand1}  = {calculation.Result}");
                break;
            case "x":
                formattedCalculation = ($"{calculation.Operand1} × 10  = {calculation.Result}");
                break;
            case "sin":
                formattedCalculation = ($"sin({calculation.Operand1})  = {calculation.Result}");
                break;
            case "cos":
                formattedCalculation = ($"cos({calculation.Operand1})  = {calculation.Result}");
                break;
            case "tan":
                formattedCalculation = ($"tan({calculation.Operand1})  = {calculation.Result}");
                break;
            default:
                formattedCalculation = "Invalid option.";
                break;
        }

        return formattedCalculation;
    }
}