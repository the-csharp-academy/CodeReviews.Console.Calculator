
using System.Text.RegularExpressions;
using CalculatorLibrary;
using Validation;

const int CalculatorAppMenu = 1;
const int ListHistoryMenu = 2;
const int DeleteHistoryMenu = 3;

double? listInput1 = null;
double? listInput2 = null;

bool endApp = false;
// Display title as the C# console calculator app.
Console.WriteLine("Console Calculator in C#\r");
Console.WriteLine("------------------------\n");

Calculator calculator = new Calculator();

while (!endApp)
{
    ListMenu();

    string? menuInput = "";
    Console.Write("Type a menu number, and then press Enter: ");
    menuInput = Console.ReadLine();

    double chosenMenu = 0;
    while (!double.TryParse(menuInput, out chosenMenu))
    {
        Console.Write("This is not valid input. Please enter a numeric value: ");
        menuInput = Console.ReadLine();
    }

    switch (chosenMenu)
    {
        case CalculatorAppMenu:
            CalculatorApp();
            break;
        case ListHistoryMenu:
            calculator.ListHistory();
            PreviousResultMenu();
            break;
        case DeleteHistoryMenu:
            calculator.ClearList();
            break;
        default:
            break;
    }

    Console.WriteLine("\n"); // Friendly linespacing. 

    // Wait for the user to respond before closing.
    Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
    if (Console.ReadLine() == "n") endApp = true;

    Console.WriteLine("\n"); // Friendly linespacing. 
}

calculator.Finish();

void CalculatorApp()
{
    // Declare variables and set to empty.
    double result = 0;
    double cleanNum1 = listInput1 ?? ValidatorService.GetUserInputForDouble("first");
    double cleanNum2 = listInput2 ?? ValidatorService.GetUserInputForDouble("second");

    // Ask the user to choose an operator.
    Console.WriteLine("Choose an operator from the following list:");
    Console.WriteLine("\ta - Add");
    Console.WriteLine("\ts - Subtract");
    Console.WriteLine("\tm - Multiply");
    Console.WriteLine("\td - Divide");
    Console.WriteLine("\te - Exponentiation");
    Console.WriteLine();
    Console.WriteLine("First number is used:");
    Console.WriteLine("\tq - Square root");
    Console.WriteLine("\ti - Sine(°)");
    Console.WriteLine("\to - Cosine(°)");
    Console.WriteLine("\tt - Tangent(°)");
    Console.WriteLine("\tc - Cotangent(°)");
    Console.Write("Your option? ");

    string? op = Console.ReadLine();

    // Validate input is not null, and matches the pattern
    if (op == null || ! Regex.IsMatch(op, "[a|s|m|d|q|e|i|o|t|c]"))
    {
        Console.WriteLine("Error: Unrecognized input.");
    }
    else
    { 
        try
        {
            result = calculator.DoOperation(cleanNum1, cleanNum2, op);
            if (double.IsNaN(result))
            {
                Console.WriteLine("This operation will result in a mathematical error.\n");
            }
            else Console.WriteLine("Your result: {0:0.##########}\n", result);
        }
        catch (Exception e)
        {
            Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
        }
    }
    Console.WriteLine("------------------------\n");

    listInput1 = null;
    listInput2 = null;
}

void ListMenu()
{
    Console.WriteLine("------------------------");
    Console.WriteLine($"{CalculatorAppMenu} - Calculator");
    Console.WriteLine($"{ListHistoryMenu} - List History");
    Console.WriteLine($"{DeleteHistoryMenu} - Delete History");
    Console.WriteLine("------------------------\n");
}

void PreviousResultMenu()
{
    if (!calculator.HistoryHasEnoughResults(1))
        return;

    string? input = "";
    
    Console.Write("Do you want to use previous results from the list? (y/n): ");
    input = Console.ReadLine();

    if (input != "y")
    {
        Console.WriteLine("\nExiting list...");
        return;
    }
    
    Console.Write("\nHow many previous result do you want to use? (1-2): ");
    input = Console.ReadLine();

    int numbersCount = ValidatorService.GetValidIntInputValue(input);
    bool validNumberWasChosen = numbersCount == 1 || numbersCount == 2;
    bool historyHasNotEnoughResult = numbersCount == 2 && !calculator.HistoryHasEnoughResults(2);

    if (!validNumberWasChosen || historyHasNotEnoughResult)
    {
        Console.WriteLine("Input is not valid, or not enough results are available. Exiting list...\n\n");
        return;
    }

    UseNumbersFromList(numbersCount);
}

void UseNumbersFromList(int inputCount)
{
    double? num2 = null;
    
    calculator.ListHistory();
    
    double? num1 = GetHistoryResult("first");
    
    if (inputCount == 2)
        num2 = GetHistoryResult("second");

    listInput1 = num1;
    listInput2 = num2;

    CalculatorApp();
}

double GetHistoryResult(string numberCountText)
{
    CalculatorHistory? previousResult = null;
    bool validHistoryChosen = false;

    do
    { 
        Console.Write($"Choose the number of the results for the {numberCountText} number: ");

        string? input = Console.ReadLine();
        int chosenNumber = ValidatorService.GetValidIntInputValue(input);

        previousResult = calculator.GetHistoryByIndex(chosenNumber - 1);

        if (previousResult == null)
            Console.WriteLine($"\nThere is no result available for index {chosenNumber}");
        else
            validHistoryChosen = true;

    } while (!validHistoryChosen);
    

    return previousResult!.Result;
}