using System;
using MaM.Definitions;
using MaM.Generators;
using MaM.Helpers;

namespace MaM.GameLogic
{
  public static class Game
  {
    public static void Run(string gameConfigFilename, string saveFilename, string cryptoKey = null)
    {
      var gameContents = GameGenerator.Generate(gameConfigFilename, saveFilename, cryptoKey);

      var gameState = new GameState(DateTime.Now, gameContents.seed, gameContents.player);
      SaveFileHelper.PromptUserToSaveGame(ref gameState, cryptoKey);

      //TODO - implement

    }
  }
}
