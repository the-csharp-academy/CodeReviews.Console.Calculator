using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace CodeReviews.Console.Calculator;

public enum Operand
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
    SquareRoot,
    Power,
    Cos,
    Sin
}

public class Operator : IParsable<Operator>
{
    private readonly Operand? _operand;
    
    public Operator(string? input)
    {
        _operand = input?.ToUpper().Trim() switch
        {
            "+" => Operand.Addition,
            "-" => Operand.Subtraction,
            "*" => Operand.Multiplication,
            "/" => Operand.Division,
            "SQRT" => Operand.SquareRoot,
            "^" => Operand.Power,
            "COS" => Operand.Cos,
            "SIN" => Operand.Sin,
            _ => null
        };
    }

    public double Result(double a, double b) => _operand switch
    {
        Operand.Addition => a + b,
        Operand.Subtraction => a! - b,
        Operand.Multiplication => a * b,
        Operand.Division => b != 0 ? a / b : throw new DivideByZeroException("Can't divide by zero"),
        Operand.SquareRoot => b >= 0 ? Math.Sqrt(b) : throw new Exception("Square root of negative number isn't a real number"),
        Operand.Power => (b % 0 ==0 || a >= 0) ? Math.Pow(a, b) : throw new Exception("Power of negative number with fractional exponent isn't a real number"),
        Operand.Cos => Math.Cos(b),
        Operand.Sin => Math.Sin(b),
        _ => throw new InvalidEnumArgumentException("Operand not recognized")
    };

    public string GetSymbol() => _operand switch
    {
        Operand.Addition => "+",
        Operand.Subtraction => "-",
        Operand.Multiplication => "*",
        Operand.Division => "/",
        Operand.SquareRoot => "SQRT",
        Operand.Power => "^",
        Operand.Cos => "cos",
        Operand.Sin => "sin",
        _ => throw new InvalidEnumArgumentException("Operand not recognized")
    };

    public int AmountInput()
    {
        switch (_operand)
        {
            case Operand.SquareRoot:
            case Operand.Cos:
            case Operand.Sin:
                return 1;
            default:
                return 2;
        }
    }

    public static Operator Parse(string s, IFormatProvider? provider)
    {
        return TryParse(s, provider, out var result) ? result : throw new FormatException("Not a valid input Operator");
    }

    public static bool TryParse([NotNullWhen(true)] string? s, IFormatProvider? provider, [MaybeNullWhen(false)] out Operator result)
    {
        var op = new Operator(s);

        if (op._operand == null)
        {
            result = null;
            return false;
        }

        result = op;
        return true;
    }
}
