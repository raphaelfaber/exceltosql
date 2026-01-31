using ExcelToSql.Extractors;
using ExcelToSql.Models;

namespace ExcelToSql.Tests.Extractors;

public class ExcelExtractorTests
{
    [Fact]
    public void ConvertToSqlType_WithStringType_ShouldReturnVarChar()
    {
        // Arrange
        var extractor = new ExcelExtractor("dummy.xlsx");

        // Act
        var result = extractor.ConvertToSqlType(DataTypeInferencer.TypeString);

        // Assert
        Assert.Equal(SqlType.VarChar, result);
    }

    [Fact]
    public void ConvertToSqlType_WithDoubleType_ShouldReturnFloat()
    {
        // Arrange
        var extractor = new ExcelExtractor("dummy.xlsx");

        // Act
        var result = extractor.ConvertToSqlType(DataTypeInferencer.TypeDouble);

        // Assert
        Assert.Equal(SqlType.Float, result);
    }

    [Fact]
    public void ConvertToSqlType_WithIntType_ShouldReturnInt()
    {
        // Arrange
        var extractor = new ExcelExtractor("dummy.xlsx");

        // Act
        var result = extractor.ConvertToSqlType(DataTypeInferencer.TypeInt);

        // Assert
        Assert.Equal(SqlType.Int, result);
    }

    [Fact]
    public void ConvertToSqlType_WithDateTimeType_ShouldReturnDateTime()
    {
        // Arrange
        var extractor = new ExcelExtractor("dummy.xlsx");

        // Act
        var result = extractor.ConvertToSqlType(DataTypeInferencer.TypeDateTime);

        // Assert
        Assert.Equal(SqlType.DateTime, result);
    }

    [Fact]
    public void ConvertToSqlType_WithBooleanType_ShouldReturnBit()
    {
        // Arrange
        var extractor = new ExcelExtractor("dummy.xlsx");

        // Act
        var result = extractor.ConvertToSqlType(DataTypeInferencer.TypeBool);

        // Assert
        Assert.Equal(SqlType.Bit, result);
    }

    [Fact]
    public void ConvertToSqlType_WithDecimalType_ShouldReturnDecimal()
    {
        // Arrange
        var extractor = new ExcelExtractor("dummy.xlsx");

        // Act
        var result = extractor.ConvertToSqlType(DataTypeInferencer.TypeDecimal);

        // Assert
        Assert.Equal(SqlType.Decimal, result);
    }

    [Theory]
    [InlineData(DataTypeInferencer.TypeLong, SqlType.BigInt)]
    [InlineData(DataTypeInferencer.TypeByte, SqlType.TinyInt)]
    [InlineData("unknown", SqlType.Unknown)]
    public void ConvertToSqlType_WithVariousTypes_ShouldReturnCorrectSqlType(string type, SqlType expected)
    {
        // Arrange
        var extractor = new ExcelExtractor("dummy.xlsx");

        // Act
        var result = extractor.ConvertToSqlType(type);

        // Assert
        Assert.Equal(expected, result);
    }
}
