namespace UI.Console
{
    public static class CellIndexGenerator
    {
        public static string Generate(int row, int column)
        {
            return $"{row},{column}";
        }
    }
}

