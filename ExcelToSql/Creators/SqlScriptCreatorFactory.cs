using ExcelToSql.Models;

namespace ExcelToSql.Creators;

/// <summary>
/// Factory that returns a dialect-specific <see cref="SqlScriptCreator"/> implementation.
/// </summary>
public static class SqlScriptCreatorFactory
{
    public static SqlScriptCreator Create(SqlDatabase database)
    {
        return database switch
        {
            SqlDatabase.SqlServer => new SqlServerScriptCreator(),
            SqlDatabase.Oracle => new OracleScriptCreator(),
            // Known-but-not-yet-implemented engines should surface a clear error
            SqlDatabase.MySql => throw new NotSupportedException("MySql support is not implemented."),
            SqlDatabase.PostgreSql => throw new NotSupportedException("PostgreSQL support is not implemented."),
            SqlDatabase.Sqlite => throw new NotSupportedException("SQLite support is not implemented."),
            SqlDatabase.Redshift => throw new NotSupportedException("Redshift support is not implemented."),
            SqlDatabase.Snowflake => throw new NotSupportedException("Snowflake support is not implemented."),
            SqlDatabase.Db2 => throw new NotSupportedException("DB2 support is not implemented."),
            _ => throw new ArgumentException($"Unsupported or unknown database: {database}", nameof(database))
        };
    }

    public static SqlScriptCreator Create(string databaseAlias)
    {
        if (string.IsNullOrWhiteSpace(databaseAlias))
            throw new ArgumentNullException(nameof(databaseAlias));

        var normalized = databaseAlias.Trim().ToLowerInvariant();

        // quick enum parse for canonical names
        if (System.Enum.TryParse<SqlDatabase>(normalized, true, out var parsed) && parsed != SqlDatabase.Unknown)
            return Create(parsed);

        // common aliases
        var db = normalized switch
        {
            "mssql" or "sqlserver" or "sql-server" or "sql server" => SqlDatabase.SqlServer,
            "oracle" => SqlDatabase.Oracle,
            "mysql" or "mariadb" => SqlDatabase.MySql,
            "postgres" or "postgresql" or "psql" => SqlDatabase.PostgreSql,
            "sqlite" or "sql-lite" => SqlDatabase.Sqlite,
            "redshift" => SqlDatabase.Redshift,
            "snowflake" => SqlDatabase.Snowflake,
            "db2" => SqlDatabase.Db2,
            _ => SqlDatabase.Unknown
        };

        if (db == SqlDatabase.Unknown)
            throw new ArgumentException($"Unknown or unsupported database alias: '{databaseAlias}'", nameof(databaseAlias));

        return Create(db);
    }
}