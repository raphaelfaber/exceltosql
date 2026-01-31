namespace ExcelToSql.Extractors;

using System.Globalization;

public static class DataTypeInferencer
{
    public const string TypeBool = "bool";
    public const string TypeInt = "int";
    public const string TypeDouble = "double";
    public const string TypeDecimal = "decimal";
    public const string TypeDateTime = "datetime";
    public const string TypeString = "string";
    public const string TypeLong = "long";
    public const string TypeByte = "byte";

    public static string Infer(string value) => Infer(value, null);

    public static string Infer(string? value, CultureInfo? culture)
    {
        if (string.IsNullOrWhiteSpace(value))
            return TypeString;

        var s = value.Trim();

        if (s.Equals("true", StringComparison.OrdinalIgnoreCase) ||
            s.Equals("false", StringComparison.OrdinalIgnoreCase) ||
            s == "0" || s == "1")
            return TypeBool;

        if (culture != null)
        {
            var decSep = culture.NumberFormat.NumberDecimalSeparator;
            var hasOtherSep = s.Contains('.') || s.Contains(',');

            if (hasOtherSep && !s.Contains(decSep))
            {
                if (int.TryParse(s, NumberStyles.Integer, culture, out _))
                    return TypeInt;
                if (DateTime.TryParse(s, culture, DateTimeStyles.None, out _))
                    return TypeDateTime;

                return TypeString;
            }

            if (int.TryParse(s, NumberStyles.Integer, culture, out _))
                return TypeInt;
            if (decimal.TryParse(s, NumberStyles.Number, culture, out _))
                return TypeDouble;
            if (DateTime.TryParse(s, culture, DateTimeStyles.None, out _))
                return TypeDateTime;

            return TypeString;
        }

        var hasComma = s.Contains(',');
        var hasDot = s.Contains('.');

        if (hasComma && !hasDot)
        {
            var ptBr = CultureInfo.GetCultureInfo("pt-BR");
            if (int.TryParse(s, NumberStyles.Integer, ptBr, out _))
                return TypeInt;
            if (decimal.TryParse(s, NumberStyles.Number, ptBr, out _))
                return TypeDouble;
            if (DateTime.TryParse(s, ptBr, DateTimeStyles.None, out _))
                return TypeDateTime;
        }

        if (hasDot && !hasComma)
        {
            var inv = CultureInfo.InvariantCulture;
            if (int.TryParse(s, NumberStyles.Integer, inv, out _))
                return TypeInt;
            if (decimal.TryParse(s, NumberStyles.Number, inv, out _))
                return TypeDouble;
            if (DateTime.TryParse(s, inv, DateTimeStyles.None, out _))
                return TypeDateTime;
        }

        if (int.TryParse(s, NumberStyles.Integer, CultureInfo.InvariantCulture, out _))
            return TypeInt;
        if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.InvariantCulture, out _))
            return TypeDouble;
        if (decimal.TryParse(s, NumberStyles.Number, CultureInfo.CurrentCulture, out _))
            return TypeDouble;
        if (DateTime.TryParse(s, CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            return TypeDateTime;
        if (DateTime.TryParse(s, CultureInfo.CurrentCulture, DateTimeStyles.None, out _))
            return TypeDateTime;

        return TypeString;
    } 
}
