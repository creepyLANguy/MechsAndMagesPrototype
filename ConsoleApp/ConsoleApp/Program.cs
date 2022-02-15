using MaM.Generators;
using MaM.Helpers;

namespace MaM
{
  internal static class Program
  {
    private const string gameConfigFile   = "gameconfig.json";
    private const string saveFilename     = null; //TODO - test with full savefile containing complete player definition.

    private static void Main()
    {
      var gameConfig = FileHelper.GetGameConfigFromFile(gameConfigFile);

      var gameContents = GameGenerator.GenerateGame(saveFilename, gameConfig);

      GameLogic.Run(ref gameContents);
    }
  }
}
