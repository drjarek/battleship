namespace UI.Console
{
    public interface ITable
    {
        public void SetValue(int row, int column, string value);
    
        public void SetValue(int row, int column, string value, Colors color);
    }
}

