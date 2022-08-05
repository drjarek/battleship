using FluentAssertions;
using GameEngine.Domain.Repository.Model;
using GameEngine.Domain.Repository.Model.Extensions;
using Xunit;

namespace GameEngine.Tests.Domain.Repository.Model.Extensions
{
    public class BattleshipExtensionsTests
    {
        [Fact]
        public void Should_return_correct_size()
        {
            var battleship = new Battleship
            {
                Start = new Coordinates
                {
                    Row = 1,
                    Column = 0
                },
                End = new Coordinates
                {
                    Row = 1,
                    Column = 1
                }
            };

            battleship.Size().Should().Be(2);
        }
    }
}

