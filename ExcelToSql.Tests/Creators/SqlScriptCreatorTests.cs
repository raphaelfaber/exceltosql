using ExcelToSql.Creators;
using ExcelToSql.Models;

namespace ExcelToSql.Tests.Creators;

public class SqlScriptCreatorTests
{
    private class TestCreator : SqlScriptCreator
    {
        public TestCreator() { }
        protected override string CreateTableScriptCore(Table table) => $"CREATE {table.Name}";
        protected override string InsertRowsScriptCore(Table table) => $"INSERT {table.Name}";

        // Helper used in tests to call the public API
        public string CallCreate(Table table) => CreateTableScript(table);
        public string CallInsert(Table table) => InsertRowsScript(table);
    }

    [Fact]
    public void CreateTableScript_UsesProvidedTable()
    {
        var table = new Table { Name = "t" };
        var creator = new TestCreator();

        var sql = creator.CallCreate(table);

        Assert.Equal("CREATE t", sql);
    }

    [Fact]
    public void InsertRowsScript_UsesProvidedTable()
    {
        var table = new Table { Name = "t" };
        var creator = new TestCreator();

        var sql = creator.CallInsert(table);

        Assert.Equal("INSERT t", sql);
    }

    [Fact]
    public void PublicMethods_NullTable_ThrowArgumentNullException()
    {
        var creator = new TestCreator();
        Assert.Throws<ArgumentNullException>(() => creator.CallCreate(null!));
        Assert.Throws<ArgumentNullException>(() => creator.CallInsert(null!));
    }
}