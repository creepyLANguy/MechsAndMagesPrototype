using System;
using System.IO;
using System.Text;
using Aspose.Cells;
using Aspose.Cells.Utility;

namespace MaM.Helpers;

public static class FileHelper
{
  public static bool WriteFileToDrive(string filename, string content, string folderName = "")
  {
    if (folderName != string.Empty && Directory.Exists(folderName) == false)
    {
      Directory.CreateDirectory(folderName ?? string.Empty);
    }

    filename = folderName + filename;

    Console.WriteLine("\nSaving " + filename);

    using (var sw = new StreamWriter(File.Open(filename, FileMode.Create, FileAccess.Write), Encoding.UTF8))
    {
      try
      {
        sw.Write(content);
      }
      catch (Exception e)
      {
        Console.WriteLine("\nFAILED TO WRITE FILE \'" + filename + "\' TO DRIVE!");
        Console.WriteLine(e.Message);
        return false;
      }
    }

    Console.WriteLine("Saved");

    return true;
  }

  public static bool DeleteFileFromDrive(string filename, string folderName = "")
  {
    if (folderName != string.Empty && Directory.Exists(folderName) == false)
    {
      Console.WriteLine("\nError - could not find folder : " + folderName);
      return false;
    }

    filename = folderName + filename;

    Console.WriteLine("\nDeleting " + filename);

    try
    {
      File.Delete(filename);
    }
    catch (Exception e)
    {
      Console.WriteLine("\nFAILED TO DELETE FILE \'" + filename + "\' ON DRIVE!");
      Console.WriteLine(e.Message);
      return false;
    }
      
    Console.WriteLine("Deleted");

    return true;
  }

  public static string ExcelToJson(string excelFile, int worksheetIndex = 0)
  {
    var loadOptions = new LoadOptions(LoadFormat.Auto);

    var workbook = new Workbook(excelFile, loadOptions);

    var cells = workbook.Worksheets[worksheetIndex].Cells;

    var range = cells.CreateRange(0, 0, cells.LastCell.Row + 1, cells.LastCell.Column + 1);

    var exportOptions = new ExportRangeToJsonOptions();

    var json = JsonUtility.ExportRangeToJson(range, exportOptions);

    return json;
  }
}