using ExcelToSql.Extractors;
using ExcelToSql.Models;

namespace ExcelToSql.Tests.Extractors;

public class CsvExtractorTests : IDisposable
{
    private readonly string _testDirectory;
    private readonly string _testCsvPath;

    public CsvExtractorTests()
    {
        _testDirectory = Path.Combine(Path.GetTempPath(), "ExcelToSql_Tests");
        Directory.CreateDirectory(_testDirectory);
        _testCsvPath = Path.Combine(_testDirectory, "test.csv");
    }

    [Fact]
    public void ExtractTable_WithSimpleCsvFile_ShouldReturnTable()
    {
        // Arrange
        var csvContent = "Name,Age,Salary\nJohn,30,50000\nJane,28,55000";
        File.WriteAllText(_testCsvPath, csvContent);
        var extractor = new CsvExtractor(_testCsvPath);

        // Act
        var table = extractor.ExtractTable();

        // Assert
        Assert.NotNull(table);
        Assert.Equal("test", table.Name);
        Assert.NotEmpty(table.Rows);
    }

    [Fact]
    public void ExtractTable_ShouldMarkFirstRowAsHeader()
    {
        // Arrange
        var csvContent = "Name,Age\nJohn,30";
        File.WriteAllText(_testCsvPath, csvContent);
        var extractor = new CsvExtractor(_testCsvPath);

        // Act
        var table = extractor.ExtractTable();

        // Assert
        Assert.True(table.Rows[0].IsHeader);
        Assert.False(table.Rows[1].IsHeader);
    }

    [Fact]
    public void ExtractTable_ShouldParseColumnsCorrectly()
    {
        // Arrange
        var csvContent = "Name,Age,Salary\nJohn,30,50000";
        File.WriteAllText(_testCsvPath, csvContent);
        var extractor = new CsvExtractor(_testCsvPath);

        // Act
        var table = extractor.ExtractTable();

        // Assert
        Assert.Equal(3, table.Rows[0].Columns.Count);
        Assert.Equal("Name", table.Rows[0].Columns[0].Value);
        Assert.Equal("Age", table.Rows[0].Columns[1].Value);
        Assert.Equal("Salary", table.Rows[0].Columns[2].Value);
    }

    [Fact]
    public void InferDataType_WithIntegerValue_ShouldReturnInt()
    {
        // Arrange
        var extractor = new CsvExtractor(_testCsvPath);

        // Act
        var result = extractor.ConvertToSqlType(DataTypeInferencer.TypeInt);

        // Assert
        Assert.Equal(SqlType.Int, result);
    }

    [Fact]
    public void InferDataType_WithBooleanValue_ShouldReturnBit()
    {
        // Arrange
        var extractor = new CsvExtractor(_testCsvPath);

        // Act
        var result = extractor.ConvertToSqlType(DataTypeInferencer.TypeBool);

        // Assert
        Assert.Equal(SqlType.Bit, result);
    }

    [Fact]
    public void ConvertToSqlType_WithStringType_ShouldReturnVarChar()
    {
        // Arrange
        var extractor = new CsvExtractor(_testCsvPath);

        // Act
        var result = extractor.ConvertToSqlType(DataTypeInferencer.TypeString);

        // Assert
        Assert.Equal(SqlType.VarChar, result);
    }

    [Fact]
    public void ConvertToSqlType_WithDateTimeType_ShouldReturnDateTime()
    {
        // Arrange
        var extractor = new CsvExtractor(_testCsvPath);

        // Act
        var result = extractor.ConvertToSqlType(DataTypeInferencer.TypeDateTime);

        // Assert
        Assert.Equal(SqlType.DateTime, result);
    }

    [Fact]
    public void ConvertToSqlType_WithUnknownType_ShouldReturnVarChar()
    {
        // Arrange
        var extractor = new CsvExtractor(_testCsvPath);

        // Act
        var result = extractor.ConvertToSqlType("unknown");

        // Assert
        Assert.Equal(SqlType.VarChar, result);
    }

    public void Dispose()
    {
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }
}
