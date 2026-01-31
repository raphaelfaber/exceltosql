using ExcelToSql.Models;

namespace ExcelToSql.Creators;

public class OracleScriptCreator : SqlScriptCreator
{
    protected override string CreateTableScriptCore(Table table)
    {
        var header = table.Header ?? table.Rows.FirstOrDefault();
        var columns = header?.Columns ?? new List<Column>();

        var colDefs = new List<string>();
        for (int i = 0; i < columns.Count; i++)
        {
            var colName = string.IsNullOrWhiteSpace(columns[i].Value) ? $"C{i + 1}" : columns[i].Value;
            var sqlType = MapToOracleType(columns[i].Type);
            colDefs.Add($"{EscapeSqlIdentifier(colName)} {sqlType}");
        }

        var cols = string.Join(", ", colDefs);
        var tableName = EscapeSqlIdentifier(table.Name ?? "unnamed");
        return $"CREATE TABLE {tableName} ({cols});";
    }

    protected override string InsertRowsScriptCore(Table table)
    {
        var headerRow = table.Header ?? table.Rows.FirstOrDefault();
        if (headerRow == null) return string.Empty;

        var columnNames = headerRow.Columns.Select((c, i) => string.IsNullOrWhiteSpace(c.Value) ? $"C{i+1}" : c.Value).ToList();
        var columnList = string.Join(", ", columnNames.Select(EscapeSqlIdentifier));

        var intoLines = new List<string>();
        foreach (var row in table.Rows)
        {
            var values = new List<string>();
            for (int i = 0; i < columnNames.Count; i++)
            {
                var col = i < row.Columns.Count ? row.Columns[i] : new Column { Type = "string", Value = null };
                values.Add(FormatValueForSql(col));
            }

            intoLines.Add($"  INTO {EscapeSqlIdentifier(table.Name ?? "unnamed")} ({columnList}) VALUES ({string.Join(", ", values)})");
        }

        if (!intoLines.Any()) return string.Empty;

        return $"INSERT ALL\n{string.Join("\n", intoLines)}\nSELECT * FROM dual;";
    }

    private static string MapToOracleType(string? type)
    {
        if (string.IsNullOrWhiteSpace(type)) return "NVARCHAR2(255)";
        var t = type.Trim().ToLowerInvariant();

        return t switch
        {
            "int" => "NUMBER",
            "bigint" or "long" => "NUMBER",
            "smallint" => "NUMBER",
            "tinyint" or "byte" => "NUMBER",
            "decimal" or "numeric" => "NUMBER(18,2)",
            "float" or "double" => "FLOAT",

            "varchar" => "VARCHAR2(255)",
            "nvarchar" => "NVARCHAR2(255)",
            "nvarchar(max)" => "NCLOB",
            "text" => "CLOB",
            "char" => "CHAR(1)",

            "datetime" => "TIMESTAMP",
            "date" => "DATE",
            "time" => "TIMESTAMP",

            "bit" or "bool" or "boolean" => "NUMBER(1)",

            _ => "NVARCHAR2(255)"
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
            return decimal.TryParse(v, System.Globalization.NumberStyles.Number, System.Globalization.CultureInfo.InvariantCulture, out _) ? v : "NULL";
        if (t == "bit" || t == "bool" || t == "boolean")
        {
            if (v == "1" || v.Equals("true", StringComparison.OrdinalIgnoreCase)) return "1";
            if (v == "0" || v.Equals("false", StringComparison.OrdinalIgnoreCase)) return "0";
            return "NULL";
        }
        if (t == "datetime" || t == "date" || t == "time")
        {
            if (DateTime.TryParse(v, out var dt))
                return $"TO_TIMESTAMP('{dt:yyyy-MM-dd HH:mm:ss.fff}','YYYY-MM-DD HH24:MI:SS.FF3')";
            return "NULL";
        }

        return $"'{EscapeSqlString(v)}'";
    }

    private static string EscapeSqlString(string s) => s.Replace("'", "''");
    private static string EscapeSqlIdentifier(string? id) => (id ?? string.Empty).Replace("\"", "\"\"");
}
