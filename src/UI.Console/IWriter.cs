namespace UI.Console
{
    public interface IWriter
    {
        public void WriteLine(string? value);

        public void Write(string value, Colors color = Colors.Default);

        public string? ReadLine();

        public void ClearConsole();
    }
}

