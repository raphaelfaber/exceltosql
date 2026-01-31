namespace ExcelToSql.Creators;

public abstract class SqlScriptCreator
{
    // New API: methods accept a Table parameter so callers can pass different tables.
    // Public methods validate arguments and delegate to protected abstract implementations
    public string CreateTableScript(ExcelToSql.Models.Table table)
    {
        if (table is null) throw new ArgumentNullException(nameof(table));
        return CreateTableScriptCore(table);
    }

    public string InsertRowsScript(ExcelToSql.Models.Table table)
    {
        if (table is null) throw new ArgumentNullException(nameof(table));
        return InsertRowsScriptCore(table);
    }

    // Implementors override these to provide the actual script generation logic
    protected abstract string CreateTableScriptCore(ExcelToSql.Models.Table table);
    protected abstract string InsertRowsScriptCore(ExcelToSql.Models.Table table);
}