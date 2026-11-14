using System.Text.RegularExpressions;
using CalculatorLibrary;

Calculator calculator = new Calculator();
bool endApp = false;
// Display title as the C# console calculator app.
Console.WriteLine("Console Calculator in C#\r");
Console.WriteLine("------------------------\n");

while (!endApp)
{
   // Declare variables and set to empty.
   // Use Nullable types (with ?) to match type of System.Console.ReadLine
   string? userChoice = "";
   double result = 0;

   Console.WriteLine("1. Enter numers");
   Console.WriteLine("2. Use from list");
   Console.Write("Enter option number => ");
   userChoice = Console.ReadLine();

   switch (userChoice)
   {
      case "1":
         calculator.EnterNumbers();
         break;
      case "2":
         try
         {
            calculator.ChooseNumbersFromHistoryList();
         }
         catch (IndexOutOfRangeException ex)
         {
            Console.WriteLine(ex.Message);
            calculator.EnterNumbers();
         }
         break;
   }

   // Ask the user to choose an operator.
   Console.WriteLine("Choose an operator from the following list:");
   Console.WriteLine("\ta - Add");
   Console.WriteLine("\ts - Subtract");
   Console.WriteLine("\tm - Multiply");
   Console.WriteLine("\td - Divide");
   Console.Write("Your option? ");

   string? op = Console.ReadLine();

   // Validate input is not null, and matches the pattern
   if (op == null || !Regex.IsMatch(op, "^(a|s|m|d|u)$"))
   {
      Console.WriteLine("Error: Unrecognized input.");
   }
   else
   {
      try
      {
         result = calculator.DoOperation(op);
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

   // Wait for the user to respond before closing.
   Console.WriteLine("\tn - Exit");
   Console.WriteLine("\tu - Calculator usage");
   Console.WriteLine("\th - Calculation history");
   Console.Write("Your option? ");

   switch (Console.ReadLine())
   {
      case "n":
         endApp = true;
         break;
      case "u":
         calculator.ShowUsage();
         break;
      case "h":
         calculator.ShowHistory();
         break;
   }

   Console.WriteLine("\n"); // Friendly linespacing.
}

calculator.Finish();