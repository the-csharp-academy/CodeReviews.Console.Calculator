using System.Text.RegularExpressions;
using CalculatorLibrary;

bool endApp = false;
int useCalculator = -1;
List<double> numberList = new();
using var calculator = new Calculator();

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
            Console.WriteLine("Speak:\nYes or No or Delete");
            Console.Write("Your option?");


            var choice = await Spechtext.OutputSpeech();
            string choiceText = choice.Text.Trim().TrimEnd('.').ToLower();

            switch (choiceText)
            {
                case "delete":
                    numberList.Clear();
                    Console.WriteLine("History deleted.\n");
                    validHistoryChoice = true;
                    number1 = await GetNumberInput("Type a number, then press Enter: ");
                    number2 = await GetNumberInput("Type another number, then press Enter: ");
                    break;

                case "yes":
                    number1 = await SelectFromHistory(calculator, numberList);
                    number2 = await GetNumberInput("Type another number, and the press and then press Enter: ");
                    validHistoryChoice = true;
                    break;
                case "no":
                    number1 = await GetNumberInput("Type a number, and then press Enter: ");
                    number2 = await GetNumberInput("Type another number, and the press and then press Enter: ");
                    validHistoryChoice = true;
                    break;
                default:
                    ErrorMensagem("Error: Unrecognized input. Please Speak 'yes','no' or 'delete' .");
                    break;
            }
        }
    }

    else
    {
        number1 = await GetNumberInput("Type a number, and then press Enter: ");
        number2 = await GetNumberInput("Type another number, and the press and then press Enter: ");
    }

    bool validChooseOp = false;
    string op = "";

    while (!validChooseOp)
    {
        Console.WriteLine("\nChoose an operator from the following list:\n");
        Console.WriteLine("\t1 - Add");
        Console.WriteLine("\t2 - Subtract");
        Console.WriteLine("\t3 - Multiply");
        Console.WriteLine("\t4 - Divide");
        Console.WriteLine("\t5 - Square");
        Console.WriteLine("\t6 - Power");
        Console.WriteLine("\t7 - 10x");
        Console.WriteLine("\t8 - Sine");
        Console.WriteLine("\t9 - Cosine");
        Console.WriteLine("\t10 - Tangente");
        Console.Write("Your option? ");

        var opSpeech = await Spechtext.OutputSpeech();
        op = opSpeech.Text.Trim().TrimEnd('.');

        if (!Regex.IsMatch(op, @"^(1|2|3|4|5|6|7|8|9|10)$"))
        {
            ErrorMensagem("Error: Unrecognized input.");
            continue;
        }
        validChooseOp = true;
    }


    if (Regex.IsMatch(op!, @"^(5|7|8|9|10)$"))
    {
        bool validNumberChoose = false;
        string numberOp = "";

        while (!validNumberChoose)
        {
            Console.WriteLine("Choose um number");
            Console.WriteLine("1 - number 1");
            Console.WriteLine("2 - Number 2");

            var numberOpSpeech = await Spechtext.OutputSpeech();
            numberOp = numberOpSpeech.Text.TrimEnd('.').Trim();

            if (!Regex.IsMatch(numberOp, @"^(1|2)$"))
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
return;

static async Task<double> GetNumberInput(string prompt)
{
    Console.WriteLine(prompt);
    string input;
    double number;

    do
    {
        var speechNumber = await Spechtext.OutputSpeech();
        input = speechNumber.Text.Trim().TrimEnd('.');

        if (!double.TryParse(input, out number))
        {
            Console.Write("Invalid input. Please say a valid number: ");
        }
    } while (!double.TryParse(input, out number));

    return number;
}

static async Task<double> SelectFromHistory(Calculator calculator, List<double> numberList)
{
    while (true)
    {
        Console.WriteLine("\nChoose a number from History:");
        calculator.ListNumbers(numberList);
        Console.Write("Enter index: ");

        var speechChoose = await Spechtext.OutputSpeech();

        if (int.TryParse(speechChoose.Text.TrimEnd('.'), out int choose) && choose > 0 && choose <= numberList.Count)
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