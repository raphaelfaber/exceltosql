// Sample usage (kept for reference). This file is not compiled into the library.
// To run the sample, copy the code into a console app or enable compilation temporarily.

using ExcelToSql.Extractors;
using ExcelToSql.Models;

public static class SampleUsage
{
    public static ExcelToSql.Models.Table LoadSample()
    {
        string filePath = "samples/sample.xlsx";
        TableExtractor extractor = TableExtractorFactory.CreateExtractor(filePath);
        return extractor.ExtractTable(1);
    }
}