using ExcelToSql.Extractors;
using ExcelToSql.Models;

namespace ExcelToSql.Tests.Extractors;

public class SampleExcelTests
{
    [Fact]
    public void SampleXlsx_IsUsersTable()
    {
        var path = "samples/sample.xlsx";
        var extractor = TableExtractorFactory.CreateExtractor(path);
        var table = extractor.ExtractTable(1);

        Assert.NotNull(table);
        Assert.NotNull(table.Header);
        var headers = table.Header!.Columns.Select(c => (c.Value ?? string.Empty).Trim()).ToArray();
        Assert.Equal(new[] { "Nome", "Idade", "Peso", "Altura" }, headers);

        Assert.True(table.Rows.Count >= 3, "expected at least 3 data rows");

        var first = table.Rows[0].Columns.Select(c => c.Value).ToArray();
        Assert.Equal("Ana", first[0]);
        Assert.Equal("29", first[1]);
        Assert.Equal("62.5", first[2]);
        Assert.Equal("1.68", first[3]);
    }
}