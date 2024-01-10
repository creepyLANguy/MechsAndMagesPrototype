using MaM.Definitions;
using System;
using MaM.Helpers;

namespace MaM.GameLogic
{
  class BattlePhases
  {
    public static void RunMulliganPhase(ref Player player, ref BattlePack battlePack, ref Random random)
    {
      var mulliganCost = 1;

      while (true)
      {
        Console.WriteLine("\nYour Hand:");
        ConsoleMessages.PrintCards(battlePack.hand.GetAllCardsInHand());

        Console.WriteLine("\nThe Market:");
        ConsoleMessages.PrintCards(battlePack.market.GetDisplayedCards());

        if (player.health - mulliganCost <= 0)
        {
          return;
        }

        Console.WriteLine("Life : " + player.health);
        var message = "Mulligan this hand and cycle the market by paying " + mulliganCost + " life?\n1) Yes\n2) No";

#if DEBUG
        Console.WriteLine(message);
        const int choice = 0;
#else
      var choice = UserInput.GetInt(message);
#endif

        if (choice == 1)
        {
          player.health -= mulliganCost;

          battlePack.Mulligan(ref random);

          ++mulliganCost;
        }
        else
        {
          break;
        }
      }
    }

    public static TurnAction RunActionSelectionPhase()
    {
      var message = "\nSelect an action:";
      foreach (TurnAction turnAction in Enum.GetValues(typeof(TurnAction)))
      {
        message += "\n" + turnAction.ToString("D") + ") " + turnAction.ToString().ToSentenceCase();
      }
      return (TurnAction)UserInput.GetInt(message);
    }

    public static void RunPlayCardsPhase(ref BattlePack battlePack, ref TurnPools turnPools, ref Player player)
    {
      //TODO
    }
  }
}
