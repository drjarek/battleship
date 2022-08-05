using System.Collections.Generic;

namespace UI.Console
{
    public class Table : ITable
    {
        public int NumberOfColumns { get; }
        public int NumberOfRows { get; }
    
        public IDictionary<string, Cell> Cells { get; }

        public Table(int numberOfRows, int numberOfColumns)
        {
            NumberOfColumns = numberOfColumns;
            NumberOfRows = numberOfRows;
            Cells = new Dictionary<string, Cell>();
        
            GenerateCells();
        }

        public void SetValue(int row, int column, string value)
        {
            SetValue(row, column, value, Colors.Default);
        }

        public void SetValue(int row, int column, string value, Colors color)
        {
            var cell = Cells[CellIndexGenerator.Generate(row, column)];
            cell.Value = value;
            cell.Color = color;
        }

        public bool IsCellEmpty(int row, int column)
        {
            if (!Cells.ContainsKey(CellIndexGenerator.Generate(row, column))) return false;
        
            var cell = GetCell(row, column);
            return string.IsNullOrWhiteSpace(cell.Value);

        }

        private Cell GetCell(int row, int column)
        {
            return Cells[CellIndexGenerator.Generate(row, column)];
        }

        private void GenerateCells()
        {
            for (var row = -1; row < NumberOfRows; row++)
            {
                for (var column = -1; column < NumberOfColumns; column++)
                {
                    var cell = new Cell
                    {
                        Row = row,
                        Column = column
                    };
                
                    Cells.Add(cell.Index, cell);
                }
            }
        }
    }
}

