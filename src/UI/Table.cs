namespace UI;

public class Table
{
    public int NumberOfColumns { get; }
    public int NumberOfRows { get; }
    
    public IDictionary<string, Cell> Cells { get; }

    public Table(int numberOfRows, int numberOfColumns)
    {
        this.NumberOfColumns = numberOfColumns;
        NumberOfRows = numberOfRows;
        Cells = new Dictionary<string, Cell>();
        
        GenerateCells();
    }

    public void SetValue(int row, int column, string value)
    {
        Cells[CellIndexGenerator.Generate(row, column)].Value = value;
    }

    private void GenerateCells()
    {
        for (var r = 0; r < NumberOfRows; r++)
        {
            for (var c = 0; c < NumberOfColumns; c++)
            {
                var cell = new Cell
                {
                    Row = r,
                    Column = c
                };
                
                Cells.Add(cell.Index, cell);
            }
        }
    }
}