namespace Validation;

public static class ValidatorService
{
    public static double GetUserInputForDouble(string numberCountText)
    {
        Console.Write($"Type the {numberCountText} number, and then press Enter: ");
        string? numInput = Console.ReadLine();

        return GetValidDoubleInputValue(numInput);
    }

    public static double GetValidDoubleInputValue(string? input)
    {
        double cleanNum = 0.0;

        while (!double.TryParse(input, out cleanNum))
        {
            Console.Write("This is not valid input. Please enter a numeric value: ");
            input = Console.ReadLine();
        }

        return cleanNum;
    }


    public static int GetValidIntInputValue(string? input)
    {
        int cleanNum = 0;

        while (!int.TryParse(input, out cleanNum))
        {
            Console.Write("\nThis is not valid input. Please enter a numeric integer value: ");
            input = Console.ReadLine();
        }

        return cleanNum;
    }

}