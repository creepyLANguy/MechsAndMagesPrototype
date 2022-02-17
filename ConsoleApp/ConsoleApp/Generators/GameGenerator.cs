using System;
using System.Collections.Generic;
using System.Linq;
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

      var gameState = string.IsNullOrEmpty(saveFilename) ? 
        new GameState(DateTime.Now, Math.Abs((int)DateTime.Now.Ticks), null) :
        FileHelper.GetGameStateFromFile(saveFilename, ref cards);
      
      var random = new Random(gameState.randomSeed);

      var player = gameState.player ?? GenerateNewPlayer(gameConfig.playerConfig, gameConfig.initialCardSelections, ref cards, random);

      var journey = GetJourney(
        gameConfig.journeyLength, 
        gameConfig.bossesExcelFile, 
        gameConfig.mapConfigs, 
        gameConfig.normalEnemyConfig, 
        gameConfig.eliteEnemyConfig, 
        ref cards, 
        ref random
        );

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

    private static Player GenerateNewPlayer(PlayerConfig playerConfig, List<InitialCardSelection> initialSelections, ref List<Card> cards, Random random)
    {
      var playerName = GetPlayerName();

      var deck = cards.Where(card => card.Guild == Guilds.Neutral).ToList();
      cards.RemoveAll(card => card.Guild == Guilds.Neutral);

      var selectedCards = PromptPlayerForInitialCardSelections(ref initialSelections, ref cards, random);
      deck.AddRange(selectedCards);

      var deckIds = deck.Select(card => card.Id).ToList();

      var player = new Player(
        false,
        playerName,
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
        deckIds,
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
      Console.WriteLine("Enter your name:");
      return Console.ReadLine();
    }

    //Note, prolly important to pass a copy of random as in the future, with prior completion bonuses awarded, we may be using random an indeterminate amount of times here.
    private static List<Card> PromptPlayerForInitialCardSelections(ref List<InitialCardSelection> initialCardSelections, ref List<Card> cards, Random random)
    {
      var allSelectedCards = new List<Card>();

      foreach (var initialCardSelection in initialCardSelections)
      {
        cards.Shuffle(ref random);

        var costSpecificCards = cards.Where(card => card.Cost >= initialCardSelection.minCost && card.Cost <= initialCardSelection.maxCost);
        var offeredCards = costSpecificCards.Take(initialCardSelection.cardCount).ToList();

        var selectedCard = GetSelectedCard(ref offeredCards);

        allSelectedCards.Add(selectedCard);

        cards.Remove(selectedCard);
      }

      return allSelectedCards;
    }

    private static Card GetSelectedCard(ref List<Card> offeredCards)
    {
      Console.WriteLine("Select one of the following cards by specifying its number in the list : ");
      for (var index = 0; index < offeredCards.Count; index++)
      {
        var card = offeredCards[index];
        Console.WriteLine((index+1) + ")" + "\t" + card.Name + "\t\t|\t" + "Mana Cost :" + card.Cost + "\t|\t" + "Type:" + card.Type.Key + "\t|\t" + "Guild:" + card.Guild.Key);
      }

      var selectionIndex = int.Parse(Console.ReadLine()) - 1;

      return offeredCards[selectionIndex];
    }
  }
}
