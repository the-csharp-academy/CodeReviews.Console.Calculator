namespace CalculatorLibrary
{
    public interface IOperation
    {
        public abstract string Name { get; }
        public abstract double Execute(double leftNumber, double rightNumber);
    }
    public class Add : IOperation
    {
        public string Name => "Addition";
        public double Execute(double leftNumber, double rightNumber)
        {
            return leftNumber + rightNumber;
        }
    }
    public class Subtract : IOperation
    {
        public string Name => "Subtract";
        public double Execute(double leftNumber, double rightNumber)
        {
            return leftNumber - rightNumber;
        }
    }
    public class Multiply : IOperation
    {
        public string Name => "Multiply";
        public double Execute(double leftNumber, double rightNumber)
        {
            return leftNumber * rightNumber;
        }
    }
    public class Divide : IOperation
    {
        public string Name => "Divide";
        public double Execute(double leftNumber, double rightNumber)
        {
            if (rightNumber != 0) return leftNumber / rightNumber;
            else
            {
                Console.WriteLine("Can't divide by 0, returning original number");
                return leftNumber;
            }
        }
    }
}
