namespace ExcelToSql.Creators;

public abstract class SqlScriptCreator
{
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

    protected abstract string CreateTableScriptCore(ExcelToSql.Models.Table table);
    protected abstract string InsertRowsScriptCore(ExcelToSql.Models.Table table);
} 