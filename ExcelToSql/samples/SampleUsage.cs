// Sample usage (kept for reference). This file is not compiled into the library.
// To run the sample, copy the code into a console app or enable compilation temporarily.

using System;
using System.IO;
using ExcelToSql.Extractors;
using ExcelToSql.Creators;
using ExcelToSql.Models;

public static class SampleUsage
{
    // Loads the example workbook and extracts the first worksheet as a Table.
    // This demonstrates how a consumer would obtain the model from the library.
    public static ExcelToSql.Models.Table LoadSample()
    {
        string filePath = "samples/sample.xlsx";
        TableExtractor extractor = TableExtractorFactory.CreateExtractor(filePath);
        return extractor.ExtractTable(1);
    }

    // Generate SQL Server scripts (CREATE + INSERT) from a Table.
    // Returns a tuple with the CREATE statement and the INSERTs block.
    public static (string create, string inserts) GenerateSqlServerScripts(Table table)
    {
        var creator = SqlScriptCreatorFactory.Create(SqlDatabase.SqlServer);
        var create = creator.CreateTableScript(table);
        var inserts = creator.InsertRowsScript(table);
        return (create, inserts);
    }

    // Generate Oracle scripts (CREATE + INSERT ALL) from a Table.
    public static (string create, string inserts) GenerateOracleScripts(Table table)
    {
        var creator = SqlScriptCreatorFactory.Create(SqlDatabase.Oracle);
        var create = creator.CreateTableScript(table);
        var inserts = creator.InsertRowsScript(table);
        return (create, inserts);
    }

    // Example method that demonstrates end-to-end usage and writes results to disk.
    // - Loads the sample workbook
    // - Generates SQL Server and Oracle scripts
    // - Writes two files under the samples/ folder
    public static void DemoWriteScriptsToSamples()
    {
        var table = LoadSample();
        var (createSqlServer, insertSqlServer) = GenerateSqlServerScripts(table);
        var (createOracle, insertOracle) = GenerateOracleScripts(table);

        // Compose file bodies
        var sqlServerFile = createSqlServer + "\n\n" + insertSqlServer;
        var oracleFile = createOracle + "\n\n" + insertOracle;

        // Ensure samples directory exists and write files for easy inspection
        Directory.CreateDirectory("samples");
        File.WriteAllText("samples/sample.sqlserver.sql", sqlServerFile);
        File.WriteAllText("samples/sample.oracle.sql", oracleFile);

        // Print short summary to console for quick verification
        Console.WriteLine("Wrote scripts:");
        Console.WriteLine(" - samples/sample.sqlserver.sql");
        Console.WriteLine(" - samples/sample.oracle.sql");
    }

    // Lightweight demo that returns the generated SQL texts (useful in REPL/tests).
    public static (string sqlServer, string oracle) DemoReturnScripts()
    {
        var t = LoadSample();
        var ss = GenerateSqlServerScripts(t);
        var or = GenerateOracleScripts(t);
        return (ss.create + "\n\n" + ss.inserts, or.create + "\n\n" + or.inserts);
    }
}