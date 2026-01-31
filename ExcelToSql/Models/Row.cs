namespace ExcelToSql.Models;

public class Row
{
    public bool IsHeader { get; set; }
    public List<Column> Columns { get; set; } = new List<Column>();
}
