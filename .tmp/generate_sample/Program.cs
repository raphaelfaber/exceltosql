using ClosedXML.Excel;

internal static class Program
{
    private static void Main()
    {
        var path = "/home/raphaelfaber/Documentos/Respositories/exceltosql/ExcelToSql/samples/sample.xlsx";

        var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add("Users");

        // Header
        ws.Cell(1, 1).Value = "Nome";
        ws.Cell(1, 2).Value = "Idade";
        ws.Cell(1, 3).Value = "Peso";
        ws.Cell(1, 4).Value = "Altura";

        // Rows
        ws.Cell(2, 1).Value = "Ana";
        ws.Cell(2, 2).Value = 29;
        ws.Cell(2, 3).Value = 62.5;
        ws.Cell(2, 4).Value = 1.68;

        ws.Cell(3, 1).Value = "Bruno";
        ws.Cell(3, 2).Value = 35;
        ws.Cell(3, 3).Value = 82.3;
        ws.Cell(3, 4).Value = 1.82;

        ws.Cell(4, 1).Value = "Carla";
        ws.Cell(4, 2).Value = 42;
        ws.Cell(4, 3).Value = 70.0;
        ws.Cell(4, 4).Value = 1.60;

        wb.SaveAs(path);
        System.Console.WriteLine($"Wrote: {path}");
    }
}
