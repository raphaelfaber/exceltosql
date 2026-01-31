using ExcelToSql.Models;

namespace ExcelToSql.Tests.Models;

public class SqlDatabaseTests
{
    [Fact]
    public void Enum_DefinesExpectedValues()
    {
        var names = System.Enum.GetNames(typeof(SqlDatabase));

        Assert.Contains("Unknown", names);
        Assert.Contains("SqlServer", names);
        Assert.Contains("Oracle", names);
        Assert.Contains("MySql", names);
        Assert.Contains("PostgreSql", names);
        Assert.Contains("Sqlite", names);
    }

    [Fact]
    public void DefaultValue_IsUnknown()
    {
        Assert.Equal(SqlDatabase.Unknown, default(SqlDatabase));
    }
}