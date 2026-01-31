using ExcelToSql.Models;

namespace ExcelToSql.Tests.Models;

public class ColumnTests
{
    [Fact]
    public void Type_ShouldSetAndGetValue()
    {
        // Arrange
        var column = new Column();
        var expectedType = "VarChar";

        // Act
        column.Type = expectedType;

        // Assert
        Assert.Equal(expectedType, column.Type);
    }

    [Fact]
    public void Value_ShouldSetAndGetValue()
    {
        // Arrange
        var column = new Column();
        var expectedValue = "TestValue";

        // Act
        column.Value = expectedValue;

        // Assert
        Assert.Equal(expectedValue, column.Value);
    }

    [Fact]
    public void Column_ShouldInitializeWithProperties()
    {
        // Arrange & Act
        var column = new Column { Type = "Int", Value = "42" };

        // Assert
        Assert.Equal("Int", column.Type);
        Assert.Equal("42", column.Value);
    }

    [Theory]
    [InlineData("VarChar", "John")]
    [InlineData("Int", "123")]
    [InlineData("DateTime", "2024-01-31")]
    [InlineData("Decimal", "99.99")]
    public void Column_ShouldAcceptVariousTypes(string type, string value)
    {
        // Arrange & Act
        var column = new Column { Type = type, Value = value };

        // Assert
        Assert.Equal(type, column.Type);
        Assert.Equal(value, column.Value);
    }
}
