using MaM.Utilities;

namespace MaM
{
  internal static class Program
  {
    private const string gameConfigFile   = "gameconfig.json";
    private const string saveFilename     = null;

    private static void Main()
    {
      var gameConfig = FileIO.GetGameConfigFromFile(gameConfigFile);

      GameLogic.RunGame(saveFilename, gameConfig);
    }
  }
}
