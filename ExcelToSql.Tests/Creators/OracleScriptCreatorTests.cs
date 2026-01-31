using ExcelToSql.Creators;
using ExcelToSql.Models;

namespace ExcelToSql.Tests.Creators;

public class OracleScriptCreatorTests
{
    [Fact]
    public void CreateTableScript_GeneratesExpectedColumns()
    {
        var table = new Table
        {
            Name = "people",
            Header = new Row
            {
                Columns =
                {
                    new Column { Value = "Name", Type = "VarChar" },
                    new Column { Value = "Age", Type = "Int" },
                    new Column { Value = "Active", Type = "Bit" }
                }
            },
        };

        var creator = new OracleScriptCreator();
        var sql = creator.CreateTableScript(table);

        Assert.Contains("CREATE TABLE people", sql);
        Assert.Contains("Name VARCHAR2(255)", sql);
        Assert.Contains("Age NUMBER", sql);
        Assert.Contains("Active NUMBER(1)", sql);
    }

    [Fact]
    public void InsertRowsScript_GeneratesInsertsAndEscapesStrings()
    {
        var table = new Table
        {
            Name = "people",
            Header = new Row
            {
                Columns =
                {
                    new Column { Value = "Name", Type = "VarChar" },
                    new Column { Value = "Age", Type = "Int" },
                    new Column { Value = "Active", Type = "Bit" }
                }
            },
            Rows =
            {
                new Row
                {
                    Columns =
                    {
                        new Column { Value = "O'Malley", Type = "VarChar" },
                        new Column { Value = "30", Type = "Int" },
                        new Column { Value = "true", Type = "Bit" }
                    }
                },
                new Row
                {
                    Columns =
                    {
                        new Column { Value = null, Type = "VarChar" },
                        new Column { Value = "", Type = "Int" },
                        new Column { Value = "0", Type = "Bit" }
                    }
                }
            }
        };

        var creator = new OracleScriptCreator();
        var sql = creator.InsertRowsScript(table);

        Assert.Contains("INSERT ALL", sql);
        Assert.Contains("SELECT * FROM dual", sql);
        Assert.Contains("'O''Malley'", sql);
        Assert.Contains("30", sql);
        Assert.Contains("1", sql);
        Assert.Contains("NULL", sql);
    }

    [Fact]
    public void InsertRowsScript_ReturnsEmpty_WhenNoDataRows()
    {
        var table = new Table
        {
            Name = "t",
            Header = new Row { Columns = { new Column { Value = "C", Type = "VarChar" } } }
        };

        var creator = new OracleScriptCreator();
        var sql = creator.InsertRowsScript(table);

        Assert.Equal(string.Empty, sql);
    }
}