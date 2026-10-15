using System.Drawing;
using System.Text.RegularExpressions;
using CalculatorLibrary;

bool endApp = false;
int useCalculator = -1;
List<double> numberList = new();
Calculator calculator = new Calculator();

Console.WriteLine("Console Calculator in C#\r");
Console.WriteLine("------------------------\n");

while (!endApp)
{
    useCalculator++;
    Console.WriteLine($"Using Calculator {useCalculator} time(s)\n");
    double number1 = 0;
    double number2 = 0;
    double result = 0;

    if (numberList.Count > 0)
    {
        bool validHistoryChoice = false;

        while (!validHistoryChoice)
        {
            Console.WriteLine("Do you want to use a previous result in this calcultaor ?");
            Console.WriteLine("y - yes\nn - no\nd - delete");
            Console.Write("Your option?");
            string? choice = Console.ReadLine()?.Trim().ToLower();

            switch (choice)
            {
                case "d":
                    numberList.Clear();
                    Console.WriteLine("History deleted.\n");
                    validHistoryChoice = true;
                    break;

                case "y":
                    number1 = SelectFromHistory(calculator, numberList); 
                    number2 = GetNumberInput("Type another number, and the press and then press Enter: ");
                    validHistoryChoice = true;
                    break;
                case "n":
                    number1 = GetNumberInput("Type a number, and then press Enter: ");
                    number2 = GetNumberInput("Type another number, and the press and then press Enter: ");
                    validHistoryChoice = true;
                    break;
                default:
                    ErrorMensagem("Error: Unrecognized input. Please enter 'y','n' or 'd' .");
                    break;
            }
        }
    }
    
    if (numberList.Count == 0)
    {
        number1 = GetNumberInput("Type a number, and then press Enter: ");
        number2 = GetNumberInput("Type another number, and the press and then press Enter: ");
    }
   
    bool validChooseOp = false;
    string? op = "";
    while (!validChooseOp)
    {
        Console.WriteLine("\nChoose an operator from the following list:\n");
        Console.WriteLine("\ta - Add");
        Console.WriteLine("\ts - Subtract");
        Console.WriteLine("\tm - Multiply");
        Console.WriteLine("\td - Divide");
        Console.WriteLine("\tsq - Square");
        Console.WriteLine("\tp - Power");
        Console.WriteLine("\t10 - 10x");
        Console.WriteLine("\tsin - Seno");
        Console.WriteLine("\tcos - Cosseno");
        Console.WriteLine("\ttan - Tangente");
        Console.Write("Your option? ");
        op = Console.ReadLine();
        if (op == null || !Regex.IsMatch(op, "^(a|s|m|d|sq|p|10|tan|cos|sin)$"))
        {
            ErrorMensagem("Error: Unrecognized input.");
            continue;
        }
        validChooseOp = true;
    }
    

    if (Regex.IsMatch(op!, "^(10|sq|tan|cos|sin)$"))
    {
        bool validNumberChoose = false;
        string? numberOp = "";
        while (!validNumberChoose)
        {
            Console.Clear();
            Console.WriteLine("Choose um number");
            Console.WriteLine("1 - number 1");
            Console.WriteLine("2 - Number 2");

            numberOp = Console.ReadLine();

            if (numberOp == null || !Regex.IsMatch(numberOp, "^(1|2)$"))
            {
                ErrorMensagem("Error: Unrecognized input.");
                continue;
            }
            validNumberChoose = true;
        }
        
        double number = numberOp == "1" ? number1 : numberOp == "2" ? number2 : 0;

        result = calculator.DoOtherOperation(number, op!);
        if (double.IsNaN(result))
        {
            ErrorMensagem("This operation will result in a mathematical error.");
        }
        else
        {
            Console.WriteLine("Your result: {0:0.##}\n", result);
            numberList.Add(result);
        }
    }
    else
    {
        try
        {
            result = calculator.DoOperation(number1, number2, op!);
            if (double.IsNaN(result))
            {
                ErrorMensagem("This operation will result in a mathematical error.");
            }
            else
            {
                Console.WriteLine("Your result: {0:0.##}\n", result);
                numberList.Add(result);
            }
        }
        catch (Exception e)
        {
            ErrorMensagem("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
        }
    }

    Console.WriteLine("------------------------\n");

    Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
    if (Console.ReadLine() == "n") endApp = true;

    Console.WriteLine("\n");
}
calculator.Finish();
return;

static double GetNumberInput(string prompt)
{
    Console.WriteLine(prompt);
    string? input = Console.ReadLine();
    double number;

    while (!double.TryParse(input, out number))
    {
        Console.Write("Invalid input. Please enter a valid number: ");
        input = Console.ReadLine();
    }
    return number;
}

static double SelectFromHistory(Calculator calculator, List<double> numberList)
{
    while (true)
    {
        Console.WriteLine("\nChoose a number from History:");
        calculator.ListNumbers(numberList);
        Console.Write("Enter index: ");

        if (int.TryParse(Console.ReadLine(), out int choose) && choose > 0 && choose <= numberList.Count)
        {
            return numberList[choose - 1];
        }

        Console.WriteLine($"Invalid choice. Please enter an index between 1 and {numberList.Count}.");
    }
}

static void ErrorMensagem(string prompt)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.BackgroundColor = ConsoleColor.DarkRed;
    Console.Write(prompt + " Press Enter to continue.");
    Console.ResetColor();

    Console.ReadLine();
}