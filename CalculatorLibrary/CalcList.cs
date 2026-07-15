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
                
                Console.WriteLine("Press any key.");
                
            }
            else
            {
                Console.WriteLine("Not found any records.\nPress any key.");
                
            }
        }
        public static void Clear()
        {
            if (calcHistory.Count > 0)
            {
                calcHistory.Clear();
            }
            else Console.WriteLine("Not found any records to delete.");
        }
    }
}
