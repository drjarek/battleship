namespace UI.Console
{
    public class TableWriter : ITableWriter
    {
        private readonly IConsoleManager _consoleManager;

        public TableWriter(IConsoleManager consoleManager)
        {
            _consoleManager = consoleManager;
        }
        
        public void Write(Table table)
        {
            var cells = table.Cells;
            for (var rowIndex = -1; rowIndex < table.NumberOfRows; rowIndex++)
            {
                for (var columnIndex = -1; columnIndex < table.NumberOfColumns; columnIndex++)
                {
                    var cell = cells[CellIndexGenerator.Generate(rowIndex, columnIndex)];
                    var tableCell = "";
                    if (columnIndex == -1)
                    {
                        tableCell = "|";
                    }
                    
                    _consoleManager.Write(tableCell);
                    _consoleManager.Write(" ");

                    _consoleManager.Write(cell.Value.PadRight(2), cell.Color);
                    _consoleManager.Write(" |");
                }
                
                _consoleManager.WriteLine("");
                
            }
        }
    }
}

