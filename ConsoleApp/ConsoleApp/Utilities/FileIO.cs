using System;
using System.IO;
using System.Linq;
using Aspose.Cells;
using Aspose.Cells.Utility;
using Newtonsoft.Json;

namespace MaM.Utilities
{
  internal static class FileIO
  {
    public static void WriteFileToDrive(string filename, string content)
    {
      Console.WriteLine("Saving " + filename);
      File.WriteAllText(filename, content);
      Console.WriteLine("Saved");
    }

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

    private static string ObjectToJson(object obj)
    {
      return JsonConvert.SerializeObject(obj);
    }

    public static void WriteCurrentStateToDrive(ref GameState gameState)
    {
      var content = "{";
      content += "\n\"Time\":";
      content += ObjectToJson(gameState.time);
      content += "\n,";
      content += "\n\"Player\":";
      content += ObjectToJson(gameState.player);
      content += "\n,";
      content += "\n\"Journey\":";
      content += ObjectToJson(gameState.journey);
      content += "\n}";
      var filename = "MaM_Save_" + gameState.time.ToString("yyyy-dd-M_HH-mm-ss") + ".json";
      WriteFileToDrive(filename, content);
    }

    public static GameState GetGameStateFromFile(string fileName)
    {
      //TODO - implement
      return new GameState();
    }

  }
}
