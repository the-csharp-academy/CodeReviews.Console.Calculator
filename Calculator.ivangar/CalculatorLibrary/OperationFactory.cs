using CalculatorLibrary.Enums;
using CalculatorLibrary.Models;

namespace CalculatorLibrary
{
    public static class OperationFactory
    {
        public static MathOperation CreateBasic(double a, double b, char op, double result) =>
            new MathOperation { OperandA = a, OperandB = b, Operation = op, Result = result };

        public static SquareRoot CreateSquareRoot(double radicand, char op, double result) =>
            new SquareRoot { Radicand = radicand, Operation = op, Result = result };

        public static Exponentiation CreateExponentiation(double baseNumber, double exponent, char op, double result) =>
            new Exponentiation { Base = baseNumber, Exponent = exponent, Operation = op, Result = result };

        public static TrigonometryOperation CreateTrigonometry(double degrees, TrigonometryFunctions function, char op, double result) =>
            new TrigonometryOperation { Degrees = degrees, Function = function, Operation = op, Result = result };
    }
}
