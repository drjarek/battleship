namespace UI;

public interface IWriter
{
    public void WriteLine(string value);

    public void WriteLine(string value, ConsoleColor color);
}