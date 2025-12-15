using MaM.Definitions;
using System;
using System.Collections.Generic;
using MaM.Menus;
using System.Linq;
using MaM.Enums;
using ConsoleTableExt;

namespace MaM.Helpers;
class Terminal
{
  private static string GetPrintableCardName(string cardName)
  {
    const int printableCardNameLength = 14;
    const char spacer = ' ';
    const string ellipsis = "...";

    return cardName.Length <= printableCardNameLength
      ? cardName.PadRight(printableCardNameLength, spacer)
      : cardName[..(1 + (printableCardNameLength - ellipsis.Length))] + ellipsis;
  }

  private static void PrintNode(Node node, int n)
  {
    //AL.
    //TODO - print properly as table.

    const string tab = "\t";

    var fightDetails =
      node.nodeType == NodeType.FIGHT
        ? tab + (((Fight)node).fightType + tab + ((Fight)node).guild)
        : "";

    var buff =
      n + ")" +
      tab + "[" + node.x + ", " + node.y + "]" +
      tab + node.nodeType +
      fightDetails +
      (node.isMystery ? tab + "MYSTERY" : string.Empty);

    Print("\n" + buff);
  }

  public static void StartBattle()
    => Print("\n\n[Start Battle]");

  public static void PrintTurnOwner(string name) 
    => Print("\n\nTurn Owner:\t" + name);

  public static void PrintBattleState()
  {
    PrintCombatantState(Battle.Player);
    PrintCombatantState(Battle.Enemy);
    return;

    static void PrintCombatantState(Combatant combatant)
    {
      var combatantName = combatant == Battle.Player ? "You" : "Enemy";

      var cols = new[] {"Life", "Power", "Manna", "Defense"};

      var row = new List<object>
        {combatant.health, combatant.power, combatant.manna, (combatant.isDefending ? "ACTIVE" : "NONE")};

      PrintAsTable(new List<List<object>> { row }, combatantName, cols);
      Print("\n");
    }
  }

  public static void PrintHand(List<Card> cards)
    => PrintCards(cards, "Your Hand");

  public static void PrintMarket(List<Card> cards)
    => PrintCards(cards, "The Market");

  public static void OfferMulligan(int playerHealth, int mulliganCost)
  {
    //AL.
    //TODO - print properly as table.

    var message =
      "Life : " + playerHealth + "\n" +
      "\nMulligan this hand and cycle the market by paying " + mulliganCost + " life?" +
      "\n" + YesNoChoice.YES.ToString("D") + ") " + nameof(YesNoChoice.YES).ToSentenceCase() +
      "\n" + YesNoChoice.NO.ToString("D") + ") " + nameof(YesNoChoice.NO).ToSentenceCase();
    Print("\n" + message);
  }

  public static void PromptForAction(bool canRecruit)
  {
    //AL.
    //TODO - print properly as table.

    var message = "\nSelect an action:";
    foreach (PlayerTurnAction turnAction in Enum.GetValues(typeof(PlayerTurnAction)))
    {
      if (turnAction == PlayerTurnAction.RECRUIT && canRecruit == false) continue;

      message += "\n" + turnAction.ToString("D") + ") " + turnAction.ToString().ToSentenceCase();
    }
    Print("\n" + message);
  }

  public static void PromptForSaveSlot(List<List<object>> list)
    => PrintAsTable(list,
        "Please Select A Save Slot",
        new[] { "#", "Date/Time", "Seed", "Map", "Node", "Health", "Player Name" });

  public static void ShowFilenameWasNull()
    => Print("\nfilename was null");
  
  private static void PrintCards(List<Card> cards, string title = "", bool showCosts = true)
  {
    Print("\n");

    var cols = new List<string>();
    cols.AddRange(new List<string> { "#", "Card Name", "Guild" });
    if (showCosts)
    {
      cols.AddRange(new List<string> { "Power Cost", "Manna Cost", "Total Cost" });
    }
    cols.AddRange(new List<string> { "Power", "Ability" });

    var list = new List<List<object>>();
    for (var index = 0; index < cards.Count; index++)
    {
      var card = cards[index];

      var abilityString = card.ability.ToString().ToSentenceCase();
      if (card.ability != CardAbility.NONE)
      {
        abilityString += ", " + card.abilityCount;
      }

      list.Add(new List<object> {(index + 1), GetPrintableCardName(card.name), card.guild});
      if (showCosts)
      {
        list[index].AddRange(new List<object>
        {
          ("" + card.powerCost).PadRight(3), 
          ("" + card.mannaCost).PadRight(3),
          ("" + card.powerCost + card.mannaCost).PadRight(3)
        });
      }
      list[index].AddRange(new List<object> { ("" + card.power).PadRight(2),abilityString });
    }

    PrintAsTable(list, title, cols.ToArray());

    Print("\n");
  }

  public static void ShowMainMenu()
    => PrintMenuAsTable(typeof(MainMenuItem), "Main Menu");

  public static void ShowExitMenu()
    => PrintMenuAsTable(typeof(YesNoChoice), "Are you sure you want to exit?");

  public static void PromptForName()
    => Print("\n\nEnter your name:");

  public static void PromptForCardDraft(ref List<Card> offeredCards)
    => PrintCards(offeredCards, "Select one of the following cards by specifying its number in the list");

  public static void ShowDeath(ref Journey journey)
  {
    Print("\n\nYOU DIED\nCompletion Percent : " + GetCompletionPercentage(ref journey) + "%");
    return;

    static double GetCompletionPercentage(ref Journey journey, int decimalPlaces = 0)
    {
      var completedNodes = 0;
      foreach (var map in journey.maps)
      {
        foreach (var node in map.nodes)
        {
          completedNodes += node is { isComplete: true } ? 1 : 0;
        }
      }

      var totalNodes = (double)journey.maps.Sum(map => map.height);

      var completedPercentage = completedNodes / totalNodes * 100;

      return Math.Round(completedPercentage, decimalPlaces);
    }
  }

  public static void ShowCompletedNode(int count, int mapHeight, int mapIndex, int mapsCount)
    => Print(
      "\n\n" +
      "Completed Node " + count +
      " of " + mapHeight +
      ", of Map " + (mapIndex + 1) +
      " of " + mapsCount);
  
  public static void ShowCompletedMap(int mapIndex)
    => Print("\n\nCompleted Map " + (mapIndex + 1));

  public static void ShowCompletedRun()
    => Print(
      "\n" +
      "\nCONGRATULATIONS!" +
      "\nRun completed." +
      "\n\nReturning to main menu...");

  public static void PromptForStartingNode(ref List<Node> firstRow)
  {
    //AL.
    //TODO - print properly as table.

    Print("\n\nPlease select your starting location:");
    var n = 0;
    foreach (var startingNode in firstRow)
    {
      PrintNode(startingNode, ++n);
    }
  }

  public static void PromptForNextNode(ref Node currentNode, ref Map map )
  {
    //AL.
    //TODO - print properly as table.

    Console.WriteLine("\nPlease select your next location:");
    var n = 0;
    foreach (var (x, y) in currentNode.destinations)
    {
      var node = map.nodes[x, y];
      PrintNode(node, ++n);
    }
  }

  public static void FileHelperSave(string filename)
    => Print("\n\nSaving " + filename);

  public static void FileHelperFolderNotFound(string folderName)
    => Print("\n\nError - could not find folder : " + folderName);

  public static void FileHelperDeleting(string filename)
    => Print("\n\nDeleting " + filename);

  public static void FileHelperDeleteFailed(string filename, string eMessage)
    => Print("\n\nFAILED TO DELETE FILE \'" + filename + "\' ON DRIVE!\n" + eMessage);

  public static void FileHelperDeleted()
    => Print("Deleted");

  public static void FileHelperWriteFailed(string filename, string eMessage)
    => Print("\n\nFAILED TO WRITE FILE \'" + filename + "\' TO DRIVE!\n" + eMessage);

  public static void FileHelperWritten()
    => Print("\nSaved");

  public static void PromptToPlayCard(bool canPlayAll)
  {
    //AL.
    //TODO - print properly as table. 

    var allCardsInHand = Battle.Hand.GetAllCardsInHand();

    Print("\n\nPlay a card by specifying its number in the list :");

    if (canPlayAll)
    {
      Print("\n\n0) PLAY ALL CARDS");
    }

    PrintCards(allCardsInHand, "Current Hand", false);
    Print("-1) SKIP TO NEXT PHASE");
  }

  public static void EnemyTurnActionBuff(int value)
    => Print("\n\nEnemy BUFFS for " + value);

  public static void EnemyTurnActionAttack(int threat)
    => Print("\n\nEnemy ATTACKS for " + threat);

  public static void EnemyTurnActionDefend()
    => Print("\n\nEnemy is DEFENDING");

  public static void EnemyTurnActionLeech(int threat)
    => Print("\n\nEnemy LEECHES for " + threat);

  public static void EnemyTurnActionPass()
    => Print("\n\nEnemy PASSES");

  public static void PromptToRecruit(List<Card> offeredCards)
  {
    //AL.
    //TODO - print properly as table.

    Print("\n\nSelect one of the following cards to recruit :");
    Print("\n\n0) SKIP TO NEXT PHASE");
    PrintCards(offeredCards);
  }

  public static void ShowRecruitFailed(Card card)
    => Print(
      "\n" +
      "\nYou could not recruit " + card.name +
      "\nPlease select another card and make sure you have the required amounts of power and manna.");

  public static void ShowRecruited(Card card)
    => Print("\n\nYou recruited : " + card.name);

  public static void PromptInvalidChoiceTryAgain()
    => Print("\n\nInvalid choice, please make sure your option exists in the list and try again...");

  public static void PrintFightResult(FightResult fightResult)
  {
    var message = "";
    switch (fightResult)
    {
      case FightResult.PLAYER_LOSE:
        message += "\nYou LOST the fight....";
        break;
      case FightResult.PLAYER_WIN:
        message += "You WON the fight!";
        break;
      case FightResult.NONE:
      default:
        break;
    }

    Print("\n" + message);
  }

  public static void PromptToChooseReward(List<Card> offeredRewards)
    => PrintCards(offeredRewards, "Choose a reward");

  public static void OfferCampsiteExchange()
  {
    //AL.
    //TODO - print properly as table.

    Print("\n\nGet some rest and choose one of the following:");
    Print("1) Sacrifice a card, gain double its total cost as life points.");
    Print("2) Choose a card and pay twice its total cost in life points.");
  }

  public static void PromptExchangeLifeForCard(ref List<Card> cards)
    => PrintCards(cards, "Select one of the following cards");

  public static void PromptExchangeCardForLife(List<Card> cards)
    => PrintCards(cards, "Select one of the following cards");
  
  public static void PrintHealth(int health)
    => Console.WriteLine("\nYour life :" + health);

  public static void PromptShun(List<Card> hand)
  {
    if (hand.Count == 0)
    {
      Print("\n\nNo cards in hand to Shun.");
      return;
    }

    PrintCards(hand, "\nShun a card from your hand");
  }

  public static void ShowDrawResult(List<Card> hand, bool successfullyDrew)
  {
    Print(
      successfullyDrew
        ? "\n\nDrew an additional card."
        : "\n\nDeck and graveyard empty - could not draw card.");

    PrintHand(hand);
  }  
  
  public static void ShowHealResult(int health)
  {
    Print("\n\nHealing applied.");
    PrintHealth(health);
  }

  public static void ShowCycleResult(List<Card> marketCardsDisplayed)
  {
    Print("\n\nCycled the market.");
    PrintMarket(marketCardsDisplayed);
  }

  public static void ShowStompFailed()
    => Print("\n\nStomp failed - hand and field likely empty.");

  public static void ShowStompResult(Card stompedCard, bool stompFromHand)
    => PrintCards(
      stompFromHand ? Battle.Hand.GetAllCardsInHand() : Battle.Field,
      "Stomped from " + (stompFromHand ? "Hand" : "Field") + " : " + GetPrintableCardName(stompedCard.name)
      );

  public static void PromptForManualSeed()
    => Print("\n\nEnter a positive integer to manually seed the journey, else enter 0 to use a random seed:");

  public static void Print<T>(T value)
    => Console.Write(value);

  public static void PrintAsTable(
    List<List<object>> list, 
    string title, 
    string[] columns, 
    ConsoleColor titleColour = ConsoleColor.Black, 
    ConsoleColor titleBackgroundColour = ConsoleColor.Gray, 
    TableAligntment tableAlignment = TableAligntment.Left)
      => ConsoleTableBuilder
          .From(list)
          .WithTitle(title, titleColour, titleBackgroundColour)
          .WithColumn(columns)
          .ExportAndWriteLine(tableAlignment);

  public static void PrintMenuAsTable(
    object optionsEnum,
    string title,
    ConsoleColor titleColour = ConsoleColor.Black,
    ConsoleColor titleBackgroundColour = ConsoleColor.Gray,
    TableAligntment tableAlignment = TableAligntment.Left)
  {
    var rows = new List<List<object>>();

    foreach (var value in Enum.GetValues((Type) optionsEnum))
    {
      rows.Add(new List<object>
      {
        Convert.ToInt32(value),
        value.ToString().ToSentenceCase()
      });
    }

    ConsoleTableBuilder
      .From(rows)
      .WithTitle(title, titleColour, titleBackgroundColour)
      .WithMinLength(new Dictionary<int, int> {{ 0, title.Length }})
      .ExportAndWriteLine(tableAlignment);
  }
}