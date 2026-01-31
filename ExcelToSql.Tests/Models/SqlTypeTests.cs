using ExcelToSql.Models;

namespace ExcelToSql.Tests.Models;

public class SqlTypeTests
{
    [Fact]
    public void SqlType_ShouldHaveAllNumericTypes()
    {
        // Assert
        Assert.True(Enum.IsDefined(typeof(SqlType), SqlType.Int));
        Assert.True(Enum.IsDefined(typeof(SqlType), SqlType.BigInt));
        Assert.True(Enum.IsDefined(typeof(SqlType), SqlType.SmallInt));
        Assert.True(Enum.IsDefined(typeof(SqlType), SqlType.Decimal));
        Assert.True(Enum.IsDefined(typeof(SqlType), SqlType.Float));
    }

    [Fact]
    public void SqlType_ShouldHaveAllStringTypes()
    {
        // Assert
        Assert.True(Enum.IsDefined(typeof(SqlType), SqlType.VarChar));
        Assert.True(Enum.IsDefined(typeof(SqlType), SqlType.Char));
        Assert.True(Enum.IsDefined(typeof(SqlType), SqlType.Text));
        Assert.True(Enum.IsDefined(typeof(SqlType), SqlType.NVarChar));
    }

    [Fact]
    public void SqlType_ShouldHaveDateTimeTypes()
    {
        // Assert
        Assert.True(Enum.IsDefined(typeof(SqlType), SqlType.Date));
        Assert.True(Enum.IsDefined(typeof(SqlType), SqlType.DateTime));
        Assert.True(Enum.IsDefined(typeof(SqlType), SqlType.Time));
    }

    [Fact]
    public void SqlType_ShouldHaveUnknownAsDefault()
    {
        // Assert
        Assert.True(Enum.IsDefined(typeof(SqlType), SqlType.Unknown));
    }
}
