using System;
using System.Collections.Generic;
using System.Text;

namespace CalculatorLibrary
{
    static class CalcList
    {
        private static List<CalculationsHistory> calcHistory = new List<CalculationsHistory>();

        public static void Add(CalculationsHistory history)
        {
            calcHistory.Add(history);
        }
        public static void Show()
        {
            if (calcHistory.Count > 0)
            {
                Console.WriteLine("=============================================");
                foreach (CalculationsHistory history in calcHistory)
                {
                    history.Display();
                    Console.WriteLine("=============================================");
                }
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Not found any records.\n");
                Console.ResetColor();
            }
        }
        public static void Clear()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (calcHistory.Count > 0)
            {
                calcHistory.Clear();
                Console.WriteLine("All records were successfully deleted.");
            }
            else Console.WriteLine("Not found any records to delete.");
            Console.ResetColor();
        }
    }
}
