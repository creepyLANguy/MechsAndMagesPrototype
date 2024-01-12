using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Helpers;
using MaM.Readers;

namespace MaM.Generators;

public static class GameGenerator
{
  public static GameContents Generate(string gameConfigFilename, string saveFilename, string cryptoKey = null)
  {
    var gameConfig = GameConfigReader.GetGameConfigFromFile(gameConfigFilename);

    var cards = CardReader.GetCardsFromExcel(gameConfig.cardsExcelFile);

    var gameState = SaveGameHelper.IsLegit(saveFilename) 
      ? SaveGameHelper.Read(saveFilename, cards, cryptoKey) 
      : new GameState(DateTime.Now, UbiRandom.GetSeed(), null);

    return GenerateGameContents(ref gameConfig, ref gameState, ref cards);
  }
    
  private static GameContents GenerateGameContents(ref GameConfig gameConfig, ref GameState gameState, ref List<Card> cards)
  {
    var player = gameState.player ?? GenerateNewPlayer(gameConfig.playerConfig, gameConfig.initialCardSelections, ref cards);

    var journey = GetJourney(ref gameConfig);

    journey.currentMapIndex = player.completedMapCount;

    var gameContents = new GameContents(player, journey, cards, gameConfig.handSize, gameState.randomSeed);

    return gameContents;
  }

  private static Journey GetJourney(ref GameConfig gameConfig)
  {
    var normalEnemies = EnemyReader.GetEnemiesFromExcel(gameConfig.normalEnemiesExcelFile);
    var eliteEnemies = EnemyReader.GetEnemiesFromExcel(gameConfig.eliteEnemiesExcelFile);
    var bosses = EnemyReader.GetEnemiesFromExcel(gameConfig.bossesExcelFile);

    var journey = JourneyGenerator.GenerateJourney(
      gameConfig.journeyLength,
      gameConfig.mapConfigs,
      ref normalEnemies,
      ref eliteEnemies,
      ref bosses);

#if DEBUG
    GraphVis.SaveMapsAsDotFiles(ref journey, false);
#endif

    return journey;
  }

  private static Player GenerateNewPlayer(PlayerConfig playerConfig, List<InitialCardSelection> initialSelections, ref List<Card> cards)
  {
    var playerName = GetPlayerName();

    var deck = cards.Where(card => card.guild == Guild.NEUTRAL).ToList();
    var selectionSet = cards.Where(card => card.guild != Guild.NEUTRAL).ToList();

    var selectedCards = PromptPlayerForInitialCardSelections(ref initialSelections, ref selectionSet);
    deck.AddRange(selectedCards);

    var deckIds = deck.Select(card => card.id).ToList();

    var player = new Player(
      playerName,
      -1,
      -1,
      new List<Tuple<int, int>>(),
      0,
      playerConfig.health,
      deckIds,
      deck
    );

    return player;
  }

  private static string GetPlayerName()
  {
    ConsoleMessages.PromptForName();

#if DEBUG
    return "DebugPlayer";
#else
    return UserInput.Get();
#endif
  }

  //Note, prolly important to pass a copy of random as in the future, with prior completion bonuses awarded, we may be using random an indeterminate amount of times here.
  private static List<Card> PromptPlayerForInitialCardSelections(ref List<InitialCardSelection> initialCardSelections, ref List<Card> cards)
  {
    var allSelectedCards = new List<Card>();

    foreach (var initialCardSelection in initialCardSelections)
    {
      cards.Shuffle();

      var offeredCards = cards.Take(initialCardSelection.cardCount).ToList();

      var selectedCard = GetSelectedCard(ref offeredCards);

      allSelectedCards.Add(selectedCard);

      cards.Remove(selectedCard);
    }

    return allSelectedCards;
  }

  private static Card GetSelectedCard(ref List<Card> offeredCards)
  {
    ConsoleMessages.PromptForCardDraft(ref offeredCards);

#if DEBUG
    return offeredCards[0];
#else
    return offeredCards[int.Parse(UserInput.Get() ?? string.Empty) - 1];
#endif
  }
}