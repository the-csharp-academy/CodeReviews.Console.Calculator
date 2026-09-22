using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Microsoft.Calculator.CalculatorLibrary;
using Microsoft.Calculator.CalculatorLibrary.Enums;
using Microsoft.Calculator.CalculatorLibrary.Models;
using Microsoft.CognitiveServices.Speech;
using Microsoft.Extensions.Configuration;

class Program
{
  private static string GetSecret(string secretKey)
  {
    var config = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

    string? key = config[secretKey];
    if (key == null)
    {
      Console.WriteLine("Issue getting secret...");
      return "N/A";
    }
    return key;
  }
  private static string GetSpeechServiceKey()
  {
    return GetSecret("SpeechService:key");
  }
  private static string GetSpeechServiceRegion()
  {
    return GetSecret("SpeechService:region");
  }
  private static string CleanSpokenNumber(string text) =>
    Regex.Replace(text, @"[^0-9.\-]", "");

  private static async Task<double?> TryGetNumberFromMic()
  {
    try
    {
      var config = SpeechConfig.FromSubscription(GetSpeechServiceKey(), GetSpeechServiceRegion());
      using var recognizer = new SpeechRecognizer(config);
      Console.WriteLine("Using your mic, tell us a number...");
      var micResult = await recognizer.RecognizeOnceAsync();

      if (micResult.Reason != ResultReason.RecognizedSpeech)
      {
        Console.WriteLine("Didn't catch that from the mic, switching to text input.");
        return null;
      }

      string cleaned = CleanSpokenNumber(micResult.Text);
      if (double.TryParse(cleaned, out double spokenNumber))
      {
        Console.WriteLine($"Heard: {spokenNumber}");
        return spokenNumber;
      }

      Console.WriteLine($"Couldn't turn \"{micResult.Text}\" into a number, switching to text input.");
      return null;
    }
    catch (Exception e)
    {
      Console.WriteLine("Mic isn't available, switching to text input.\n - Details: " + e.Message);
      return null;
    }
  }

  private static async Task<double> GetValidNumber()
  {
    double? spokenNumber = await TryGetNumberFromMic();
    if (spokenNumber.HasValue) return spokenNumber.Value;

    Console.Write("Type a number: ");
    string? userInput = Console.ReadLine();
    double cleanInput;
    while (!double.TryParse(userInput, out cleanInput))
    {
      Console.Write("This is not valid input. Please enter a numeric value: ");
      userInput = Console.ReadLine();
    }
    return cleanInput;
  }
  private static async Task<double> GetNumberOrHistoryResult()
  {
    List<CalculationRecord> history = calc.GetHistory();
    if (history.Count == 0) return await GetValidNumber();

    Console.Write("Type a number, or 'p' to use a previous result: ");
    string? userInput = Console.ReadLine();

    if (userInput?.Trim().ToLower() != "p")
    {
      double cleanInput;
      while (!double.TryParse(userInput, out cleanInput))
      {
        Console.Write("This is not valid input/ Please enter a numeric value: ");
        userInput = Console.ReadLine();
      }
      return cleanInput;
    }

    DisplayHistory();
    Console.Write($"Which result? (1-{history.Count}): ");
    int index;
    while (!int.TryParse(Console.ReadLine(), out index) || index < 1 || index > history.Count)
    {
      Console.Write($"Please enter a number between 1 and {history.Count}: ");
    }
    return history[index - 1].Result;
  }
  private static OperationType GetValidOperationType()
  {
    // Ask the user to choose an operator.
    Console.WriteLine("Choose an operator from the following list:");
    Console.WriteLine("\tadd  - Add");
    Console.WriteLine("\tsub  - Subtract");
    Console.WriteLine("\tmul  - Multiply");
    Console.WriteLine("\tdiv  - Divide");
    Console.WriteLine("\tsqrt - Square root");
    Console.WriteLine("\tpow  - Raise to a power");
    Console.WriteLine("\tx10  - Multiply by 10");
    Console.WriteLine("\tsin  - Sine (degrees)");
    Console.WriteLine("\tcos  - Cosine (degrees)");
    Console.WriteLine("\ttan  - Tangent (degrees)");
    Console.Write("Your option? ");

    string? opInput = Console.ReadLine()?.Trim().ToLower();

    while (opInput == null || !Regex.IsMatch(opInput, "^(add|sub|mul|div|sqrt|pow|x10|sin|cos|tan)$"))
    {
      Console.WriteLine("This is not a valid option. Please choose one of the options listed above.");
      opInput = Console.ReadLine()?.Trim().ToLower();
    }

    OperationType op = opInput switch
    {
      "add" => OperationType.Add,
      "sub" => OperationType.Subtract,
      "mul" => OperationType.Multiply,
      "div" => OperationType.Divide,
      "sqrt" => OperationType.SquareRoot,
      "pow" => OperationType.TakingThePower,
      "x10" => OperationType.TimesTen,
      "sin" => OperationType.Sine,
      "cos" => OperationType.Cosine,
      _ => OperationType.Tangent,
    };

    return op;
  }

  private static bool IsUnaryOperation(OperationType op) =>
    op is OperationType.SquareRoot or OperationType.TimesTen or OperationType.Sine or OperationType.Cosine or OperationType.Tangent;

  private static MenuChoice GetValidMenuChoice()
  {
    Console.WriteLine("View history (h) or new calculation (c)?: ");
    string? choiceInput = Console.ReadLine();

    while (choiceInput == null || !Regex.IsMatch(choiceInput, "^(h|c)$"))
    {
      Console.WriteLine($"{choiceInput} is not a valid option, please choose (h)istory or (c)alculation");
      choiceInput = Console.ReadLine();
    }

    MenuChoice choice = choiceInput switch
    {
      "c" => MenuChoice.Calculation,
      _ => MenuChoice.History
    };

    return choice;
  }
  private static async Task Calculation()
  {
    OperationType op = GetValidOperationType();

    // Ask the user to type the first number.
    double cleanNum1 = await GetNumberOrHistoryResult();

    double cleanNum2 = 0;
    if (!IsUnaryOperation(op))
    {
      // Ask the user to type the second number.
      cleanNum2 = await GetNumberOrHistoryResult();
    }

    double result;

    try
    {
      result = calc.DoOperation(cleanNum1, cleanNum2, op);
      if (double.IsNaN(result))
      {
        Console.WriteLine("This operation will result in a mathematical error.\n");
      }
      else
      {
        Console.WriteLine("Your result: {0:0.##}\n", result);
        Console.WriteLine($"Calculations completed - {calc.GetCalculatorCount()}");
      }
    }
    catch (Exception e)
    {
      Console.WriteLine("Oh no! An exception occurred trying to do the math.\n - Details: " + e.Message);
    }
  }
  private static string GetOperationSymbol(OperationType op) => op switch
  {
    OperationType.Add => "+",
    OperationType.Subtract => "-",
    OperationType.Multiply => "x",
    OperationType.Divide => "÷",
    OperationType.SquareRoot => "√",
    OperationType.TakingThePower => "^",
    OperationType.TimesTen => "x 10",
    OperationType.Sine => "sin",
    OperationType.Cosine => "cos",
    _ => "tan",
  };

  private static void DisplayHistory()
  {
    List<CalculationRecord> history = calc.GetHistory();
    for (int i = 0; i < history.Count; i++)
    {
      CalculationRecord record = history[i];
      string symbol = GetOperationSymbol(record.Operation);
      string expression = record.Operation switch
      {
        OperationType.SquareRoot => $"{symbol}{record.Num1}",
        OperationType.TimesTen => $"{record.Num1} {symbol}",
        OperationType.Sine or OperationType.Cosine or OperationType.Tangent => $"{symbol}({record.Num1}°)",
        _ => $"{record.Num1} {symbol} {record.Num2}",
      };
      Console.WriteLine($"{i + 1}. {expression} = {record.Result}");
    }
  }

  static readonly Calculator calc = new();
  static async Task Main(string[] args)
  {
    bool endApp = false;
    // Display title as the C# console calculator app.
    Console.WriteLine("Console Calculator in C#\r");
    Console.WriteLine("------------------------\n");

    while (!endApp)
    {
      if (calc.GetCalculatorCount() >= 1)
      {
        MenuChoice choice = GetValidMenuChoice();
        if (choice == MenuChoice.Calculation)
        {
          await Calculation();
        }
        else
        {
          DisplayHistory();
        }
      }
      else
      {
        await Calculation();
      }

      Console.WriteLine("------------------------\n");

      // Wait for the user to respond before closing.
      Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
      if (Console.ReadLine() == "n") endApp = true;

      Console.WriteLine("\n"); // Friendly linespacing.
    }
    calc.Finish();
    return;
  }
}