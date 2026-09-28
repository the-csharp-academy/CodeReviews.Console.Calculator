using CalculatorLibrary.Enums;

namespace CalculatorLibrary
{
    public static class CalculatorMath
    {
        public static bool TryDivide(double a, double b, out double result)
        {
            if (b == 0)
            {
                Console.WriteLine("Cannot divide by zero.");
                result = default;
                return false;
            }
            result = a / b;
            return true;
        }

        public static bool TryCalculateSquareRoot(double radicand, out double result)
        {
            if (radicand < 0)
            {
                Console.WriteLine("Cannot calculate square root of a negative number.");
                result = default;
                return false;
            }
            result = Math.Round(Math.Sqrt(radicand), 2, MidpointRounding.AwayFromZero);
            return true;
        }

        public static bool TryCalculateExponent(double baseNumber, double exponent, out double result)
        {
            result = Math.Pow(baseNumber, exponent);

            if (double.IsNaN(result) || double.IsInfinity(result))
            {
                Console.WriteLine("The exponent is very big, try a smaller number");
                result = double.NaN;
                return false;
            }

            return true;
        }

        public static bool TryCalculateTrigonometry(double degrees, TrigonometryFunctions function, out double result)
        {
            if (degrees > 360 || degrees < -360)
            {
                Console.WriteLine("The degree value is outside of this scope, try a smaller number");
                result = double.NaN;
                return false;
            }

            var radians = degrees * Math.PI / 180.0;

            result = function switch
            {
                TrigonometryFunctions.Sin => Math.Round(Math.Sin(radians), 4),
                TrigonometryFunctions.Cos => Math.Round(Math.Cos(radians), 4),
                TrigonometryFunctions.Tan => Math.Round(Math.Tan(radians), 4),
                _ => double.NaN
            };

            return !double.IsNaN(result);
        }
    }
}
