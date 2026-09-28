using Calculator.Library;

namespace Calculator.Console;
using Console = System.Console;

public static class UserInterface
{
    private static double _num1;
    private static double _num2;
    
    // Could add timestamps in the name for persistent logs
    // Here we would rather override the file each time 
    private const string LogName = "calculator.log";
    private static readonly Logger Logs = new Logger(LogName);
    private static readonly JsonLogger JsonLogs = new JsonLogger();
    
    public static void Start()
    {
        bool endApp = false;
        Intro();

        while (!endApp)
        {
            _num1 = AskUserANumber();
            Library.Operator op = AskUserOperand();
            // For some operation, only one number is needed, take the value of num1 for easier print
            _num2 = (op.AmountInput() > 1) ? AskUserANumber("second") : _num1;

            try
            {
                PrintResult(op);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            endApp = AskCloseApp();
        }
        
        Logs.Dispose();
        JsonLogs.Dispose();
    }
    
    private static void Intro()
    {
        // Display title as the C# console calculator app.
        Console.WriteLine("Console Calculator in C#\r");
        Console.WriteLine("------------------------\n");
    }

    private static double AskUserANumber(string amount = "first")
    {
        return AskUser.Ask<double>(
            [$"Type the {amount} number, and press ENTER"]);
    }

    private static Library.Operator AskUserOperand()
    {
        return AskUser.Ask<Library.Operator>(
            ["Choose an option from the following list",
                "\t+ - Add",
                "\t- - Subtract",
                "\t* - Multiply",
                "\t/ - Divide",
                "\tsqrt - Square root",
                "\t^ - Power",
                "\tcos - Cosinus",
                "\tsin - Sinus"]);
    }

    private static void PrintResult(Library.Operator op)
    {
        double result = op.Result(_num1, _num2);
        Console.WriteLine($"Your result: {(op.AmountInput() > 1 ? _num1 : "")} {op.GetSymbol()} {_num2} = {result}");
        Logs.Result(_num1, _num2, op, result);
        JsonLogs.LogResult(_num1, _num2, op, result);
    }

    private static bool AskCloseApp()
    {
        Console.WriteLine("Press 'n' and Enter to close the app, or press 'd' and Enter to delete the logs or press any other key and Enter to continue: ");
        string? input = Console.ReadLine();
        if (input == "d")
        {
            Logs.DeleteLogs();
            JsonLogs.DeleteLogs();
            return AskCloseApp();
        }
        return (input == "n");
    }
}