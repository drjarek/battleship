using FluentAssertions;
using Xunit;

namespace UI.Tests;

public class TableTests
{
    [Theory]
    [InlineData(3, 2, 6)]
    [InlineData(2, 3, 6)]
    public void Should_generate_valid_number_of_cells(int rows, int columns, int expectedCells)
    {
        var table = new Table(rows, columns);
        table.Cells.Should().HaveCount(expectedCells);
    }

    [Fact]
    public void Should_generate_valid_dells_for_2_rows_and_3_columns()
    {
        var expectedCells = new Dictionary<string, Cell>
        {
            { "0,0", CreateCell(0, 0) },
            { "0,1", CreateCell(0, 1) },
            { "0,2", CreateCell(0, 2) },
            { "1,0", CreateCell(1, 0) },
            { "1,1", CreateCell(1, 1) },
            { "1,2", CreateCell(1, 2) }
        };

        var table = new Table(2, 3);
        var actualCells = table.Cells;

        actualCells.Should().BeEquivalentTo(expectedCells);
    }
    
    [Fact]
    public void Should_generate_valid_dells_for_3_rows_and_2_columns()
    {
        var expectedCells = new Dictionary<string, Cell>
        {
            { "0,0", CreateCell(0, 0) },
            { "0,1", CreateCell(0, 1) },
            { "1,0", CreateCell(1,0) },
            { "1,1", CreateCell(1,1) },
            { "2,0", CreateCell(2,0) },
            { "2,1", CreateCell(2,1) }
        };

        var table = new Table(3, 2);
        var actualCells = table.Cells;

        actualCells.Should().BeEquivalentTo(expectedCells);
    }

    [Fact]
    public void Should_update_cell()
    {
        var expectedCell = new Cell
        {
            Column = 1,
            Row = 0,
            Value = "OK"
        };
        
        var table = new Table(2, 2);
        table.SetValue(0,1,"OK");

        var cells = table.Cells;
        var actualCell = cells["0,1"];

        actualCell.Should().BeEquivalentTo(expectedCell);
    }

    private static Cell CreateCell(int row, int column, string value = "")
    {
        return new Cell
        {
            Column = column,
            Row = row,
            Value = value
        };
    }
}