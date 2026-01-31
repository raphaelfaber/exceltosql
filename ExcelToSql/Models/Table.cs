namespace ExcelToSql.Models;

public class Table
{
    // Name can be empty initially; default to empty string to satisfy the
    // nullable-analysis and avoid surprise nulls for consumers.
    public string Name { get; set; } = string.Empty;

    // Header row (separate from data rows)
    public Row? Header { get; set; }

    // Rows contains only data rows (header is not included here)
    public List<Row> Rows { get; set; } = new List<Row>();
}
