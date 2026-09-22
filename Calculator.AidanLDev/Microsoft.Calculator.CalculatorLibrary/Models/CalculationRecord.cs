using Microsoft.Calculator.CalculatorLibrary.Enums;

namespace Microsoft.Calculator.CalculatorLibrary.Models;

public record CalculationRecord(double Num1, double Num2, OperationType Operation, double Result);