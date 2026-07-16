using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorLibrary
{
    internal class CalculationsHistory
    {
        private double FirstNumber { get; set; }
        private double? SecondNumber { get; set; } = null;
        private string? Operation { get; set; }
        private double Result { get; set; }
        public CalculationsHistory(double firstNum, double secondNum, string op, double res)
        {
            FirstNumber = firstNum; 
            SecondNumber = secondNum; 
            Operation = op; 
            Result = res;
        }
        public CalculationsHistory(double firstNum, string op, double res)
        {
            FirstNumber = firstNum;
            Operation = op;
            Result = res;
        }

        public void Display()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (SecondNumber != null)
            {
                Console.WriteLine($"First number: {FirstNumber}, second number:{SecondNumber}\n" +
                    $"Operation: {Operation}\nResult:{Result}");
            }
            else Console.WriteLine($"Number: {FirstNumber}\n" +
                    $"Operation: {Operation}\nResult:{Result}");
            Console.ResetColor();
        }
    }
}
