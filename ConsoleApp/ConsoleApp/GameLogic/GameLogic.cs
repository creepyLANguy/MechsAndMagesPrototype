using MaM.Generators;

namespace MaM.GameLogic
{
  public static class Game
  {
    public static void Run(string gameConfigFilename, string saveFilename, string cryptoKey = null)
    {
      var gameContents = GameGenerator.Generate(gameConfigFilename, saveFilename, cryptoKey);

      //TODO - implement
    }
  }
}
