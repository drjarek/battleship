namespace UI;

public class Cell
{
    public int Column { get; init; }

    public int Row { get; init; }

    public string Value { get; set; } = string.Empty;

    public string Index => CellIndexGenerator.Generate(Row, Column);
}