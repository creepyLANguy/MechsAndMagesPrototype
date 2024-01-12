using MaM.Definitions;
using System;
using System.Linq;
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

    public static PlayerTurnAction RunActionSelectionPhase()
    {
      ConsoleMessages.PromptForAction();
      return (PlayerTurnAction)UserInput.GetInt();
    }

    public static void RunPlayCardsPhase(ref BattlePack battlePack, ref Player player, ref int power, ref int manna)
    {
      while (battlePack.hand.GetAllCardsInHand().Count > 0)
      {
        ConsoleMessages.PromptToPlayCard(ref battlePack);

        var selection = UserInput.GetInt();

        var allCardsInHand = battlePack.hand.GetAllCardsInHand();

        if (selection < 0)
        {
          return;
        }
        else if (selection == 0)
        {
          foreach (var card in allCardsInHand)
          {
            ProcessCardEffects(card, ref battlePack, ref player, ref power, ref manna);
          }
          battlePack.graveyard.AddRange(allCardsInHand);
          battlePack.hand.Clear();
        }
        else if (selection <= allCardsInHand.Count)
        {
          --selection;
          var selectedCard = allCardsInHand[selection];
          ProcessCardEffects(selectedCard, ref battlePack, ref player, ref power, ref manna);
          battlePack.graveyard.Add(selectedCard);
          battlePack.hand.Remove_Single(selectedCard);
        }
      }
    }

    private static void ProcessCardEffects(Card card, ref BattlePack battlePack, ref Player player, ref int power, ref int manna)
    {
      var powers = card.defaultActions.Where(x => x.Item1 == CardAttribute.P);
      power += powers.Sum(attack => attack.Item2);

      //TODO - process all card effects/attributes
    }
  }
}
