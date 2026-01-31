namespace ExcelToSql.Models;

public class Column
{
    // Type may be unknown for some cells -> nullable
    public string? Type { get; set; }

    // Value may be missing (empty cell) -> nullable
    public string? Value { get; set; }
}
