using CalculatorLibrary.Models;

namespace CalculatorLibrary.Services;

internal class CalculatorService
{
    internal bool TryCalculate(ParsedExpression expression, AngleMode angleMode, out double result, out string? error)
    {
        result = default;
        error = null;

        double secondOperand;
        if ((expression.Operation == Operation.Addition ||
            expression.Operation == Operation.Subtraction ||
            expression.Operation == Operation.Multiplication ||
            expression.Operation == Operation.Division ||
            expression.Operation == Operation.Exponentiation) &&
            expression.SecondOperand is null)
        {
            error = "Invalid second operand for binary expression.";
            return false;
        }
        else
        {
            secondOperand = expression.SecondOperand.GetValueOrDefault();
        }

        if (expression.Operation == Operation.Division && expression.SecondOperand == 0)
        {
            error = "Cannot divide by zero.";
            return false;
        }

        if (expression.Operation == Operation.SquareRoot && expression.FirstOperand < 0)
        {
            error = "Cannot get square root of negative numbers.";
            return false;
        }

        double trigonometricalOperand = expression.FirstOperand;
        if ((expression.Operation == Operation.Sine ||
            expression.Operation == Operation.Cosine ||
            expression.Operation == Operation.Tangent) &&
            angleMode != AngleMode.Radians)
        {
             trigonometricalOperand = expression.FirstOperand * (Math.PI / 180);
        }

        result = expression.Operation switch
        {
            Operation.Addition => expression.FirstOperand + secondOperand,
            Operation.Subtraction => expression.FirstOperand - secondOperand,
            Operation.Multiplication => expression.FirstOperand * secondOperand,
            Operation.Division => expression.FirstOperand / secondOperand,
            Operation.Exponentiation => Math.Pow(expression.FirstOperand, secondOperand),
            Operation.SquareRoot => Math.Sqrt(expression.FirstOperand),
            Operation.Sine => Math.Sin(trigonometricalOperand),
            Operation.Cosine => Math.Cos(trigonometricalOperand),
            Operation.Tangent => Math.Tan(trigonometricalOperand),
            _ => expression.FirstOperand
        };

        if (!double.IsFinite(result))
        {
            error = "Operation results in invalid value.";
            return false;
        }

        result = Math.Round(result, 9, MidpointRounding.AwayFromZero);
        return true;
    }
}