namespace MaM.Definitions;

public static class SaveGame
{
  public static string SaveFileDirectory = "savegames" + FileSystem.directorySeparator;
  public const string GameConfigFilename = "gameconfig.json";
  //public const string CryptoKey = "嵵߬ꇄ寘汅浫䔜ꌰ";
  public const string CryptoKey = null;
  public const string SaveFileExtension = ".sav";
}