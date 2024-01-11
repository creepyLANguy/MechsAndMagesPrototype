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
        ConsoleMessages.PrintHand(battlePack.hand.GetAllCardsInHand());

        ConsoleMessages.PrintMarket(battlePack.market.GetDisplayedCards());

        if (player.health - mulliganCost <= 0)
        {
          return;
        }

        ConsoleMessages.OfferMulligan(player.health, mulliganCost);
#if DEBUG
        const int choice = 0;
#else
      var choice = UserInput.GetInt();
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
      ConsoleMessages.PromptForAction();
      return (TurnAction)UserInput.GetInt();
    }

    public static void RunPlayCardsPhase(ref BattlePack battlePack, ref Player player, ref int power)
    {
      //TODO
    }
  }
}
