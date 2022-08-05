namespace UI;

public class ConsoleManager : IConsoleManager
{
    public void WriteLine(string value)
    {
        Console.WriteLine(value);
    }

    public void WriteLine(string value, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        WriteLine(value);
        Console.ResetColor();
    }
}