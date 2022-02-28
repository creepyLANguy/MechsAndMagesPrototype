using System;
using System.Collections.Generic;
using System.IO;
using MaM.Definitions;
using MaM.Readers;
using Newtonsoft.Json;

namespace MaM.Helpers
{
 public static class SaveFileHelper
 {
   private const string SaveFileDirectory = @"savegames\";

    private static string ObjectToJson(object obj, bool indented = false)
    {
      return JsonConvert.SerializeObject(obj, indented ? Formatting.Indented : Formatting.None);
    }

    public static bool IsLegit(string filename)
    {
      return string.IsNullOrEmpty(filename) == false && File.Exists(SaveFileDirectory + filename) == true;
    }

    public static GameState GetGameStateFromFile(string filename, ref List<Card> cards, string cryptoKey = null)
    {
      var content = File.ReadAllText(SaveFileDirectory + filename);

      if (cryptoKey != null)
      {
        content = Crypto.DecryptString(content, cryptoKey);
      }

      var gameState = JsonConvert.DeserializeObject<GameState>(content);

      if (gameState.player != null)
      {
        gameState.player.deck = CardReader.GetCardsFromIds(gameState.player.deckCardIds, ref cards);
      }

      return gameState;
    }

    public static bool SaveGameStateToFile(ref GameState gameState, string filename, string cryptoKey = null, bool indented = true)
    {
      if (filename == null)
      {
        Console.WriteLine("filename was null");
        return false;
      }

      //We remove full card lists before exporting as these bloat the save file.
      //The decks should be repopulated based on the stored ids when resuming a save state.
      gameState.player.deck = null;

      var content = ObjectToJson(gameState, indented);

      if (cryptoKey != null)
      {
        content = Crypto.EncryptString(content, cryptoKey);
      }

      FileHelper.WriteFileToDrive(filename, content, SaveFileDirectory);

      return true;
    }

    public static bool Erase(string filename)
    {
      if (filename == null)
      {
        Console.WriteLine("filename was null");
        return false;
      }

      FileHelper.DeleteFileFromDrive(filename, SaveFileDirectory);

      return true;
    }
   
    public static bool PromptUserToSaveGame(ref GameState gameState, string cryptoKey = null)
    {
      if (UserInput.Request("\nWould you like to save your game?\n1) Yes\n2) No") == "2")
      {
        Console.WriteLine("WARNING - DID NOT SAVE GAME!");
        return false;
      }

      var filename = UserInput.Request("\nProvide a name for your save file:");

      return SaveGameStateToFile(ref gameState, filename, cryptoKey);
    }
  }
}
