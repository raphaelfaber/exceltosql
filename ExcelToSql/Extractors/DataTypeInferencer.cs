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

    // Backward-compatible entrypoint: uses automatic culture detection
    public static string Infer(string value) => Infer(value, null);

    // New overload: allow callers to specify a culture; if null, method will
    // attempt to infer culture from the content (',' vs '.') and fall back
    // to invariant and pt-BR where appropriate.
    public static string Infer(string? value, CultureInfo? culture)
    {
        if (string.IsNullOrWhiteSpace(value))
            return TypeString;

        var s = value.Trim();

        // Boolean (most specific)
        if (s.Equals("true", StringComparison.OrdinalIgnoreCase) ||
            s.Equals("false", StringComparison.OrdinalIgnoreCase) ||
            s == "0" || s == "1")
            return TypeBool;

        // If caller provided a culture, prefer it for numeric/date parsing.
        // Be strict: only accept decimal parsing if the string contains the
        // culture's decimal separator (prevents treating "99,99" as 9999 for Invariant).
        if (culture != null)
        {
            var decSep = culture.NumberFormat.NumberDecimalSeparator;
            var hasOtherSep = s.Contains('.') || s.Contains(',');

            // If the value contains a separator but it is NOT the culture's
            // decimal separator, avoid parsing as decimal to prevent surprises.
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

        // Heuristic: if value contains a comma but no dot, try common comma cultures first
        var hasComma = s.Contains(',');
        var hasDot = s.Contains('.');

        if (hasComma && !hasDot)
        {
            // try pt-BR style (comma decimal)
            var ptBr = CultureInfo.GetCultureInfo("pt-BR");
            if (int.TryParse(s, NumberStyles.Integer, ptBr, out _))
                return TypeInt;
            if (decimal.TryParse(s, NumberStyles.Number, ptBr, out _))
                return TypeDouble;
            if (DateTime.TryParse(s, ptBr, DateTimeStyles.None, out _))
                return TypeDateTime;

            // fall through to other checks
        }

        if (hasDot && !hasComma)
        {
            // try invariant (dot decimal)
            var inv = CultureInfo.InvariantCulture;
            if (int.TryParse(s, NumberStyles.Integer, inv, out _))
                return TypeInt;
            if (decimal.TryParse(s, NumberStyles.Number, inv, out _))
                return TypeDouble;
            if (DateTime.TryParse(s, inv, DateTimeStyles.None, out _))
                return TypeDateTime;

            // fall through
        }

        // Generic attempts (covers inputs without clear separator or mixed)
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

        // Default
        return TypeString;
    }
}
