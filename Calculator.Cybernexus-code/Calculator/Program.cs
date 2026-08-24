
using System.Text.RegularExpressions;
using CalculatorLibrary;

bool endApp = false;

Calculator calculator = new Calculator();
while (!endApp)
{
    Console.Clear();
    string? numInput1 = "";
    string? numInput2 = "";
    double result = 0;

    Console.WriteLine("Type a number, and then press Enter");
    numInput1 = Console.ReadLine();

    double cleanNum1 = 0;
    while (!double.TryParse(numInput1, out cleanNum1))
    {
        Console.WriteLine("This is not valid input. Please enter an integer value: ");
        numInput1 = Console.ReadLine();
    };

    Console.WriteLine("Type another number, and then press Enter");
    numInput2 = Console.ReadLine();

    double cleanNum2 = 0;
    while(!double.TryParse(numInput2, out cleanNum2))
    {
        Console.WriteLine("This is not valid input. Please enter an integer value: ");
        numInput2 = Console.ReadLine();
    };

    Console.WriteLine("Cjhoose an operator from the following list:");
    Console.WriteLine("\t+ - Add"); 
    Console.WriteLine("\t- - Subtract");
    Console.WriteLine("\t* - Multiply");
    Console.WriteLine("\t/ - Divide");

    string? op = Console.ReadLine();

    if (op == null || !Regex.IsMatch(op, "^([+\\-*/])$"))
    {
        Console.WriteLine("Error! Please enter a valid operator.");
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
            else Console.WriteLine("Your result: {0:0.##}\n", result);
        }
        catch (Exception e)
        {
            Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
        }
    }
    Console.WriteLine("------------------------\n");

    Console.WriteLine("Press 'q' and Enter to quit the app, or press any other key and Enter to continue: ");

    if (Console.ReadKey().Key == ConsoleKey.Q) endApp = true;

}
calculator.Finish();
return;