using FluentAssertions;
using Xunit;

namespace UI.Console.Tests
{
    public class CellIndexGeneratorTests
    {
        [Theory]
        [InlineData(0,0,"0,0")]
        [InlineData(1000,1000,"1000,1000")]
        [InlineData(10,99,"10,99")]
        [InlineData(88,20,"88,20")]
        public void Should_generate_valid_index(int row, int column, string expectedIndex)
        {
            var actualIndex = CellIndexGenerator.Generate(row, column);

            actualIndex.Should().Be(expectedIndex);
        }
    }
}

