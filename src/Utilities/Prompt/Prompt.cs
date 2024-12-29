namespace SmartRide.src.Utilities.Prompt;

public class Prompt
{
    public static void PressKeyToContinue()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        Console.ResetColor();
    }
}
