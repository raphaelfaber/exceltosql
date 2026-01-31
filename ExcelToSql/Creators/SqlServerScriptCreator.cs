using ExcelToSql.Models;

namespace ExcelToSql.Creators;

public class SqlServerScriptCreator : SqlScriptCreator
{
    protected override string CreateTableScriptCore(Table table)
    {
        var header = table.Header ?? table.Rows.FirstOrDefault();
        var columns = header?.Columns ?? new List<Column>();

        var colDefs = new List<string>();
        for (int i = 0; i < columns.Count; i++)
        {
            var colName = string.IsNullOrWhiteSpace(columns[i].Value) ? $"C{i + 1}" : columns[i].Value;
            var sqlType = MapToSqlServerType(columns[i].Type);
            colDefs.Add($"[{EscapeSqlIdentifier(colName)}] {sqlType}");
        }

        var cols = string.Join(", ", colDefs);
        var tableName = EscapeSqlIdentifier(table.Name ?? "unnamed");
        return $"CREATE TABLE [{tableName}] ({cols});";
    }

    protected override string InsertRowsScriptCore(Table table)
    {
        var headerRow = table.Header ?? table.Rows.FirstOrDefault();
        if (headerRow == null) return string.Empty;

        var columnNames = headerRow.Columns.Select((c, i) => string.IsNullOrWhiteSpace(c.Value) ? $"C{i+1}" : c.Value).ToList();
        var columnList = string.Join(", ", columnNames.Select(EscapeSqlIdentifierForBracket));

        var valueRows = new List<string>();
        foreach (var row in table.Rows)
        {
            var values = new List<string>();
            for (int i = 0; i < columnNames.Count; i++)
            {
                var col = i < row.Columns.Count ? row.Columns[i] : new Column { Type = "string", Value = null };
                values.Add(FormatValueForSql(col));
            }

            valueRows.Add($"({string.Join(", ", values)})");
        }

        if (!valueRows.Any()) return string.Empty;

        var tableName = EscapeSqlIdentifier(table.Name ?? "unnamed");
        return $"INSERT INTO [{tableName}] ({columnList}) VALUES\n{string.Join(",\n", valueRows)};";
    }

    private static string MapToSqlServerType(string? type)
    {
        if (string.IsNullOrWhiteSpace(type)) return "NVARCHAR(MAX)";
        var t = type.Trim().ToLowerInvariant();

        return t switch
        {
            "int" => "INT",
            "bigint" or "long" => "BIGINT",
            "smallint" => "SMALLINT",
            "tinyint" or "byte" => "TINYINT",
            "decimal" or "numeric" => "DECIMAL(18,2)",
            "float" or "double" => "FLOAT",

            "varchar" => "VARCHAR(255)",
            "nvarchar" => "NVARCHAR(255)",
            "nvarchar(max)" => "NVARCHAR(MAX)",
            "text" => "TEXT",
            "char" => "CHAR(1)",

            "datetime" => "DATETIME",
            "date" => "DATE",
            "time" => "TIME",

            "bit" or "bool" or "boolean" => "BIT",

            _ => "NVARCHAR(MAX)"
        }; 
    }

    private static string FormatValueForSql(Column col)
    {
        if (col == null || col.Value == null) return "NULL";

        var t = (col.Type ?? string.Empty).Trim().ToLowerInvariant();
        var v = col.Value;

        if (string.IsNullOrEmpty(v)) return "NULL";

        if (t == "int" || t == "bigint" || t == "smallint" || t == "tinyint")
            return int.TryParse(v, out _) ? v : "NULL";
        if (t == "decimal" || t == "numeric" || t == "float" || t == "double")
            return decimal.TryParse(v, out _) ? v : "NULL";
        if (t == "bit" || t == "bool" || t == "boolean")
        {
            if (v == "1" || v.Equals("true", StringComparison.OrdinalIgnoreCase)) return "1";
            if (v == "0" || v.Equals("false", StringComparison.OrdinalIgnoreCase)) return "0";
            return "NULL";
        }
        if (t == "datetime" || t == "date" || t == "time")
        {
            if (DateTime.TryParse(v, out var dt))
                return $"'{dt:yyyy-MM-dd HH:mm:ss.fff}'";
            return "NULL";
        }

        // Default: quote and escape single quotes
        return $"'{EscapeSqlString(v)}'";
    }

    private static string EscapeSqlString(string s) => s.Replace("'", "''");
    private static string EscapeSqlIdentifier(string? id) => (id ?? string.Empty).Replace("]", "]]");
    private static string EscapeSqlIdentifierForBracket(string? id) => $"[{EscapeSqlIdentifier(id)}]";
}