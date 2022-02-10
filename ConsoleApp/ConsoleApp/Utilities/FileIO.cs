using System;
using System.IO;
using System.Linq;
using Aspose.Cells;
using Aspose.Cells.Utility;

namespace MaM
{
  internal static class FileIO
  {
    public static string ExcelToJson(string excelFile)
    {
      var workbook = new Workbook(excelFile, new LoadOptions(LoadFormat.Auto));

      var cells = workbook.Worksheets.First().Cells;

      var range = cells.CreateRange(0, 0, cells.LastCell.Row + 1, cells.LastCell.Column + 1);

      var exportOptions = new ExportRangeToJsonOptions();

      var json = JsonUtility.ExportRangeToJson(range, exportOptions);

      //System.IO.File.WriteAllText("output.json", jsonData);

      return json;
    }

    public static void SaveFile(string filename, string content)
    {
      Console.WriteLine("Saving " + filename);
      File.WriteAllText(filename, content);
      Console.WriteLine("Saved");
    }

  }
}
