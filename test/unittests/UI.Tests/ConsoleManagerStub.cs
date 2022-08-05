namespace UI.Tests;

public class ConsoleManagerStub : IConsoleManager
{
    public readonly List<ConsoleLine> ConsoleLines = new();
    
    public void WriteLine(string value)
    {
        ConsoleLines.Add(new ConsoleLine{Line = value});
    }

    public void WriteLine(string value, ConsoleColor color)
    {
        ConsoleLines.Add(new ConsoleLine{Line = value, ConsoleColor = color});
    }
}