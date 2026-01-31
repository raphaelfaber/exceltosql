using ExcelToSql.Models;

namespace ExcelToSql.Tests.Models;

public class TableTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithEmptyRows()
    {
        // Arrange & Act
        var table = new Table();

        // Assert
        Assert.NotNull(table.Rows);
        Assert.Empty(table.Rows);
    }

    [Fact]
    public void Name_ShouldSetAndGetValue()
    {
        // Arrange
        var table = new Table();
        var expectedName = "TestTable";

        // Act
        table.Name = expectedName;

        // Assert
        Assert.Equal(expectedName, table.Name);
    }

    [Fact]
    public void Rows_ShouldAddRow()
    {
        // Arrange
        var table = new Table();
        var row = new Row();

        // Act
        table.Rows.Add(row);

        // Assert
        Assert.Single(table.Rows);
        Assert.Equal(row, table.Rows[0]);
    }

    [Fact]
    public void Rows_ShouldAddMultipleRows()
    {
        // Arrange
        var table = new Table();
        var rows = new List<Row> { new Row(), new Row(), new Row() };

        // Act
        foreach (var row in rows)
        {
            table.Rows.Add(row);
        }

        // Assert
        Assert.Equal(3, table.Rows.Count);
    }
}
