using System;
using System.Collections.Generic;
using MaM.Helpers;
using MaM.Readers;

namespace MaM.Generators
{
  public static class GameGenerator
  {
    public static GameContents GenerateGame(
      string saveFilename,
      GameConfig gameConfig
    )
    {
      var cards = CardReader.GetCardsFromExcel(gameConfig.cardsExcelFile);

      var gameState = new GameState(DateTime.Now, Math.Abs((int)DateTime.Now.Ticks), null);
      if (string.IsNullOrEmpty(saveFilename) == false)
      {
        gameState = FileHelper.GetGameStateFromFile(saveFilename, ref cards);
      }

      var random = new Random(gameState.randomSeed);

      var journey = GetJourney(
        gameConfig.journeyLength, 
        gameConfig.bossesExcelFile, 
        gameConfig.mapConfigs, 
        gameConfig.normalEnemyConfig, 
        gameConfig.eliteEnemyConfig, 
        ref cards, 
        ref random
        );

      var player = gameState.player ?? GenerateNewPlayer();

      var gameContents = new GameContents(player, journey, cards, random, gameState.randomSeed);

      return gameContents;
    }

    private static Journey GetJourney(
      int journeyLength, 
      string bossesExcelFile, 
      List<MapConfig> mapConfigs, 
      EnemyConfig normalEnemyConfig, 
      EnemyConfig eliteEnemyConfig, 
      ref List<Card> cards, 
      ref Random random
      )
    {
      var bosses = BossReader.GetBossesFromExcel(bossesExcelFile, ref cards);

      var journey = JourneyGenerator.GenerateJourney(journeyLength, mapConfigs, normalEnemyConfig, eliteEnemyConfig, ref bosses, cards, ref random);

#if DEBUG
      GraphVis.SaveMapsAsDotFiles(ref journey, false);
#endif

      return journey;
    }

    private static Player GenerateNewPlayer()
    {
      //TODO - implement
      return null;
    }
  }
}
