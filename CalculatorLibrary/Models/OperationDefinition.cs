namespace CalculatorLibrary.Models;

internal record OperationDefinition(Operation Operation, string Symbol);

internal static class SupportedOperations
{
    internal static IReadOnlyList<OperationDefinition> All { get; } =
    [
        new(Operation.Addition, "+"),
        new(Operation.Subtraction, "-"),
        new(Operation.Multiplication, "*"),
        new(Operation.Division, "/"),
        new(Operation.Exponentiation, "^"),
        new(Operation.SquareRoot, "sqrt"),
        new(Operation.Sine, "sin"),
        new(Operation.Cosine, "cos"),
        new(Operation.Tangent, "tan")
    ];
}