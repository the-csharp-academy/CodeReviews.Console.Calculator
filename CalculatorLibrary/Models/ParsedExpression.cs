namespace CalculatorLibrary.Models;

internal record ParsedExpression
(
    Operation? Operation,
    double FirstOperand,
    double? SecondOperand,
    string NormalizedExpression
);
