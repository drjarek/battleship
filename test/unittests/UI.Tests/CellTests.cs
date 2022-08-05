using FluentAssertions;
using Xunit;

namespace UI.Tests;

public class CellTests
{
    [Theory]
    [InlineData(0,0,"0,0")]
    [InlineData(100,100,"100,100")]
    [InlineData(1,99,"1,99")]
    [InlineData(88,2,"88,2")]
    public void Should_return_valid_index(int row, int column, string expectedIndex)
    {
        var cell = new Cell
        {
            Row = row,
            Column = column
        };
        var actualIndex = cell.Index;
        actualIndex.Should().Be(expectedIndex);
    }
}