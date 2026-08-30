using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCalculator.Brandonscodedata
{
    internal class CalcData
    {
        public int CalculationRound { get; init; }

        public double CalculationNumberOne { get; init; }

        public double CalculationNumberTwo { get; init; }

        public string CalculationOperator { get; init; }

        public double CalculationResult { get; init; }

        public CalcData(int round, double num1, double num2, string op, double result)
        {
            CalculationRound = round;
            CalculationNumberOne = num1;
            CalculationNumberTwo = num2;
            CalculationOperator = op;
            CalculationResult = result;
        }
    }
}
