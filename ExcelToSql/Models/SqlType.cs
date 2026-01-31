namespace ExcelToSql.Models;

public enum SqlType
{
    // Numeric types
    Int,
    BigInt,
    SmallInt,
    TinyInt,
    Decimal,
    Numeric,
    Float,
    Double,
    Real,
    
    // String types
    Char,
    VarChar,
    Text,
    NChar,
    NVarChar,
    NText,
    
    // Date/Time types
    Date,
    Time,
    DateTime,
    DateTime2,
    DateTimeOffset,
    Timestamp,
    SmallDateTime,
    
    // Boolean type
    Bit,
    Boolean,
    
    // Binary types
    Binary,
    VarBinary,
    Image,
    
    // Other types
    UniqueIdentifier,
    Guid,
    Json,
    Xml,
    Unknown
}
