using ExcelToSql.Extractors;
using System.Globalization;

namespace ExcelToSql.Tests.Extractors;

public class DataTypeInferencerTests
{
    [Fact]
    public void Infer_NullOrWhitespace_ReturnsString()
    {
        Assert.Equal(DataTypeInferencer.TypeString, DataTypeInferencer.Infer(null!));
        Assert.Equal(DataTypeInferencer.TypeString, DataTypeInferencer.Infer(string.Empty));
        Assert.Equal(DataTypeInferencer.TypeString, DataTypeInferencer.Infer("   "));
    }

    [Theory]
    [InlineData("true")]
    [InlineData("False")]
    [InlineData("0")]
    [InlineData("1")]
    public void Infer_BooleanVariants_ReturnsBool(string input)
        => Assert.Equal(DataTypeInferencer.TypeBool, DataTypeInferencer.Infer(input));

    [Theory]
    [InlineData("123")]
    [InlineData("-42")]
    [InlineData("20240101")] // numeric-looking date will be classified as int because int check runs before DateTime
    public void Infer_IntegerValues_ReturnsInt(string input)
        => Assert.Equal(DataTypeInferencer.TypeInt, DataTypeInferencer.Infer(input));

    [Theory]
    [InlineData("3.14")]
    [InlineData("-0.5")]
    [InlineData("1234567890123456789012345")]
    public void Infer_DecimalValues_ReturnsDouble(string input)
        => Assert.Equal(DataTypeInferencer.TypeDouble, DataTypeInferencer.Infer(input));

    [Fact]
    public void Infer_DecimalWithComma_AutoRecognition_ReturnsDouble()
        => Assert.Equal(DataTypeInferencer.TypeDouble, DataTypeInferencer.Infer("99,99"));

    [Fact]
    public void Infer_DecimalWithComma_WithPtBrCulture_ReturnsDouble()
        => Assert.Equal(DataTypeInferencer.TypeDouble, DataTypeInferencer.Infer("99,99", new CultureInfo("pt-BR")));

    [Fact]
    public void Infer_DecimalWithComma_WithInvariantCulture_DoesNotParseAsDouble()
        => Assert.NotEqual(DataTypeInferencer.TypeDouble, DataTypeInferencer.Infer("99,99", CultureInfo.InvariantCulture));

    [Theory]
    [InlineData("2024-01-31")]
    [InlineData("Jan 31, 2024")]
    public void Infer_DateTimeValues_ReturnsDateTime(string input)
        => Assert.Equal(DataTypeInferencer.TypeDateTime, DataTypeInferencer.Infer(input));

    [Fact]
    public void Infer_DateWithSlash_WithPtBrCulture_ReturnsDateTime()
        => Assert.Equal(DataTypeInferencer.TypeDateTime, DataTypeInferencer.Infer("31/01/2024", new CultureInfo("pt-BR")));

    [Theory]
    [InlineData("hello")]
    [InlineData("123abc")]
    public void Infer_OtherValues_ReturnsString(string input)
        => Assert.Equal(DataTypeInferencer.TypeString, DataTypeInferencer.Infer(input));
}