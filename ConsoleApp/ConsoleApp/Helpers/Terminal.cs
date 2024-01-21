using MaM.Definitions;
using System;
using System.Collections.Generic;
using MaM.Menus;
using System.Linq;
using MaM.Enums;

namespace MaM.Helpers;
class Terminal
{
  private const char Tab = '\t';
  private const char Pipe = '|';

  private static string GetPrintableCardName(string cardName)
  {
    const int printableCardNameLength = 14;
    const char spacer = ' ';
    const string ellipsis = "...";

    return cardName.Length <= printableCardNameLength
      ? cardName.PadRight(printableCardNameLength, spacer)
      : cardName.Substring(0, 1 + (printableCardNameLength - ellipsis.Length)) + ellipsis;
  }

  public static void StartBattle()
  {
    Console.WriteLine("\n[Start Battle]\n");
  }

  public static void Turn(string name)
  {
    Console.WriteLine("\n[Turn]" + Tab + Tab + name);
  }

  public static void PrintBattleState(BattlePack b)
  {
    const string dashes = "---";

    Console.WriteLine();
    
    Console.WriteLine(dashes);

    Console.WriteLine("Your Life:" + Tab + b.player.health);

    Console.WriteLine("Your Power:" + Tab + b.player.power);

    Console.WriteLine("Your Manna:" + Tab + b.player.manna); 
    
    Console.WriteLine("Your Defense:" + Tab + (b.player.isDefending ? "ACTIVE" : "NONE"));

    Console.WriteLine(dashes);

    Console.WriteLine("Enemy Life:" + Tab + b.enemy.health);
    
    Console.WriteLine("Enemy Threat:" + Tab + b.enemy.power);

    Console.WriteLine("Enemy Defense:" + Tab + (b.enemy.isDefending ? "ACTIVE" : "NONE"));

    Console.WriteLine(dashes);
  }

  public static void PrintHand(List<Card> cards)
  {
    Console.WriteLine("\nYour Hand:");
    PrintCards(cards);
  }
    
  public static void PrintMarket(List<Card> cards)
  {
    Console.WriteLine("\nThe Market:");
    PrintCards(cards);
  }

  public static void OfferMulligan(int playerHealth, int mulliganCost)
  {
    var message = 
      "Life : " + playerHealth + 
      "\nMulligan this hand and cycle the market by paying " + mulliganCost + " life?" +
      "\n" + YesNoChoice.YES.ToString("D") + ") " + YesNoChoice.YES.ToString().ToSentenceCase() +
      "\n" + YesNoChoice.NO.ToString("D") + ") " + YesNoChoice.NO.ToString().ToSentenceCase();
    Console.WriteLine(message);
  }

  public static void PromptForAction(bool canRecruit)
  {
    var message = "\nSelect an action:";
    foreach (PlayerTurnAction turnAction in Enum.GetValues(typeof(PlayerTurnAction)))
    {
      if (turnAction == PlayerTurnAction.RECRUIT && canRecruit == false)
      {
        continue;
      }

      message += "\n" + turnAction.ToString("D") + ") " + turnAction.ToString().ToSentenceCase();
    }
    Console.WriteLine(message);
  }

  public static void PromptForSaveSlot(List<Tuple<string, int>> list)
  {
    Console.WriteLine("\nPlease select a save slot:");
    foreach (var (item1, item2) in list)
    {
      Console.WriteLine(item2 + ") " + item1);
    }
  }

  public static void ShowFilenameWasNull()
  {
    Console.WriteLine("filename was null");
  }

  private static void PrintCards(List<Card> cards)
  {
    for (var index = 0; index < cards.Count; index++)
    {
      var card = cards[index];

      var abilityString = card.ability.ToString().ToSentenceCase();
      if (card.ability != CardAbility.NONE)
      {
        abilityString += ", " + card.abilityCount;
      }
      
      Console.WriteLine(
        index + 1 + ")" +
        Tab +
        GetPrintableCardName(card.name) +
        Tab + Pipe +
        "Guild : " + card.guild +
        Tab + Pipe +
        "Power Cost : " + ("" + card.powerCost).PadRight(3) +
        Tab + Pipe +
        "Manna Cost : " + ("" + card.mannaCost).PadRight(3) +
        Tab + Pipe +
        "Total Cost : " + ("" + (card.powerCost + card.mannaCost)).PadRight(3) +
        Tab + Pipe +
        "Power : " + ("" + card.power).PadRight(2) +
        Tab + Pipe +
        "Ability : " + abilityString
      );
    }

    Console.WriteLine();
  }

  public static void ShowMainMenu()
  {
    var requestString =
      "\nMain Menu" +
      "\n" + MainMenuItem.PLAY.ToString("D") + ") " + MainMenuItem.PLAY.ToString().ToSentenceCase() +
      "\n" + MainMenuItem.EXIT.ToString("D") + ") " + MainMenuItem.EXIT.ToString().ToSentenceCase();
    Console.WriteLine(requestString);
  }    
    
  public static void ShowExitMenu()
  {
    var requestString =
      "\nAre you sure you want to exit?" +
      "\n" + YesNoChoice.YES.ToString("D") + ") " + YesNoChoice.YES.ToString().ToSentenceCase() +
      "\n" + YesNoChoice.NO.ToString("D") + ") " + YesNoChoice.NO.ToString().ToSentenceCase();
    Console.WriteLine(requestString);
  }

  public static void PromptForName()
  {
    Console.WriteLine("\nEnter your name:");
  }

  public static void PromptForCardDraft(ref List<Card> offeredCards)
  {
    Console.WriteLine(" \nSelect one of the following cards by specifying its number in the list :");
    PrintCards(offeredCards);
  }

  public static void ShowDeath(ref Journey journey)
  {
    Console.WriteLine("\nYOU DIED\nCompletion Percent : " + GetCompletionPercentage(ref journey) + "%");

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
  {
    Console.WriteLine("\nCompleted Node " + count + " of " + mapHeight + ", of Map " + (mapIndex + 1) + " of " + mapsCount);
  }

  public static void ShowCompletedMap(int mapIndex)
  {
    Console.WriteLine("\nCompleted Map " + (mapIndex + 1));
  }

  public static void ShowCompletedRun()
  {
    Console.WriteLine(
      "\nCONGRATULATIONS!" + 
      "\nRun completed." +
      "\n\nReturning to main menu..."
    );
  }

  public static void PromptForStartingNode(ref List<Node> firstRow)
  {
    Console.WriteLine("\nPlease select your starting location:");
    var n = 0;
    foreach (var node in firstRow)
    {
      Console.WriteLine(
        ++n + ")" +
        Tab + "[" + node.x + ", " + node.y + "]" +
        Tab + node.nodeType + (node.isMystery ? "_Mystery" : string.Empty) + 
        (node.nodeType == NodeType.FIGHT ? Tab + ((Fight)node).guild : "")
      );
    }
  }

  public static void PromptForNextNode(ref Node currentNode, ref Map map )
  {
    Console.WriteLine("\nPlease select your next location:");
    var n = 0;
    foreach (var (x, y) in currentNode.destinations)
    {
      var node = map.nodes[x, y];
      Console.WriteLine(
        ++n + ")" +
        Tab + "[" + node.x + ", " + node.y + "]" +
        Tab + node.nodeType + (node.isMystery ? "_Mystery" : string.Empty)
      );
    }
  }

  public static void FileHelperSave(string filename)
  {
    Console.WriteLine("\nSaving " + filename);
  }

  public static void FileHelperFolderNotFound(string folderName)
  {
    Console.WriteLine("\nError - could not find folder : " + folderName);
  }

  public static void FileHelperDeleting(string filename)
  {
    Console.WriteLine("\nDeleting " + filename);
  }

  public static void FileHelperDeleteFailed(string filename, string eMessage)
  {
    Console.WriteLine("\nFAILED TO DELETE FILE \'" + filename + "\' ON DRIVE!");
    Console.WriteLine(eMessage);
  }

  public static void FileHelperDeleted()
  {
    Console.WriteLine("Deleted");
  }

  public static void FileHelperWriteFailed(string filename, string eMessage)
  {
    Console.WriteLine("\nFAILED TO WRITE FILE \'" + filename + "\' TO DRIVE!");
    Console.WriteLine(eMessage);
  }

  public static void FileHelperWritten()
  {
    Console.WriteLine("Saved");
  }

  public static void PromptToPlayCard(ref BattlePack b, bool canPlayAll)
  {
    var allCardsInHand = b.hand.GetAllCardsInHand();

    Console.WriteLine(" \nSelect one of the following cards by specifying its number in the list :");

    if (canPlayAll)
    {
      Console.WriteLine("0) PLAY ALL CARDS");
    }

    PrintCards(allCardsInHand);
    Console.WriteLine("-1) SKIP TO NEXT PHASE");
  }

  public static void EnemyTurnActionBuff(int value)
  {
    Console.WriteLine("\nEnemy BUFFS for " + value);
  }

  public static void EnemyTurnActionAttack(int threat)
  {
    Console.WriteLine("\nEnemy ATTACKS for " + threat);
  }

  public static void EnemyTurnActionDefend()
  {
    Console.WriteLine("\nEnemy is DEFENDING");
  }

  public static void EnemyTurnActionLeech(int threat)
  {
    Console.WriteLine("\nEnemy LEECHES for " + threat);
  }

  public static void EnemyTurnActionPass()
  {
    Console.WriteLine("\nEnemy PASSES");
  }

  public static void PromptToRecruit(List<Card> offeredCards)
  {
    Console.WriteLine("\nSelect one of the following cards to recruit :");
    Console.WriteLine("\n0) SKIP TO NEXT PHASE");
    PrintCards(offeredCards);
  }

  public static void ShowRecruitFailed(Card card)
  {
    Console.WriteLine("\nYou could not recruit " + card.name);
    Console.WriteLine("Please select another card and make sure you have the required amounts of power and manna.");
  }

  public static void PromptInvalidChoiceTryAgain()
  {
    Console.WriteLine("\nInvalid choice, please make sure your option exists in the list and try again...");
  }

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
    Console.WriteLine(message);
  }

  public static void PromptToChooseReward(List<Card> offeredRewards)
  {
    Console.WriteLine("\nChoose a reward: ");
    PrintCards(offeredRewards);
  }

  public static void OfferCampsiteExchange()
  {
    Console.WriteLine("Get some rest and choose one of the following:");
    Console.WriteLine("1) Sacrifice a card, gain double its total cost as life points.");
    Console.WriteLine("2) Choose a card and pay twice its total cost in life points.");
  }  
  
  public static void PromptExchangeLifeForCard(ref List<Card> cards)
  {
    Console.WriteLine("\nSelect one of the following cards :");
    PrintCards(cards);
  }  
  
  public static void PromptExchangeCardForLife(List<Card> cards)
  {
    Console.WriteLine("\nSelect one of the following cards :");
    PrintCards(cards);
  }

  public static void PrintHealth(int health)
  {
    Console.WriteLine("\nYour health :" + health);
  }

  public static void PromptShun(List<Card> hand)
  {
    if (hand.Count == 0)
    {
      Console.WriteLine("\nNo cards in hand to Shun.");
      return;
    }

    Console.WriteLine("\nShun a card from your hand: ");
    PrintCards(hand);
  }

  public static void ShowDrawResult(List<Card> hand, bool successfullyDrew)
  {
    Console.WriteLine(
      successfullyDrew
      ? "\nDrew an additional card."
      : "\nDeck and graveyard empty - could not draw card.");

    PrintHand(hand);
  }  
  
  public static void ShowHealResult(int health)
  {
    Console.WriteLine("\nHealing applied.");
    PrintHealth(health);
  }

  public static void ShowCycleResult(List<Card> marketCardsDisplayed)
  {
    Console.WriteLine("\nCycled the market.");
    PrintMarket(marketCardsDisplayed);
  }

  public static void ShowStompResult(ref BattlePack b, Card stompedCard, bool stompFromHand)
  {
    var location = stompFromHand ? "Hand" : "Field";
    var cardName = GetPrintableCardName(stompedCard.name);
    Console.WriteLine("\nStomped from " + location + " : " + cardName);
    PrintCards(stompFromHand ? b.hand.GetAllCardsInHand() : b.field);
  }

  public static void PromptForManualSeed()
  {
    Console.WriteLine("\nEnter a positive integer to manually seed the journey, else enter 0 to use a random seed:");
  }

  public static void Print<T>(T value)
  {
    Console.WriteLine(value);
  }
}