using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using MaM.Definitions;
using MaM.Readers;
using Newtonsoft.Json;

namespace MaM.Helpers;

public static class SaveGameHelper
{
  private static string ObjectToJson(object obj, bool indented = false) 
    => JsonConvert.SerializeObject(obj, indented ? Formatting.Indented : Formatting.None);

  public static bool IsLegit(string filename) 
    => string.IsNullOrEmpty(filename) == false && File.Exists(SaveGame.SaveFileDirectory + filename);

  public static GameState Read(string filename, List<Card> cards = null, string cryptoKey = null)
  {
    var content = File.ReadAllText(SaveGame.SaveFileDirectory + filename);

    if (cryptoKey != null)
    {
      var decryptedContent = Crypto.DecryptString(content, cryptoKey);
      var decodedContent = Convert.FromBase64String(decryptedContent);
      var utf8Content = Encoding.UTF8.GetString(decodedContent);
      content = utf8Content;
    }

    var gameState = JsonConvert.DeserializeObject<GameState>(content);

    if (gameState.player != null)
    {
      gameState.player.deck = CardReader.GetCardsFromIds(gameState.player.deckCardIds, ref cards);
    }

    return gameState;
  }

  public static bool Save(int randomSeed, ref Player player, string filename, string cryptoKey = null)
  {
    var gameState = new GameState(DateTime.Now, randomSeed, player);
    return SaveGameStateToFile(ref gameState, filename, cryptoKey);
  }

  private static bool SaveGameStateToFile(ref GameState gameState, string filename, string cryptoKey = null)
  {
    if (filename == null)
    {
      Console.WriteLine("filename was null");
      return false;
    }

    var backupDeck = gameState.player.deck.ToList();

    //We remove full card lists before exporting as these bloat the save file.
    //Make sure that you repopulate the player's deck based on the stored ids when resuming a save state.
    gameState.player.deck = null;

    var content = ObjectToJson(gameState, true);
      
    gameState.player.deck = backupDeck;

    if (cryptoKey != null)
    {
      content = Crypto.EncryptString(content, cryptoKey);
    }

    return FileHelper.WriteFileToDrive(filename, content, SaveGame.SaveFileDirectory);
  }

  public static bool Delete(string filename)
  {
    if (filename == null)
    {
      Console.WriteLine("filename was null");
      return false;
    }

    return FileHelper.DeleteFileFromDrive(filename, SaveGame.SaveFileDirectory);
  }
   
  public static string PromptUserToSelectSaveSlot()
  {
    var list = new List<Tuple<string, int>>();
    list.Add(new Tuple<string, int>("Begin A New Save Slot", 0));

    if (Directory.Exists(SaveGame.SaveFileDirectory) == false)
    {
      Directory.CreateDirectory(SaveGame.SaveFileDirectory);
    }

    var allFiles =
      Directory.EnumerateFiles(SaveGame.SaveFileDirectory)
        .Select(file => file.Substring(file.IndexOf(FileSystem.directorySeparator, StringComparison.Ordinal) + 1))
        .ToList();

    foreach (var file in allFiles)
    {
      var game = Read(file, cryptoKey: SaveGame.CryptoKey);

      var displayString =
        game.time.ToString(CultureInfo.CurrentCulture) +
        "\tNode: " + (game.player.completedNodeLocations.Count + 1) +
        "\tMap:" + (game.player.completedMapCount + 1) +
        "\tSeed: " + game.randomSeed;

      list.Add(new Tuple<string, int>(displayString, list.Count));
    }

    Console.WriteLine("\nPlease select a save slot:");
    foreach (var (item1, item2) in list)
    {
      Console.WriteLine(item2 + ") " + item1);
    }

    var choice = UserInput.GetInt();
    var saveFile = choice == 0 ? DateTime.Now.Ticks.ToString() : allFiles[choice - 1];
    return saveFile;
  }

  public static void ArchiveRun(string saveFilename)
  {
    Delete(saveFilename);

    //TODO - maintain the run's stats somewhere. 
  }

}