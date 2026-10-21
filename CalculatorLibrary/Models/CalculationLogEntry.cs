namespace CalculatorLibrary.Models;

internal record CalculationLogEntry
(
    DateTime TimeStamp,
    string Input,
    string? Expression,
    double? Result,
    string? Error
);
