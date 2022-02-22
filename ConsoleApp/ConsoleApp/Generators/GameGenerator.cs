using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Helpers;
using MaM.Readers;

namespace MaM.Generators
{
  public static class GameGenerator
  {
    public static GameContents Generate(string gameConfigFilename, string saveFilename)
    {
      var gameConfig = FileHelper.GetGameConfigFromFile(gameConfigFilename);

      var cards = CardReader.GetCardsFromExcel(gameConfig.cardsExcelFile);

      var gameState = string.IsNullOrEmpty(saveFilename)
        ? new GameState(DateTime.Now, Math.Abs((int) DateTime.Now.Ticks), null)
        : FileHelper.GetGameStateFromFile(saveFilename, ref cards);

      return GenerateGameContents(ref gameConfig, ref gameState, ref cards);
    }
    
    private static GameContents GenerateGameContents(ref GameConfig gameConfig, ref GameState gameState, ref List<Card> cards)
    {
      var random = new Random(gameState.randomSeed);

      var player = gameState.player ?? GenerateNewPlayer(gameConfig.playerConfig, gameConfig.initialCardSelections, ref cards, random);

      var journey = GetJourney(
        gameConfig.journeyLength, 
        gameConfig.bossesExcelFile,
        gameConfig.enemyNamesExcelFile,
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
      string enemyNamesExcelFile,
      List<MapConfig> mapConfigs, 
      EnemyConfig normalEnemyConfig, 
      EnemyConfig eliteEnemyConfig,
      ref List<Card> cards, 
      ref Random random
      )
    {
      var bosses = BossReader.GetBossesFromExcel(bossesExcelFile, ref cards);

      var enemyNames = EnemyReader.GetEnemyNames(enemyNamesExcelFile);

      var journey = JourneyGenerator.GenerateJourney(journeyLength, mapConfigs, normalEnemyConfig, eliteEnemyConfig, ref enemyNames, ref bosses, cards, ref random);

#if DEBUG
      GraphVis.SaveMapsAsDotFiles(ref journey, false);
#endif

      return journey;
    }

    private static Player GenerateNewPlayer(PlayerConfig playerConfig, List<InitialCardSelection> initialSelections, ref List<Card> cards, Random random)
    {
      var playerName = GetPlayerName();

      var deck = cards.Where(card => card.guild == Guilds.Neutral).ToList();
      cards.RemoveAll(card => card.guild == Guilds.Neutral);

      var selectedCards = PromptPlayerForInitialCardSelections(ref initialSelections, ref cards, random);
      deck.AddRange(selectedCards);

      var deckIds = deck.Select(card => card.id).ToList();

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

      //AL.
#if DEBUG
      return "1";
#endif
      //

      return Console.ReadLine();
    }

    //Note, prolly important to pass a copy of random as in the future, with prior completion bonuses awarded, we may be using random an indeterminate amount of times here.
    private static List<Card> PromptPlayerForInitialCardSelections(ref List<InitialCardSelection> initialCardSelections, ref List<Card> cards, Random random)
    {
      var allSelectedCards = new List<Card>();

      foreach (var initialCardSelection in initialCardSelections)
      {
        cards.Shuffle(ref random);

        var costSpecificCards = cards.Where(card => card.cost >= initialCardSelection.minCost && card.cost <= initialCardSelection.maxCost);
        var offeredCards = costSpecificCards.Take(initialCardSelection.cardCount).ToList();

        var selectedCard = GetSelectedCard(ref offeredCards);

        allSelectedCards.Add(selectedCard);

        cards.Remove(selectedCard);
      }

      return allSelectedCards;
    }

    private static Card GetSelectedCard(ref List<Card> offeredCards)
    {
      const string tab = "\t";
      const string pipe = "|";

      Console.WriteLine(" \nSelect one of the following cards by specifying its number in the list :");
      for (var index = 0; index < offeredCards.Count; index++)
      {
        var card = offeredCards[index];

        Console.WriteLine(
          index + 1 + ")" +
           tab +
          GetPrintableCardName(card.name) +
          tab + tab + pipe + tab +
          "Manna Cost :" + card.cost +
          tab + pipe + tab +
          "Type:" + card.type.Key +
          tab + pipe + tab +
          "Guild:" + card.guild.Key
          );
      }

      //AL.
#if DEBUG
      return offeredCards[1];
#endif
      //

      var selectionIndex = int.Parse(Console.ReadLine() ?? string.Empty) - 1;

      return offeredCards[selectionIndex];
    }

    private static string GetPrintableCardName(string cardName)
    {
      const int printableLength = 14;
      const char spacer = ' ';
      const string ellipsis = "...";

      return cardName.Length <= printableLength 
        ? cardName.PadRight(printableLength, spacer)
        : cardName.Substring(0, 1 + (printableLength - ellipsis.Length)) + ellipsis;
    }
  }
}
