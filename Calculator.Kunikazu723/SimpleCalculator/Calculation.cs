using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCalculator
{
    internal class Calculation
    {
        public double Operand1 { get; init; }
        public double Operand2 { get; init; }
        public double Result { get; init; }
        public string Operation { get; init; } = string.Empty;

    }
}
