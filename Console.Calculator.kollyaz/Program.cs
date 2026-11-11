using System.Globalization;

namespace RefactoredCalculator;

public static class Program
{
  public static void Main()
  {
    Console.WriteLine("\nConsole Calculator in C#\r");
    Console.WriteLine("--------------------------------------------------\n");

    CalculatorApplication app = new();
    app.Run();
  }
}

public sealed class CalculatorApplication
{
  private readonly CalculationHistory history = new();
  private readonly BasicCalculator basicCalculator;
  private readonly ScientificCalculator scientificCalculator;

  public CalculatorApplication()
  {
    basicCalculator = new BasicCalculator(history);
    scientificCalculator = new ScientificCalculator(history);
  }

  public void Run()
  {
    bool exit = false;

    while (!exit)
    {
      Console.WriteLine("Please enter what operation you would like to use:");
      Console.WriteLine("\t1 - Basic Math");
      Console.WriteLine("\t2 - Scientific Math");
      Console.WriteLine("\t3 - All History");
      Console.WriteLine("\t4 - Exit");


      string choice = ConsoleInput.ReadChoice(["1", "2", "3", "4"]);

      try
      {
        double? result = choice switch
        {
          "1" => basicCalculator.Run(),
          "2" => scientificCalculator.Run(),
          "3" => ShowHistory(),
          "4" => null,
          _ => null
        };

        exit = choice == "4";

        if (result.HasValue)
        {
          Console.WriteLine($"Your result: {result.Value:0.##}");
        }
      }
      catch (DivideByZeroException exception)
      {
        Console.WriteLine($"Error: {exception.Message}");
      }
      catch (ArgumentException exception)
      {
        Console.WriteLine($"Error: {exception.Message}");
      }

      Console.WriteLine("Press 'e' and Enter to close, or press any other key to continue.");
      exit = string.Equals(Console.ReadLine(), "e", StringComparison.OrdinalIgnoreCase);
    }
  }

  private double? ShowHistory()
  {
    history.ShowAll();
    return null;
  }
}

internal sealed class BasicCalculator
{
  private readonly CalculationHistory history;

  public BasicCalculator(CalculationHistory history)
  {
    this.history = history;
  }

  public double? Run()
  {
    Console.WriteLine("Choose an operator:");
    Console.WriteLine("\t1 - Add");
    Console.WriteLine("\t2 - Subtract");
    Console.WriteLine("\t3 - Multiply");
    Console.WriteLine("\t4 - Divide");
    Console.WriteLine("\t5 - History");

    string choice = ConsoleInput.ReadChoice(["1", "2", "3", "4", "5"]);
    if (choice == "5")
    {
      ShowHistory();
      return null;
    }

    double firstNumber = PreviousResultPrompt.TrySelect(history, CalculationKind.Basic)
      ?? ConsoleInput.ReadDouble("Enter the first number: ");
    double secondNumber = ConsoleInput.ReadDouble("Enter the second number: ");
    BasicOperation operation = choice switch
    {
      "1" => BasicOperation.Add,
      "2" => BasicOperation.Subtract,
      "3" => BasicOperation.Multiply,
      "4" => BasicOperation.Divide,
      _ => throw new ArgumentOutOfRangeException(nameof(choice))
    };

    double result = BasicOperations.Calculate(firstNumber, secondNumber, operation);
    string symbol = operation switch
    {
      BasicOperation.Add => "+",
      BasicOperation.Subtract => "-",
      BasicOperation.Multiply => "×",
      BasicOperation.Divide => "÷",
      _ => throw new ArgumentOutOfRangeException(nameof(operation))
    };

    history.Add($"{firstNumber} {symbol} {secondNumber}", result, CalculationKind.Basic);
    return result;
  }

  private void ShowHistory()
  {
    history.Show(CalculationKind.Basic);
    Console.WriteLine("Enter 'a' to delete basic history, or press Enter to return.");
    if (string.Equals(Console.ReadLine(), "a", StringComparison.OrdinalIgnoreCase))
    {
      history.Delete(CalculationKind.Basic);
      Console.WriteLine("Basic history deleted.");
    }
  }
}

internal enum BasicOperation
{
  Add,
  Subtract,
  Multiply,
  Divide
}

internal static class BasicOperations
{
  public static double Calculate(double firstNumber, double secondNumber, BasicOperation operation)
  {
    return operation switch
    {
      BasicOperation.Add => firstNumber + secondNumber,
      BasicOperation.Subtract => firstNumber - secondNumber,
      BasicOperation.Multiply => firstNumber * secondNumber,
      BasicOperation.Divide when secondNumber != 0 => firstNumber / secondNumber,
      BasicOperation.Divide => throw new DivideByZeroException("Cannot divide by zero."),
      _ => throw new ArgumentOutOfRangeException(nameof(operation))
    };
  }
}

internal sealed class ScientificCalculator
{
  private readonly CalculationHistory history;

  public ScientificCalculator(CalculationHistory history)
  {
    this.history = history;
  }

  public double? Run()
  {
    Console.WriteLine("Choose an operator:");
    Console.WriteLine("\t1 - Square Root");
    Console.WriteLine("\t2 - Power");
    Console.WriteLine("\t3 - 10^x");
    Console.WriteLine("\t4 - Sin");
    Console.WriteLine("\t5 - Cos");
    Console.WriteLine("\t6 - Tan");
    Console.WriteLine("\t7 - History");

    string choice = ConsoleInput.ReadChoice(["1", "2", "3", "4", "5", "6", "7"]);
    if (choice == "7")
    {
      ShowHistory();
      return null;
    }

    double number = PreviousResultPrompt.TrySelect(history, CalculationKind.Scientific)
      ?? ConsoleInput.ReadDouble("Enter the number to operate on: ");
    double exponent = choice == "2"
      ? ConsoleInput.ReadDouble("Enter the exponent: ")
      : 0;
    ScientificOperation operation = choice switch
    {
      "1" => ScientificOperation.SquareRoot,
      "2" => ScientificOperation.Power,
      "3" => ScientificOperation.PowerOfTen,
      "4" => ScientificOperation.Sine,
      "5" => ScientificOperation.Cosine,
      "6" => ScientificOperation.Tangent,
      _ => throw new ArgumentOutOfRangeException(nameof(choice))
    };

    double result = ScientificOperations.Calculate(number, exponent, operation);
    string expression = operation switch
    {
      ScientificOperation.SquareRoot => $"√({number})",
      ScientificOperation.Power => $"{number}^{exponent}",
      ScientificOperation.PowerOfTen => $"10^{number}",
      ScientificOperation.Sine => $"sin({number})",
      ScientificOperation.Cosine => $"cos({number})",
      ScientificOperation.Tangent => $"tan({number})",
      _ => throw new ArgumentOutOfRangeException(nameof(operation))
    };

    history.Add(expression, result, CalculationKind.Scientific);
    return result;
  }

  private void ShowHistory()
  {
    history.Show(CalculationKind.Scientific);
    Console.WriteLine("Enter 'a' to delete scientific history, or press Enter to return.");
    if (string.Equals(Console.ReadLine(), "a", StringComparison.OrdinalIgnoreCase))
    {
      history.Delete(CalculationKind.Scientific);
      Console.WriteLine("Scientific history deleted.");
    }
  }
}

internal enum ScientificOperation
{
  SquareRoot,
  Power,
  PowerOfTen,
  Sine,
  Cosine,
  Tangent
}

internal static class ScientificOperations
{
  public static double Calculate(double number, double exponent, ScientificOperation operation)
  {
    return operation switch
    {
      ScientificOperation.SquareRoot => SquareRoot(number),
      ScientificOperation.Power => Math.Pow(number, exponent),
      ScientificOperation.PowerOfTen => Math.Pow(10, number),
      ScientificOperation.Sine => Math.Sin(ToRadians(number)),
      ScientificOperation.Cosine => Math.Cos(ToRadians(number)),
      ScientificOperation.Tangent => Math.Tan(ToRadians(number)),
      _ => throw new ArgumentOutOfRangeException(nameof(operation))
    };
  }

  private static double SquareRoot(double number)
  {
    if (number < 0)
    {
      throw new ArgumentException("Cannot calculate a real square root of a negative number.");
    }

    return Math.Sqrt(number);
  }

  private static double ToRadians(double degrees) => degrees * Math.PI / 180;
}

internal enum CalculationKind
{
  Basic,
  Scientific
}

internal sealed record Calculation(string Expression, double Result, CalculationKind Kind);

internal sealed class CalculationHistory
{
  private readonly List<Calculation> calculations = [];

  public int Count => calculations.Count;

  public bool HasCalculations(CalculationKind kind) =>
    calculations.Any(calculation => calculation.Kind == kind);

  public void Add(string expression, double result, CalculationKind kind)
  {
    calculations.Add(new Calculation(expression, result, kind));
  }

  public void ShowAll() => Show(null);

  public void Show(CalculationKind? kind)
  {
    Console.WriteLine("-----------Calculation History----------");
    var matches = calculations
      .Select((calculation, index) => (calculation, index))
      .Where(item => kind is null || item.calculation.Kind == kind)
      .ToList();

    if (matches.Count == 0)
    {
      Console.WriteLine(kind is null
        ? "No calculations have been made yet."
        : $"No {kind} calculations have been made yet.");
    }
    else
    {
      foreach (var item in matches)
      {
        Console.WriteLine(
          $"{item.index + 1}. {item.calculation.Expression} = {item.calculation.Result:0.##}");
      }
    }

    Console.WriteLine("-----------------------------------------");
  }

  public double GetResult(int selection)
  {
    if (selection < 1 || selection > calculations.Count)
    {
      throw new ArgumentOutOfRangeException(nameof(selection), "Choose a number shown in history.");
    }

    return calculations[selection - 1].Result;
  }

  public void Delete(CalculationKind kind)
  {
    calculations.RemoveAll(calculation => calculation.Kind == kind);
  }
}

internal static class PreviousResultPrompt
{
  public static double? TrySelect(CalculationHistory history, CalculationKind kind)
  {
    if (!history.HasCalculations(kind))
    {
      return null;
    }

    Console.WriteLine("Do you want to use a previous result? (y/n)");
    if (!string.Equals(Console.ReadLine(), "y", StringComparison.OrdinalIgnoreCase))
    {
      return null;
    }

    history.ShowAll();
    Console.WriteLine("Select a calculation:");
    int selection = ConsoleInput.ReadInteger(1, history.Count);
    return history.GetResult(selection);
  }
}

internal static class ConsoleInput
{
  public static string ReadChoice(IReadOnlyCollection<string> choices)
  {
    while (true)
    {
      string? input = Console.ReadLine()?.Trim();
      if (input is not null && choices.Contains(input))
      {
        return input;
      }

      Console.WriteLine("That is not a valid input. Please enter a valid input.");
    }
  }

  public static double ReadDouble(string prompt)
  {
    Console.Write(prompt);
    while (true)
    {
      if (double.TryParse(Console.ReadLine(), NumberStyles.Float | NumberStyles.AllowThousands,
        CultureInfo.CurrentCulture, out double value))
      {
        return value;
      }

      Console.WriteLine("Please enter a valid number.");
    }
  }

  public static int ReadInteger(int minimum, int maximum)
  {
    while (true)
    {
      if (int.TryParse(Console.ReadLine(), out int value) && value >= minimum && value <= maximum)
      {
        return value;
      }

      Console.WriteLine("Please select a valid number:");
    }
  }
}


//////////////////////////////////////////////////////////////////////
// class Program
// {
//   public static void Main()
//   {
//     Console.WriteLine("Console Calculator in C#\r");
//     Console.WriteLine("------------------------\n");

//     CalculatorApp app = new();
//     app.StartApp();
//   }
// }

// class CalculatorApp
// {
//   private bool closeApp = false;
//   private int amountUsed = 0;
//   private readonly History history = new();

//   private readonly BasicMathApp basicMath;
//   private readonly ScientificMathApp scientificMath;

//   public CalculatorApp()
//   {
//     basicMath = new BasicMathApp(history);
//     scientificMath = new ScientificMathApp(history);
//   }


//   public void StartApp()
//   {

//     while (!closeApp)
//     {

//       Console.WriteLine("Please enter what operation you would like to use:");
//       Console.WriteLine("\t1 - Basic Math");
//       Console.WriteLine("\t2 - Scientific Math");
//       Console.WriteLine("\t3 - All History");

//       string? userMath = Console.ReadLine();
//       while (userMath != "1" && userMath != "2" && userMath != "3")
//       {
//         Console.WriteLine("That is not a valid input. Please enter a valid input.");
//         userMath = Console.ReadLine();
//       }

//       try
//       {
//         double? result = null;
//         switch (userMath)
//         {
//           case "1":
//             result = basicMath.CalcB();
//             break;
//           case "2":
//             result = scientificMath.CalcS();
//             break;
//           case "3":
//             history.ShowAllHistory();
//             break;
//         }

//         // if (double.IsNaN(result))
//         // {
//         //   Console.WriteLine("This operation resulted in a mathematical error.");
//         // }
//         if (result.HasValue)
//         {
//           amountUsed++;
//           Console.WriteLine($"Your result: {result:0.##}\n");
//         }
//       }
//       catch (DivideByZeroException e)
//       {
//         Console.WriteLine($"Error: {e.Message}");
//       }
//       catch (ArgumentException e)
//       {
//         Console.WriteLine($"Error: {e.Message}");
//       }
//       catch (Exception e)
//       {
//         Console.WriteLine($"An unexpected error occurred: {e.Message}");
//       }

//       // Console.WriteLine("\n----------------------------");

//       Console.WriteLine("\n Press 'e' and Enter to close the app, or press any other key and Enter to continue");
//       if (Console.ReadLine() == "e") closeApp = true;

//       Console.WriteLine("\n");
//     }

//   }
// }

// class BasicMathApp
// {
//   private static readonly string[] Operations = ["1", "2", "3", "4", "5"];
//   private readonly History history;

//   public BasicMathApp(History history)
//   {
//     this.history = history;
//   }


//   public double? CalcB()
//   {
//     double result = 0;
//     double firstNum = 0;
//     double secondNum = 0;


//     Console.WriteLine("Choose an operator from the following:");
//     Console.WriteLine("\t1 - Add");
//     Console.WriteLine("\t2 - Substract");
//     Console.WriteLine("\t3 - Multiply");
//     Console.WriteLine("\t4 - Divide");
//     Console.WriteLine("\t5 - History");

//     Console.Write("Your option?\t");

//     string? op = Console.ReadLine();
//     if (string.IsNullOrWhiteSpace(op) || !Operations.Contains(op))
//     {
//       throw new ArgumentException("Unrecognized operator.");
//     }

//     if (op == "5")
//     {
//       history.ShowBasicHistory();

//       Console.WriteLine("\n");
//       Console.WriteLine("(a). Delete history");

//       string a = Console.ReadLine()!;

//       if (a.Equals("a", StringComparison.CurrentCultureIgnoreCase))
//         history.DeleteBasicHistory();

//       return null;
//     }


//     double? previousResult = PreviousResult.TryUse(history, "Basic");
//     if (previousResult.HasValue)
//     {
//       firstNum = previousResult.Value;
//     }
//     else
//     {
//       firstNum = NumberInput.ReadDouble("Type a number and then press enter: ");
//     }

//     secondNum = NumberInput.ReadDouble("Type a second number and then press enter: ");

//     string symbol = op switch
//     {
//       "1" => "+",
//       "2" => "-",
//       "3" => "×",
//       "4" => "÷",
//       _ => op
//     };

//     result = BasicMath.DoBasicMath(firstNum, secondNum, op);

//     history.AddHistory($"{firstNum} {symbol} {secondNum}", result, "Basic");

//     return result;

//   }

// }

// class BasicMath
// {
//   public static double DoBasicMath(double firstNum, double secondNum, string operation)
//   {

//     return operation switch
//     {
//       "1" => firstNum + secondNum,
//       "2" => firstNum - secondNum,
//       "3" => firstNum * secondNum,
//       "4" => secondNum != 0 ? firstNum / secondNum : throw new DivideByZeroException(),

//       _ => throw new InvalidOperationException()
//     };
//   }
// }

// public class ScientificMathApp
// {
//   private readonly History history;
//   private readonly string[] validOptions = ["1", "2", "3", "4", "5", "6", "7"];

//   public ScientificMathApp(History history)
//   {
//     this.history = history;
//   }

//   public double? CalcS()
//   {
//     double resultS = 0;
//     double userInp = 0;
//     double userExp = 0;

//     Console.WriteLine("Choose an operator from the following:");
//     Console.WriteLine("\t1 - Square Root");
//     Console.WriteLine("\t2 - Taking the power");
//     Console.WriteLine("\t3 - 10^x");
//     Console.WriteLine("\t4 - Sin");
//     Console.WriteLine("\t5 - Cos");
//     Console.WriteLine("\t6 - Tan");
//     Console.WriteLine("\t7 - History");

//     Console.Write("Your option?\t");

//     string? userAns = Console.ReadLine();

//     while (!validOptions.Contains(userAns))
//     {
//       Console.WriteLine("That is not a valid input. Please enter a valid input.");
//       userAns = Console.ReadLine();
//     }

//     if (userAns == "7")
//     {
//       history.ShowScientificHistory();

//       Console.WriteLine("\n");
//       Console.WriteLine("(a). Delete history");

//       string a = Console.ReadLine()!;

//       if (a.Equals("a", StringComparison.CurrentCultureIgnoreCase))
//       {
//         history.DeleteScientificHistory();
//         Console.WriteLine("Scientific history deleted.");
//       }

//       return null;
//     }

//     double? previousResult = PreviousResult.TryUse(history, "Scientific");
//     if (previousResult.HasValue)
//     {
//       userInp = previousResult.Value;
//     }
//     else
//     {
//       userInp = NumberInput.ReadDouble("Enter the number to operate on: ");
//     }

//     if (userAns == "2")
//     {
//       userExp = NumberInput.ReadDouble("Enter the exponent: ");

//     }


//     resultS = userAns switch
//     {
//       "1" => ScientificMath.SquareRoot(userInp),
//       "2" => ScientificMath.Power(userInp, userExp),
//       "3" => ScientificMath.PowerOfTen(userInp),
//       "4" => ScientificMath.Sin(userInp),
//       "5" => ScientificMath.Cos(userInp),
//       "6" => ScientificMath.Tan(userInp),
//       _ => throw new ArgumentException("Unrecognized scientific math operation.", nameof(userAns)),
//     };

//     string expression = userAns switch
//     {
//       "1" => $"√({userInp})",
//       "2" => $"{userInp}^{userExp}",
//       "3" => $"10^{userInp}",
//       "4" => $"sin({userInp})",
//       "5" => $"cos({userInp})",
//       "6" => $"tan({userInp})",
//       _ => userAns
//     };


//     history.AddHistory(expression, resultS, "Scientific");
//     return resultS;

//   }

// }

// public class ScientificMath
// {
//   public static double SquareRoot(double number)
//   {
//     double result = Math.Sqrt(number);

//     if (double.IsNaN(result))
//     {
//       throw new ArgumentException("You cannot calculate the square root of a negative number using real numbers.");
//     }
//     return result;

//   }

//   public static double Power(double number, double exponent)
//   {
//     return Math.Pow(number, exponent);
//   }

//   public static double PowerOfTen(double exponent)
//   {
//     return Math.Pow(10, exponent);
//   }
//   public static double Sin(double angle)
//   {
//     double radians = angle * (Math.PI / 180);
//     return Math.Sin(radians);
//   }

//   public static double Cos(double angle)
//   {
//     double radians = angle * (Math.PI / 180);
//     return Math.Cos(radians);
//   }

//   public static double Tan(double angle)
//   {
//     double radians = angle * (Math.PI / 180);
//     return Math.Tan(radians);
//   }
// }

// public class History
// {
//   private readonly List<CalculationRecord> history = [];

//   public void AddHistory(string expression, double result, string type)
//   {
//     history.Add(new CalculationRecord(expression, result, type));
//   }

//   public bool HasHistoryOfType(string type)
//   {
//     return history.Any(record => record.Type == type);
//   }

//   public void ShowBasicHistory()
//   {
//     ShowHistoryByType("Basic");
//   }

//   public void ShowScientificHistory()
//   {
//     ShowHistoryByType("Scientific");
//   }

//   public void ShowAllHistory()
//   {
//     ShowHistoryByType("All");
//   }



//   private void ShowHistoryByType(string type)
//   {
//     int displayNumber = 1;

//     Console.WriteLine("-----------Calculation History----------");
//     if (history.Count == 0)
//     {
//       Console.WriteLine("No calculation has been made yet");
//       // return;
//     }
//     else
//     {
//       for (int i = 0; i < history.Count; i++)
//       {
//         if (type == "All" || history[i].Type == type)
//         {
//           Console.WriteLine(
//                 $"{displayNumber}. {history[i].Expression} = {history[i].Result:0.##}"
//             );
//           displayNumber++;
//         }
//       }

//     }
//     Console.WriteLine("-----------------------------------------");

//   }

//   public double GetResult(int selection)
//   {
//     if (selection < 1 || selection > history.Count)
//     {
//       throw new ArgumentOutOfRangeException(nameof(selection));
//     }

//     return history[selection - 1].Result;
//   }

//   public void DeleteHistory()
//   {
//     history.Clear();
//   }

//   public void DeleteBasicHistory()
//   {
//     for (int i = history.Count - 1; i >= 0; i--)
//     {
//       if (history[i].Type == "Basic")
//       {
//         history.RemoveAt(i);
//       }
//     }
//   }

//   public void DeleteScientificHistory()
//   {
//     for (int i = history.Count - 1; i >= 0; i--)
//     {
//       if (history[i].Type == "Scientific")
//       {
//         history.RemoveAt(i);
//       }
//     }
//   }

//   public double SelectPreviousResult()
//   {
//     ShowAllHistory();

//     Console.WriteLine("Select a calculation:");

//     int selection;

//     while (!int.TryParse(Console.ReadLine(), out selection) || selection < 1 || selection > history.Count)
//     {
//       Console.WriteLine("Please select a valid number:");
//     }

//     return GetResult(selection);
//   }

// }

// public class CalculationRecord
// {
//   public string Expression { get; }
//   public double Result { get; }
//   public string Type { get; }

//   public CalculationRecord(string expression, double result, string type)
//   {
//     Expression = expression;
//     Result = result;
//     Type = type;
//   }
// }

// public static class PreviousResult
// {
//   public static double? TryUse(History history, string type)
//   {
//     if (!history.HasHistoryOfType(type))
//     {
//       return null;
//     }

//     Console.WriteLine("Do you want to use a previous result? (y/n)");
//     string? answer = Console.ReadLine();
//     return answer?.Equals("y", StringComparison.OrdinalIgnoreCase) == true
//       ? history.SelectPreviousResult()
//       : null;
//   }
// }

// public static class NumberInput
// {
//   public static double ReadDouble(string prompt)
//   {
//     Console.Write(prompt);
//     double value;
//     while (!double.TryParse(Console.ReadLine(), out value))
//     {
//       Console.WriteLine("Please enter a valid number.");
//     }

//     return value;
//   }
// }