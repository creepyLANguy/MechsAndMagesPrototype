using System;
using System.IO;
using System.Text;
using Aspose.Cells;
using Aspose.Cells.Utility;

namespace MaM.Helpers
{
 public static class FileHelper
  {
    public static void WriteFileToDrive(string filename, string content, string folderName = "")
    {
      if (folderName != string.Empty && Directory.Exists(folderName) == false)
      {
        Directory.CreateDirectory(folderName);
      }

      filename = folderName + filename;

      Console.WriteLine("Saving " + filename);

      using (var sw = new StreamWriter(File.Open(filename, FileMode.Create, FileAccess.Write), Encoding.UTF8))
      {
        sw.Write(content);
      }

      Console.WriteLine("Saved");
    }

    public static string ExcelToJson(string excelFile, int worksheetIndex = 0)
    {
      var workbook = new Workbook(excelFile, new LoadOptions(LoadFormat.Auto));

      var cells = workbook.Worksheets[worksheetIndex].Cells;

      var range = cells.CreateRange(0, 0, cells.LastCell.Row + 1, cells.LastCell.Column + 1);

      var exportOptions = new ExportRangeToJsonOptions();

      var json = JsonUtility.ExportRangeToJson(range, exportOptions);

      return json;
    }
  }
}
