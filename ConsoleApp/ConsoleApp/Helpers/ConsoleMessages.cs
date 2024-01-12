using MaM.Definitions;
using System;
using System.Collections.Generic;
using MaM.Menus;
using System.Linq;

namespace MaM.Helpers;
class ConsoleMessages
{
  private const char Tab = '\t';
  private const char Pipe = '|';

  public static void StartBattle()
  {
    Console.WriteLine("\n[Start Battle]\n");
  }

  public static void Turn(string name)
  {
    Console.WriteLine("\n[Turn]\t\t" + name);
  }

  public static void PrintBattleState(BattleTracker b)
  {
    Console.WriteLine();

    Console.WriteLine("Your Life:\t" + b.playerHealth);

    Console.WriteLine("Your Power:\t" + b.power);

    Console.WriteLine("Your Manna:\t" + b.manna); 
    
    Console.WriteLine("Enemy Life:\t" + b.enemyHealth);
    
    Console.WriteLine("Enemy Threat:\t" + b.threat);

    Console.WriteLine("Enemy Defense:\t" + (b.enemyIsDefending ? "ACTIVE" : "NONE"));
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
      "\n1) Yes" +
      "\n2) No";
    Console.WriteLine(message);
  }

  public static void PromptForAction()
  {
    var message = "\nSelect an action:";
    foreach (PlayerTurnAction turnAction in Enum.GetValues(typeof(PlayerTurnAction)))
    {
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

  public static void FilenameWasNull()
  {
    Console.WriteLine("filename was null");
  }

  private static void PrintCards(List<Card> cards)
  {
    foreach (var card in cards)
    {
      Console.WriteLine(
        UserInput.GetPrintableCardName(card.name) +
        Tab + Tab + Pipe + Tab +
        "Power Cost :" + card.powerCost+
        Tab + Pipe + Tab +
        "Manna Cost :" + card.mannaCost+
        Tab + Pipe + Tab +
        "Guild:" + card.guild
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
      "\n" + ExitMenuItem.YES.ToString("D") + ") " + ExitMenuItem.YES.ToString().ToSentenceCase() +
      "\n" + ExitMenuItem.NO.ToString("D") + ") " + ExitMenuItem.NO.ToString().ToSentenceCase();
    Console.WriteLine(requestString);
  }

  public static void PromptForName()
  {
    Console.WriteLine("\nEnter your name:");
  }

  public static void PromptForCardDraft(ref List<Card> offeredCards)
  {
    Console.WriteLine(" \nSelect one of the following cards by specifying its number in the list :");
    for (var index = 0; index < offeredCards.Count; index++)
    {
      var card = offeredCards[index];

      Console.WriteLine(
        index + 1 + ")" +
        Tab +
        UserInput.GetPrintableCardName(card.name) +
        Tab + Tab + Pipe + Tab +
        "Power Cost :" + card.powerCost +
        Tab + Pipe + Tab +
        "Manna Cost :" + card.mannaCost +
        Tab + Pipe + Tab +
        "Guild:" + card.guild
      );
    }
  }
  public static void Death(ref Journey journey)
  {
    Console.WriteLine("\nYOU DIED\nCompletion Percent : " + GetCompletionPercentage(ref journey) + "%");

    double GetCompletionPercentage(ref Journey journey, int decimalPlaces = 0)
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

  public static void CompletedNode(int count, int mapHeight, int mapIndex, int mapsCount)
  {
    Console.WriteLine("\nCompleted Node " + count + " of " + mapHeight + ", of Map " + (mapIndex + 1) + " of " + mapsCount);
  }

  public static void CompletedMap(int mapIndex)
  {
    Console.WriteLine("\nCompleted Map " + (mapIndex + 1));
  }

  public static void CompletedRun()
  {
    Console.WriteLine(
      "\nCongratulations!" + 
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
        "\t[" + node.x + ", " + node.y + "]" +
        "\t" + node.nodeType + (node.isMystery ? "_Mystery" : string.Empty)
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
        "\t[" + node.x + ", " + node.y + "]" +
        "\t" + node.nodeType + (node.isMystery ? "_Mystery" : string.Empty)
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

  public static void PromptToPlayCard(ref BattlePack battlePack)
  {
    var allCardsInHand = battlePack.hand.GetAllCardsInHand();

    Console.WriteLine(" \nSelect one of the following cards by specifying its number in the list :");
    Console.WriteLine("0) PLAY ALL CARDS");

    for (var index = 0; index < allCardsInHand.Count; index++)
    {
      var card = allCardsInHand[index];

      Console.WriteLine(
        index + 1 + ")" +
        Tab +
        UserInput.GetPrintableCardName(card.name) +
        Tab + Tab + Pipe + Tab +
        "Cost :" + card.powerCost +
        Tab + Pipe + Tab +
        "Power Cost :" + card.powerCost +
        Tab + Pipe + Tab +
        "Manna Cost :" + card.mannaCost +
        Tab + Pipe + Tab +
        "Guild:" + card.guild
      );
    }

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
}