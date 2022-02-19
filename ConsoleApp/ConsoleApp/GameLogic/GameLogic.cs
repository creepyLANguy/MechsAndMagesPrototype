using MaM.Generators;

namespace MaM.GameLogic
{
  public static class Game
  {
    public static void Run(string gameConfigFilename, string saveFilename)
    {
      var gameContents = GameGenerator.Generate(gameConfigFilename, saveFilename);

      //TODO - implement
    }
  }
}
