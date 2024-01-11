using MaM.Definitions;
using System;
using System.Collections.Generic;
using MaM.Menus;

namespace MaM.Helpers
{
  class ConsoleMessages
  {
    public static void StartBattle()
    {
      Console.WriteLine("\n[Start Battle]\n");
    }

    public static void Turn(string name)
    {
      Console.WriteLine("\n[Turn]\t\t" + name);
    }

    public static void PrintBattleState(ref Player player, ref Enemy enemy, ref int power)
    {
      Console.WriteLine();

      Console.WriteLine("Your Life:\t" + player.health);

      Console.WriteLine("Your Power:\t" + power);

      Console.WriteLine("Enemy Life:\t" + enemy.health);

      Console.WriteLine("Enemy Manna:\t" + enemy.manna);
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
      const char tab = '\t';
      const char pipe = '|';

      foreach (var card in cards)
      {
        Console.WriteLine(
          UserInput.GetPrintableCardName(card.name) +
          tab + tab + pipe + tab +
          "Manna Cost :" + card.cost +
          tab + pipe + tab +
          "Type:" + card.type +
          tab + pipe + tab +
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
  }
}
