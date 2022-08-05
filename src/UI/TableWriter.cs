namespace UI;

public class TableWriter
{
    private readonly IConsoleManager _consoleManager;

    public TableWriter(IConsoleManager consoleManager)
    {
        _consoleManager = consoleManager;
    }

    public void Write(Table table)
    {
        var cells = table.Cells;
        for (var r = 0; r < table.NumberOfRows; r++)
        {
            var row = "|";

            for (var c = 0; c < table.NumberOfColumns; c++)
            {
                row += " " + cells[CellIndexGenerator.Generate(r, c)].Value.PadRight(2) + " |";
            }
            _consoleManager.WriteLine(row);
        }
    }
}