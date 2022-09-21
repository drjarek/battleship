using System;

namespace UI.Console
{
    public class ConsoleManager : IConsoleManager
    {
        public void WriteLine(string? value)
        {
            System.Console.WriteLine(value);
        }

        public void Write(string value, Colors color = Colors.Default)
        {
            if (color != Colors.Default)
            {
                System.Console.ForegroundColor = GetConsoleColor(color);
            }
            
            System.Console.Write(value);
            System.Console.ResetColor();
        }

        private static ConsoleColor GetConsoleColor(Colors colors)
        {
            return colors == Colors.Green ? ConsoleColor.Green : ConsoleColor.Red;
        }

        public string? ReadLine()
        {
            return System.Console.ReadLine();
        }

        public void ClearConsole()
        {
            System.Console.Clear();
        }
    }
}

