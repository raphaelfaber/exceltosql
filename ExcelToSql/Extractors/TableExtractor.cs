using ExcelToSql.Models;

namespace ExcelToSql.Extractors;
public abstract class TableExtractor
{
    protected string Path { get; set; }

    public TableExtractor(string path)
    {
        Path = path;
    }

    public abstract Table ExtractTable(int worksheetIndex = 1);

    public abstract SqlType ConvertToSqlType(string dataType);
}
