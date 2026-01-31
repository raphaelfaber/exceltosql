namespace ExcelToSql.Extractors;

public class TableExtractorFactory
{
    public static TableExtractor CreateExtractor(string path)
    {
        string extension = System.IO.Path.GetExtension(path).ToLower();

        return extension switch
        {
            ".xlsx" => new ExcelExtractor(path),
            ".xls" => new ExcelExtractor(path),
            ".csv" => new CsvExtractor(path),
            _ => throw new ArgumentException($"Formato de arquivo não suportado: {extension}")
        };
    }
}
