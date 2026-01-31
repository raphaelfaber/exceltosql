using ExcelToSql.Extractors;
using ExcelToSql.Models;

string filePath = "/home/raphaelfaber/Documentos/Respositories/exceltosql/ExcelToSql/sample.xlsx";
TableExtractor extractor = TableExtractorFactory.CreateExtractor(filePath);
Table table = extractor.ExtractTable(1);