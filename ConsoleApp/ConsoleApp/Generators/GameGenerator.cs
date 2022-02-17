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

      var player = gameState.player ?? GenerateNewPlayer(gameConfig.playerConfig, gameConfig.initialSelections, random);

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

    private static Player GenerateNewPlayer(PlayerConfig playerConfig, List<InitialCardSelection> initialSelections, Random random)
    {
      var deckCardIds = PromptPlayerForInitialCardSelection(ref initialSelections, random);
      
      var deck = new List<Card>();
      CardReader.GetCardsFromIds(deckCardIds, ref deck);

      var player = new Player(
        false,
        GetPlayerName(),
        -1,
        -1,
        new List<Tuple<int, int>>(),
        -1,
        null,
        playerConfig.health,
        playerConfig.health,
        0,
        playerConfig.vision,
        playerConfig.awareness,
        playerConfig.insight,
        playerConfig.tradeRowSize,
        -1,
        -1,
        playerConfig.manna,
        playerConfig.handSize,
        -1,
        -1,
        deckCardIds,
        deck,
        null,
        null,
        null,
        null
      );

      return player;
    }

    private static string GetPlayerName()
    {
      //TODO - implement
      return "TestPlayer";
    }

    //Note, prolly important to pass a copy of random as we may be using it an indeterminate amount of times here.
    private static List<string> PromptPlayerForInitialCardSelections(ref List<InitialCardSelection> initialCardSelections, ref List<Card> cards, Random random)
    {
      var cardsSelected = new List<string>();

      foreach (var initialCardSelection in initialCardSelections)
      {
        //TODO - implement 
        //draw initialCardSelection.cardCount many cards randomly from the cards list 
        //user selects a card
        //add that card's id to the cardsSelected
        //remove that card from the main cards list
      }

      return cardsSelected;
    }
  }
}
