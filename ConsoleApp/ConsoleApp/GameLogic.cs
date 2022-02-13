using System;
using System.Collections.Generic;
using MaM.Readers;
using MaM.Utilities;

namespace MaM
{

  public static class GameLogic
  {
    public static void RunGame(
      string saveFile, 
      string cardsExcelFile,
      string bossesExcelFile,
      MapConfig mapConfig,
      EnemyConfig normalEnemyConfig,
      EnemyConfig eliteEnemyConfig,
      int journeyLength
    )
    {
      var cards = CardReader.GetCardsFromExcel(cardsExcelFile);

      var gameState = new GameState(DateTime.Now, Math.Abs((int)DateTime.Now.Ticks), null);
      if (string.IsNullOrEmpty(saveFile) == false)
      {
        gameState = FileIO.GetGameStateFromFile(saveFile, ref cards);
      }

      var random = new Random(gameState.randomSeed);

      var journey = GenerateJourney(journeyLength, bossesExcelFile, mapConfig, normalEnemyConfig, eliteEnemyConfig, ref cards, ref random);

      var player = gameState.player ?? GenerateNewPlayer();

      ContinueJourney(ref player, ref journey, ref cards, ref random);
    }

    private static Journey GenerateJourney(
      int journeyLength, 
      string bossesExcelFile, 
      MapConfig mapConfig, 
      EnemyConfig normalEnemyConfig, 
      EnemyConfig eliteEnemyConfig, 
      ref List<Card> cards, 
      ref Random random
      )
    {
      var bosses = BossReader.GetBossesFromExcel(bossesExcelFile, ref cards);

      var journey = JourneyGenerator.GenerateJourney(journeyLength, mapConfig, normalEnemyConfig, eliteEnemyConfig, ref bosses, cards, ref random);

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
    
    private static void ContinueJourney(ref Player player, ref Journey journey, ref List<Card> cards, ref Random random)
    {
      //TODO - implement
    }
  }
}
