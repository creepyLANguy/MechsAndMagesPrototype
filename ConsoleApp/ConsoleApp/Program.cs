using MaM.Utilities;

namespace MaM
{
  internal static class Program
  {
    private const string gameConfigFile   = "gameconfig.json";
    private const string saveFilename     = null; //TODO - test with full savefile containing complete player definition.

    private static void Main()
    {
      var gameConfig = FileIO.GetGameConfigFromFile(gameConfigFile);

      GameGeneration.Initiate(saveFilename, gameConfig);
    }
  }
}
