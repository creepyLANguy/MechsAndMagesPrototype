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

  public static void PrintBattleState(ref Player player, ref Enemy enemy, ref int power, ref int manna)
  {
    Console.WriteLine();

    Console.WriteLine("Your Life:\t" + player.health);

    Console.WriteLine("Your Power:\t" + power);

    Console.WriteLine("Enemy Life:\t" + enemy.health);

    Console.WriteLine("Enemy Manna:\t" + manna);
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
    foreach (TurnAction turnAction in Enum.GetValues(typeof(TurnAction)))
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
        "Manna Cost :" + card.cost +
        Tab + Pipe + Tab +
        "Type:" + card.type +
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
        "Manna Cost :" + card.cost +
        Tab + Pipe + Tab +
        "Type:" + card.type +
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
}