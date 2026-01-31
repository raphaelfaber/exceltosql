using ExcelToSql.Models;

namespace ExcelToSql.Tests.Models;

public class RowTests
{
    [Fact]
    public void Constructor_ShouldInitializeWithEmptyColumns()
    {
        // Arrange & Act
        var row = new Row();

        // Assert
        Assert.NotNull(row.Columns);
        Assert.Empty(row.Columns);
    }


    [Fact]
    public void Columns_ShouldAddColumn()
    {
        // Arrange
        var row = new Row();
        var column = new Column { Type = "Int", Value = "123" };

        // Act
        row.Columns.Add(column);

        // Assert
        Assert.Single(row.Columns);
        Assert.Equal(column, row.Columns[0]);
    }

    [Fact]
    public void Columns_ShouldAddMultipleColumns()
    {
        // Arrange
        var row = new Row();
        var columns = new List<Column>
        {
            new Column { Type = "Int", Value = "123" },
            new Column { Type = "VarChar", Value = "Test" },
            new Column { Type = "DateTime", Value = "2024-01-31" }
        };

        // Act
        foreach (var col in columns)
        {
            row.Columns.Add(col);
        }

        // Assert
        Assert.Equal(3, row.Columns.Count);
    }

}
