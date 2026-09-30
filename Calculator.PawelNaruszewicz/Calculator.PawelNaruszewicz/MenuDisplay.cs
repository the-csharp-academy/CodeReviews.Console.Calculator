namespace Calculator.PawelNaruszewicz
{
    internal class MenuDisplay
    {
        public char GetChoiceOfOperation()
        {
            char[] availableChoices = new char[] { 'a', 's', 'm', 'd' };

            Console.WriteLine("Choose an operator from the following list:");
            Console.WriteLine("\ta - Add");
            Console.WriteLine("\ts - Subtract");
            Console.WriteLine("\tm - Multiply");
            Console.WriteLine("\td - Divide");
            char chosenChar = GetPlayerChoice(availableChoices);
            return chosenChar;
        }
        public char DisplayMainMenu()
        {
            char[] availableChoices = new char[] { '1', '2', '3', '4', 'n' };

            Console.WriteLine("Here are the available options");
            Console.WriteLine("Press 1 to start new operation");
            Console.WriteLine("Press 2 to display the list of latest results");
            Console.WriteLine("Press 3 to delete the list of latest results");
            Console.WriteLine("Press 4 to use one of of latest results in new operation");
            Console.WriteLine("Or press 'n' to close the appliaction");

            char chosenOption = GetPlayerChoice(availableChoices);
            return chosenOption;

        }
        private char GetPlayerChoice(char[] availableChoices)
        {
            while (true)
            {
                char inputAsChar = '0';
                string input = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(input))
                {
                    inputAsChar = input[0];
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                    continue;
                }
                if (availableChoices.Contains(inputAsChar))
                {
                    return inputAsChar;
                }
                else
                {
                    Console.WriteLine("Invalid input.");
                }
            }

        }

    }
}
