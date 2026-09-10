using System.Text.RegularExpressions;
using CalculatorLibrary;

namespace CalculatorApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool endApp = false;

            Console.WriteLine("Console Calculator in C#");
            Console.WriteLine("------------------------\n");

            Calculator calculator = new();
            string startingNum = "";

            while (!endApp)
            {
                bool calculationsOngoing = true;
                while (calculationsOngoing)
                {
                    AskCalculationInput(calculator, startingNum);
                    startingNum = "";

                    if (!AskYesOrNo("Do another calculation? (y/n)"))
                    {
                        calculationsOngoing = false;
                    }
                }

                if (!calculator.calculationsList.Any())
                {
                    Console.WriteLine("There are no calculations to view!");
                }

                else
                {
                    if (AskYesOrNo("Would you like to view the previous calculations (y/n)"))
                    {
                        viewCalculations(calculator);

                        bool deleteCalculations = AskYesOrNo("Would you like to delete these calculations? (y/n)");

                        if (deleteCalculations == false)
                        {
                            if (AskYesOrNo("Would you like to use a previous result in another calculation? (y/n)"))
                            {
                                if (calculator.calculationsList.Any())
                                {
                                    startingNum = UseCalculation(calculator);
                                    
                                }
                                else
                                {
                                    Console.WriteLine("THere are no previous calculations available!");
                                }
                                calculationsOngoing = true;
                                continue;
                            }
                        }
                        else
                        {
                            calculator.DeleteCalculations();
                            calculationsOngoing = true;
                        }

                    }
                }
                
                Console.WriteLine("------------------------\n");

                Console.Write("Press 'n' and Enter to close the app, or press any other key and Enter to continue: ");
                if (Console.ReadLine() == "n") endApp = true;

                Console.WriteLine("\n");
                
            }

            calculator.Finish();
            return;
        }

        public static bool AskYesOrNo(string prompt)
        {
            Console.WriteLine(prompt);
            string? userInput = Console.ReadLine();

            if (userInput?.ToLower() == "y") return true;
            else return false;
            
        }

        public static void AskCalculationInput(Calculator calculatorInstance, string firstNum = "")
        {
            double result;
            double cleanNum;
            double cleanNum2 = 0;
            string secondNum = "";
            string op;
            if (firstNum == "")
            {
                Console.Write("Type a number, and then press Enter: ");
                firstNum = Console.ReadLine(); 
            }

            while (!double.TryParse(firstNum, out cleanNum))
            {
                Console.Write("This is not valid input. Please enter a numeric value: ");
                firstNum = Console.ReadLine();
            }

            Console.WriteLine("Choose an operator from the following list:");
            Console.WriteLine("\ta - Add");
            Console.WriteLine("\ts - Subtract");
            Console.WriteLine("\tm - Multiply");
            Console.WriteLine("\td - Divide");
            Console.WriteLine("\tsr - Square Root");
            Console.WriteLine("\tp - To The Power");
            Console.WriteLine("\tx10 - Times Ten");
            Console.Write("Your option? ");

            op = Console.ReadLine();

            while (op == null || !Regex.IsMatch(op, "^(a|s|m|d|sr|p|x10)$"))
            {
                Console.WriteLine("Error: Unrecognised input. Please enter a valid operator!");
                op = Console.ReadLine();
            }

            if (op != "sr" && op != "x10")
            {
                Console.Write("Type another number, and then press Enter: ");
                secondNum = Console.ReadLine();

                while (!double.TryParse(secondNum, out cleanNum2))
                {
                    Console.Write("This is not valid input. Please enter a numeric value: ");
                    secondNum = Console.ReadLine();
                }
            }   

            try
            {
                if (op == "sr")
                {
                    result = GetSquareRoot(cleanNum);
                    secondNum = "0";
                }

                else if (op == "x10")
                {
                    result = calculatorInstance.DoOperation(cleanNum, cleanNum, op);
                    secondNum = "10";
                }

                else
                {
                    result = calculatorInstance.DoOperation(cleanNum, cleanNum2, op);
                }

                
                if (double.IsNaN(result))
                {
                    Console.WriteLine("This operation will result in a mathematical error.\n");
                }
                else
                {
                    Console.WriteLine("Your result is : {0:0.##}\n", result);
                    calculatorInstance.storeCalculations(firstNum, secondNum, op, result);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Oh no! An exception occured trying to do the math.\n - Details: " + e.Message);
            }
        }

        public static void viewCalculations(Calculator calculator)
        {
            Console.WriteLine("-----Calculations-----");
            int displayNumber = 1;
            foreach (string calculation in calculator.calculationsList)
            {
                Console.WriteLine($"{displayNumber}: {calculation}");
                displayNumber++;
            }

            Console.WriteLine("Press Enter to continue.");
            Console.ReadLine();
        }

        public static string UseCalculation(Calculator calculator)
        {
            Console.WriteLine("Which result would you like to use?");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int indexOfCalculation) && indexOfCalculation > 0 && indexOfCalculation <= calculator.calculationsList.Count)
                {
                    indexOfCalculation--;

                    string resultString = calculator.calculationsList[indexOfCalculation];

                    resultString = resultString.Split('=').Last().Trim();

                    Console.WriteLine($"First number is: {resultString}");

                    return resultString;
                }
                else
                {
                    Console.WriteLine("Please enter a valid number!");
                }
            }
        }

        public static double GetSquareRoot(double number1)
        {
            return Math.Sqrt(number1);

        }
    }
}
