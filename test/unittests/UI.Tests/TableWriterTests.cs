using FluentAssertions;
using Xunit;

namespace UI.Tests;

public class TableWriterTests
{
    [Fact]
    public void Should_draw_valid_table_for_2_rows_and_2_columns()
    {
        var expectedLines = new List<ConsoleLine>
        {
            new()
            {
                Line = "|    | A  |",
                ConsoleColor = ConsoleColor.Black
            },
            new()
            {
                Line = "| 1  |    |",
                ConsoleColor = ConsoleColor.Black
            }
        };

        var table = new Table(2, 2);
        table.SetValue(0,1, "A");
        table.SetValue(1,0, "1");

        var consoleManagerStub = new ConsoleManagerStub();
        var tableWriter = new TableWriter(consoleManagerStub);
        tableWriter.Write(table);

        var actualLines = consoleManagerStub.ConsoleLines;

        actualLines.Should().BeEquivalentTo(expectedLines);
    }
    
    [Fact]
    public void Should_draw_valid_table_for_2_rows_and_3_columns()
    {
        var expectedLines = new List<ConsoleLine>
        {
            new()
            {
                Line = "|    | A  | B  |",
                ConsoleColor = ConsoleColor.Black
            },
            new()
            {
                Line = "| 1  |    |    |",
                ConsoleColor = ConsoleColor.Black
            }
        };

        var table = new Table(2, 3);
        table.SetValue(0,1, "A");
        table.SetValue(0,2, "B");
        table.SetValue(1,0, "1");

        var consoleManagerStub = new ConsoleManagerStub();
        var tableWriter = new TableWriter(consoleManagerStub);
        tableWriter.Write(table);

        var actualLines = consoleManagerStub.ConsoleLines;

        actualLines.Should().BeEquivalentTo(expectedLines);
    }
    
    [Fact]
    public void Should_draw_valid_table_for_3_rows_and_2_columns()
    {
        var expectedLines = new List<ConsoleLine>
        {
            new()
            {
                Line = "|    | A  |",
                ConsoleColor = ConsoleColor.Black
            },
            new()
            {
                Line = "| 1  |    |",
                ConsoleColor = ConsoleColor.Black
            },
            new()
            {
                Line = "| 2  |    |",
                ConsoleColor = ConsoleColor.Black
            }
        };

        var table = new Table(3, 2);
        table.SetValue(0,1, "A");
        table.SetValue(1,0, "1");
        table.SetValue(2,0, "2");

        var consoleManagerStub = new ConsoleManagerStub();
        var tableWriter = new TableWriter(consoleManagerStub);
        tableWriter.Write(table);

        var actualLines = consoleManagerStub.ConsoleLines;

        actualLines.Should().BeEquivalentTo(expectedLines);
    }
}