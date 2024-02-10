using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Enums;
using MaM.Helpers;
using MaM.Readers;

namespace MaM.Generators;

public static class GameGenerator
{
  public static void GenerateDebugMaps()
  {
    var gameConfig = GameConfigReader.GetGameConfigFromFile(SaveGame.GameConfigFilename);
    GetJourney(ref gameConfig);
  }

  public static GameContents Generate(string gameConfigFilename, string saveFilename, string cryptoKey = null)
  {
    var gameConfig = GameConfigReader.GetGameConfigFromFile(gameConfigFilename);

    var cards = CardReader.GetCardsFromExcel(gameConfig.cardsExcelFile);

    var isLegitSaveFile = SaveGameHelper.IsLegit(saveFilename);
    if (isLegitSaveFile == false)
    {
      PromptForManualSeed();
    }
    
    var gameState = isLegitSaveFile 
      ? SaveGameHelper.Read(saveFilename, cards, cryptoKey) 
      : new GameState(DateTime.Now, UbiRandom.GetCurrentSeed(), null);

    return GenerateGameContents(ref gameConfig, ref gameState, ref cards);
  }
    
  private static GameContents GenerateGameContents(ref GameConfig gameConfig, ref GameState gameState, ref List<Card> cards)
  {
    var player = 
      gameState.player ?? 
      GenerateNewPlayer(gameConfig.playerConfig, gameConfig.initialCardSelectionSizes, ref cards);

    var journey = GetJourney(ref gameConfig);

    journey.currentMapIndex = player.completedMapCount;

    var gameContents = 
      new GameContents(player, journey, cards, gameConfig.handSize, gameState.randomSeed);

    return gameContents;
  }

  private static Journey GetJourney(ref GameConfig gameConfig)
  {
    var normalEnemies = 
      EnemyReader.GetEnemiesFromExcel(gameConfig.normalEnemiesExcelFile);
    
    var eliteEnemies = 
      EnemyReader.GetEnemiesFromExcel(gameConfig.eliteEnemiesExcelFile);
    
    var bosses = 
      EnemyReader.GetEnemiesFromExcel(gameConfig.bossesExcelFile);

    var journey = JourneyGenerator.GenerateJourney(
      gameConfig.journeyLength,
      gameConfig.mapConfigs,
      ref normalEnemies,
      ref eliteEnemies,
      ref bosses,
      gameConfig.campsiteCardsOnOfferCount);

#if DEBUG || GENERATEDEBUGMAPS
    GraphVis.SaveMapsAsDotFiles(ref journey, false);
#endif

    return journey;
  }

  private static Player GenerateNewPlayer(PlayerConfig playerConfig, List<int> initialCardSelectionSizes, ref List<Card> cards)
  {
    var playerName = GetPlayerName();

    var deck = cards.Where(card => card.guild == Guild.NEUTRAL).ToList();
    
    var selectionSet = cards.Where(card => card.guild != Guild.NEUTRAL).ToList();
    selectionSet.Shuffle();

    var selectedCards = PromptPlayerForInitialCardSelections(ref initialCardSelectionSizes, ref selectionSet);
    deck.AddRange(selectedCards);

    var player = new Player(
      playerName,
      -1,
      -1,
      new List<Tuple<int, int>>(),
      0,
      playerConfig.health,
      deck
    );

    return player;
  }

  private static string GetPlayerName()
  {
    Terminal.PromptForName();
    return UserInput.GetString("DebugPlayer");
  }

  //Note, prolly important to pass a copy of random as in the future, with prior completion bonuses awarded, we may be using random an indeterminate amount of times here.
  private static List<Card> PromptPlayerForInitialCardSelections(ref List<int> initialCardSelectionSizes, ref List<Card> cards)
  {
    var allSelectedCards = new List<Card>();

    foreach (var selectionSize in initialCardSelectionSizes)
    {
      cards.Shuffle();

      var offeredCards = cards.Take(selectionSize).ToList();

      var selectedCard = GetSelectedCard(ref offeredCards);

      allSelectedCards.Add(selectedCard);

      cards.Remove(selectedCard);
    }

    return allSelectedCards;
  }

  private static Card GetSelectedCard(ref List<Card> offeredCards)
  {
    Terminal.PromptForCardDraft(ref offeredCards);
    return offeredCards[UserInput.GetInt(1) - 1];
  }

  private static void PromptForManualSeed()
  {
#if DEBUG
    return;
#endif

    Terminal.PromptForManualSeed();
    var seed = UserInput.GetInt(0);
    if (seed > 0)
    {
      UbiRandom.ForceInit(seed);
    }
  }
}