using ClosedXML.Excel;
using ExcelToSql.Models;

namespace ExcelToSql.Extractors;

public class ExcelExtractor : TableExtractor
{
    public ExcelExtractor(string path) : base(path)
    {
    }

    public override Table ExtractTable(int worksheetIndex = 1)
    {
        Table table = new Table();

        using var workbook = new XLWorkbook(Path);
        var worksheet = workbook.Worksheet(worksheetIndex);
        table.Name = worksheet.Name;

        for (int i = 1; i <= worksheet.Rows().Count(); i++)
        {
            var row = worksheet.Row(i);
            
            Row rowObj = new Row();
            if(i==1)
            {
                rowObj.IsHeader = true;
            }
            
            for (int j = 1; j <= row.Cells().Count(); j++)
            {

                Column column = new Column();
                column.Type = row.Cell(j).DataType.ToString();
                column.Value = row.Cell(j).Value.ToString();                
                rowObj.Columns.Add(column);
            }
            table.Rows.Add(rowObj);
        }

        return table;
    }

    public override SqlType ConvertToSqlType(string dataType)
    {
        return dataType?.ToLower() switch
        {
            DataTypeInferencer.TypeString => SqlType.VarChar,
            DataTypeInferencer.TypeDouble => SqlType.Float,
            DataTypeInferencer.TypeInt => SqlType.Int,
            DataTypeInferencer.TypeDateTime => SqlType.DateTime,
            DataTypeInferencer.TypeBool => SqlType.Bit,
            DataTypeInferencer.TypeDecimal => SqlType.Decimal,
            DataTypeInferencer.TypeLong => SqlType.BigInt,
            DataTypeInferencer.TypeByte => SqlType.TinyInt,
            _ => SqlType.Unknown
        };
    }
}
