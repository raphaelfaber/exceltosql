namespace ExcelToSql.Models;

public class Table
{
    public string Name { get; set; } = string.Empty;

    public Row? Header { get; set; }

    public List<Row> Rows { get; set; } = new List<Row>();
} 
