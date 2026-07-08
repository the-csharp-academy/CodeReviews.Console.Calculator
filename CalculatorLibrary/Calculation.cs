namespace CalculatorLibrary
{
    internal class Calculation
    {
        public int CalculationId { get; set; }
        public OperationType Type { get; set; }
        public double Operand1 { get; set; }
        public double Operand2 { get; set; }
        public double Result { get; set; }
    }

    internal enum OperationType
    {
        Addition,
        Subtraction,
        Multiplication,
        Division,
        Square_root,
        X10,
        Taking_the_power,
        Sine,
        Cosine,
        Tangent,
        Arcsine,
        Arccosine,
        Arctangent
    }
}
