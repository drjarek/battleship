namespace UI.Console
{
    public class GameBoard : Table
    {
        protected GameBoard(int numberOfRows, int numberOfColumns) : base(numberOfRows, numberOfColumns)
        {
            SetupColumns(numberOfColumns);
            SetupRows(numberOfRows);
        }

        private void SetupColumns(int numberOfColumns)
        {
            var columnIndex = 0;
            for (var header = 'A'; header < 'Z'; header++)
            {
                SetValue(-1, columnIndex, $"{header}");
            
                columnIndex++;
            
                if (columnIndex == numberOfColumns) break;
            }
        }

        private void SetupRows(int numberOfRows)
        {
            for (var rowIndex = 0; rowIndex < numberOfRows; rowIndex++)
            {
                SetValue(rowIndex, -1, $"{rowIndex+1}");
            }
        }
    }
}
