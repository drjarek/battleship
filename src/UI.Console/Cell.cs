using System;

namespace UI.Console
{
    public class Cell
    {
        public int Column { get; init; }

        public int Row { get; init; }

        public string Value { get; set; } = string.Empty;

        public Colors Color { get; set; } = Colors.Default;

        public string Index => CellIndexGenerator.Generate(Row, Column);
    }
}

