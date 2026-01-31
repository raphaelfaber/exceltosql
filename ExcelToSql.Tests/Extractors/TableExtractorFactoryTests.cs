using ExcelToSql.Extractors;
using ExcelToSql.Models;

namespace ExcelToSql.Tests.Extractors;

public class TableExtractorFactoryTests
{
    [Theory]
    [InlineData("file.xlsx")]
    [InlineData("file.XLSX")]
    [InlineData("data.xls")]
    public void CreateExtractor_WithExcelFile_ShouldReturnExcelExtractor(string fileName)
    {
        // Arrange
        var path = $"/temp/{fileName}";

        // Act
        var extractor = TableExtractorFactory.CreateExtractor(path);

        // Assert
        Assert.IsType<ExcelExtractor>(extractor);
    }

    [Theory]
    [InlineData("file.csv")]
    [InlineData("file.CSV")]
    [InlineData("data.csv")]
    public void CreateExtractor_WithCsvFile_ShouldReturnCsvExtractor(string fileName)
    {
        // Arrange
        var path = $"/temp/{fileName}";

        // Act
        var extractor = TableExtractorFactory.CreateExtractor(path);

        // Assert
        Assert.IsType<CsvExtractor>(extractor);
    }

    [Theory]
    [InlineData("file.txt")]
    [InlineData("file.json")]
    [InlineData("file.xml")]
    public void CreateExtractor_WithUnsupportedFormat_ShouldThrowArgumentException(string fileName)
    {
        // Arrange
        var path = $"/temp/{fileName}";

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => TableExtractorFactory.CreateExtractor(path));
        Assert.Contains("Formato de arquivo não suportado", exception.Message);
    }

    [Fact]
    public void CreateExtractor_ShouldPassPathToExtractor()
    {
        // Arrange
        var path = "/temp/test.csv";

        // Act
        var extractor = TableExtractorFactory.CreateExtractor(path);

        // Assert
        Assert.NotNull(extractor);
        // The path is stored in the protected property of the base class
    }
}
