using System.Text;
using ExcelToSql.Models;

namespace ExcelToSql.Extractors;

public class CsvExtractor : TableExtractor
{
    public CsvExtractor(string path) : base(path)
    {
    }

    public override Table ExtractTable(int worksheetIndex = 1)
    {
        Table table = new Table();
        table.Name = System.IO.Path.GetFileNameWithoutExtension(Path);

        var lines = File.ReadAllLines(Path, Encoding.UTF8);
        
        for (int i = 0; i < lines.Length; i++)
        {
            var values = lines[i].Split(',');

            Row rowObj = new Row();
            for (int j = 0; j < values.Length; j++)
            {
                Column column = new Column();
                string trimmedValue = values[j].Trim();
                column.Type = ConvertToSqlType(DataTypeInferencer.Infer(trimmedValue)).ToString();
                column.Value = trimmedValue;
                rowObj.Columns.Add(column);
            }

            if (i == 0)
                table.Header = rowObj;
            else
                table.Rows.Add(rowObj);
        }

        return table;
    }

    public override SqlType ConvertToSqlType(string dataType)
    {
        // CSV files are typically read as strings, so we default to VarChar
        return dataType?.ToLower() switch
        {
            DataTypeInferencer.TypeString => SqlType.VarChar,
            DataTypeInferencer.TypeInt => SqlType.Int,
            DataTypeInferencer.TypeDouble => SqlType.Float,
            DataTypeInferencer.TypeDecimal => SqlType.Decimal,
            DataTypeInferencer.TypeDateTime => SqlType.DateTime,
            DataTypeInferencer.TypeBool => SqlType.Bit,
            _ => SqlType.VarChar
        };
    }
}
