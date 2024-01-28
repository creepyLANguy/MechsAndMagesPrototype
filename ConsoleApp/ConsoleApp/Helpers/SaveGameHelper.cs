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

    var cardIds = gameState.player.GetDeckCardIds();
    var deck = CardReader.GetCardsFromIds(cardIds, ref cards);

    gameState.player.SetDeck(deck);

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
      Terminal.ShowFilenameWasNull();
      return false;
    }

    var content = ObjectToJson(gameState, true);

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
      Terminal.ShowFilenameWasNull();
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
        .Select(file => file[(file.IndexOf(Path.DirectorySeparatorChar, StringComparison.Ordinal) + 1)..])
        .ToList();

    foreach (var file in allFiles)
    {
      var game = Read(file, cryptoKey: SaveGame.CryptoKey);

      var displayString =
        game.time.ToString(CultureInfo.CurrentCulture) +
        //"\t\tSeed: " + game.randomSeed +
        "\t\tMap: " + (game.player.completedMapCount + 1) +
        "\t\tNode: " + (game.player.completedNodeLocations.Count + 1) +
        "\t\tHealth: " + game.player.health + 
        "\t\t" + game.player.name;

      list.Add(new Tuple<string, int>(displayString, list.Count));
    }

    Terminal.PromptForSaveSlot(list);

    var choice = UserInput.GetInt(0);
    var saveFile = (choice == 0 ? DateTime.Now.Ticks + SaveGame.SaveFileExtension : allFiles[choice - 1]);
    return saveFile;
  }

  public static void ArchiveRun(string saveFilename)
  {
    Delete(saveFilename);
  }

}