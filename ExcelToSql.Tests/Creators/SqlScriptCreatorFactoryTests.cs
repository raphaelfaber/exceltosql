using ExcelToSql.Creators;
using ExcelToSql.Models;

namespace ExcelToSql.Tests.Creators;

public class SqlScriptCreatorFactoryTests
{
    [Fact]
    public void Create_ByEnum_ReturnsSqlServerCreator()
    {
        var c = SqlScriptCreatorFactory.Create(SqlDatabase.SqlServer);
        Assert.IsType<SqlServerScriptCreator>(c);
    }

    [Fact]
    public void Create_ByEnum_ReturnsOracleCreator()
    {
        var c = SqlScriptCreatorFactory.Create(SqlDatabase.Oracle);
        Assert.IsType<OracleScriptCreator>(c);
    }

    [Theory]
    [InlineData("mssql")]
    [InlineData("SQLServer")]
    [InlineData("sql server")]
    public void Create_ByAlias_ReturnsSqlServer(string alias)
    {
        var c = SqlScriptCreatorFactory.Create(alias);
        Assert.IsType<SqlServerScriptCreator>(c);
    }

    [Theory]
    [InlineData("oracle")]
    [InlineData("Oracle")]
    public void Create_ByAlias_ReturnsOracle(string alias)
    {
        var c = SqlScriptCreatorFactory.Create(alias);
        Assert.IsType<OracleScriptCreator>(c);
    }

    [Fact]
    public void Create_UnknownAlias_Throws()
    {
        Assert.Throws<ArgumentException>(() => SqlScriptCreatorFactory.Create("nope"));
    }

    [Fact]
    public void Create_UnknownEnum_Throws()
    {
        Assert.Throws<ArgumentException>(() => SqlScriptCreatorFactory.Create(SqlDatabase.Unknown));
    }
}