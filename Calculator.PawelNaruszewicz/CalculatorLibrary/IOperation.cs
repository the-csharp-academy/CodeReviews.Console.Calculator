namespace CalculatorLibrary
{
    public interface IOperation
    {
        public string Name { get; }
        public int RequiredArguments { get; }
        public double Execute(params double[] numbers);
    }
    public class Add : IOperation
    {
        public string Name => "Addition";
        public int RequiredArguments => 2;
        public double Execute(params double[] numbers)
        {
            return numbers[0] + numbers[1];
        }
    }
    public class Subtract : IOperation
    {
        public string Name => "Subtract";
        public int RequiredArguments => 2;
        public double Execute(params double[] numbers)
        {
            return numbers[0] - numbers[1];
        }
    }
    public class Multiply : IOperation
    {
        public string Name => "Multiply";
        public int RequiredArguments => 2;
        public double Execute(params double[] numbers)
        {
            return numbers[0] * numbers[1];
        }
    }
    public class Divide : IOperation
    {
        public string Name => "Divide";
        public int RequiredArguments => 2;
        public double Execute(params double[] numbers)
        {
            if (numbers[1] != 0) return numbers[0] / numbers[1];
            else
            {
                Console.WriteLine("Can't divide by 0, returning original number");
                return numbers[0];
            }
        }
    }
    public class SquareRoot : IOperation
    {
        public string Name => "Square Root";
        public int RequiredArguments => 1;
        public double Execute(params double[] numbers)
        {
            return Math.Sqrt(numbers[0]);
        }
    }
    public class PowerOf : IOperation
    {
        public string Name => "Power of";
        public int RequiredArguments => 2;
        public double Execute(params double[] numbers)
        {
            return Math.Pow(numbers[0], numbers[1]);
        }
    }
}
