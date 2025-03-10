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

    Terminal.FileHelperSave(filename);

    using (var sw = new StreamWriter(File.Open(filename, FileMode.Create, FileAccess.Write), Encoding.UTF8))
    {
      try
      {
        sw.Write(content);
      }
      catch (Exception e)
      {
        Terminal.FileHelperWriteFailed(filename, e.Message);
        return false;
      }
    }

    Terminal.FileHelperWritten();

    return true;
  }

  public static bool DeleteFileFromDrive(string filename, string folderName = "")
  {
    if (folderName != string.Empty && Directory.Exists(folderName) == false)
    {
      Terminal.FileHelperFolderNotFound(folderName);
      return false;
    }

    filename = folderName + filename;

    Terminal.FileHelperDeleting(filename);

    try
    {
      File.Delete(filename);
    }
    catch (Exception e)
    {
      Terminal.FileHelperDeleteFailed(filename, e.Message);
      return false;
    }
      
    Terminal.FileHelperDeleted();

    return true;
  }

  public static string ExcelToJson(string excelFile, int worksheetIndex = 0)
  {
    var loadOptions = new LoadOptions(LoadFormat.Auto);

    var workbook = new Workbook(excelFile, loadOptions);

    var cells = workbook.Worksheets[worksheetIndex].Cells;

    var range = cells.CreateRange(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1);

    var exportOptions = new ExportRangeToJsonOptions();

    var json = JsonUtility.ExportRangeToJson(range, exportOptions);

    return json;
  }
}