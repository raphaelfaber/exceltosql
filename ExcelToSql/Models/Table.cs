namespace ExcelToSql.Models;

public class Table
{
    public string Name { get; set; }

    // Header row (separate from data rows)
    public Row? Header { get; set; }

    // Rows contains only data rows (header is not included here)
    public List<Row> Rows { get; set; } = new List<Row>();
}
