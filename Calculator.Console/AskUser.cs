namespace Calculator.Console;
using Console = System.Console;

public static class AskUser
{
    public static T Ask<T>(string[] optionTexts) where T : IParsable<T>
    {
        bool isValidOption = true;
        T? result;
        do
        {
            if (!isValidOption)
            {
                Console.WriteLine("Choose a correct option");
            }

            foreach (var optionText in optionTexts)
            {
                Console.WriteLine(optionText);
            }
            
            isValidOption = T.TryParse(Console.ReadLine(), null, out result);
        } while (!isValidOption);

        // isValidOption is true when result get correctly parsed so != null
        return result!;
    }

}