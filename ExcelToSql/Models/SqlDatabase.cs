namespace ExcelToSql.Models;

/// <summary>
/// Supported SQL database engines used for dialect-specific behavior.
/// </summary>
public enum SqlDatabase
{
    /// <summary>Unknown or not specified.</summary>
    Unknown = 0,

    /// <summary>Microsoft SQL Server (T-SQL).</summary>
    SqlServer,

    /// <summary>Oracle Database (PL/SQL dialect).</summary>
    Oracle,

    /// <summary>MySQL / MariaDB family.</summary>
    MySql,

    /// <summary>PostgreSQL.</summary>
    PostgreSql,

    /// <summary>SQLite.</summary>
    Sqlite,

    /// <summary>IBM DB2.</summary>
    Db2,

    /// <summary>Cloud columnar / analytics (not full dialect parity).</summary>
    Snowflake,

    /// <summary>Amazon Redshift (Postgres-derived).</summary>
    Redshift
}