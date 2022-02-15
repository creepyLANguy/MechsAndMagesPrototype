using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Cells;
using Aspose.Cells.Utility;
using MaM.Readers;
using Newtonsoft.Json;

namespace MaM.Helpers
{
  internal static class FileHelper
  {
    public static void WriteFileToDrive(string filename, string content)
    {
      Console.WriteLine("Saving " + filename);
      File.WriteAllText(filename, content);
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

    private static string ObjectToJson(object obj, bool indented = false)
    {
      return JsonConvert.SerializeObject(obj, indented ? Formatting.Indented : Formatting.None);
    }

    public static void WriteCurrentGameStateToFile(ref GameState gameState, string filename, bool indented = false)
    {
      //We remove full card lists before exporting as these bloat the save file.
      //The decks should be repopulated based on the stored ids when resuming a save state.
      gameState.player.Deck = null;

      var content = ObjectToJson(gameState, indented);

      WriteFileToDrive(filename, content);
    }

    public static GameState GetGameStateFromFile(string filename, ref List<Card> cards)
    {
      var contents = File.ReadAllText(filename);
      
      var gameState = JsonConvert.DeserializeObject<GameState>(contents);

      if (gameState.player != null)
      {
        gameState.player.Deck = CardReader.GetCardsFromIds(gameState.player.DeckCardIds, ref cards);
      }

      return gameState;
    }
    
    public static GameConfig GetGameConfigFromFile(string filename)
    {
      var contents = File.ReadAllText(filename);
      
      return JsonConvert.DeserializeObject<GameConfig>(contents);
    }
  }
}
