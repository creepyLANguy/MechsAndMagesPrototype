using MaM.Definitions;
using System;
using System.Linq;
using MaM.Helpers;

namespace MaM.GameLogic
{
    class BattlePhases
  {
    public static void RunMulliganPhase(ref Player player, ref BattlePack battlePack)
    {
      var mulliganCost = 1;

      while (true)
      {
        ConsoleMessages.PrintHand(battlePack.hand.GetAllCardsInHand());

        ConsoleMessages.PrintMarket(battlePack.market.GetDisplayedCards_All());

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

          battlePack.Mulligan();

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

    public static void RunPlayCardsPhase(ref BattlePack battlePack, ref BattleTracker battleTracker)
    {
      while (battlePack.hand.GetAllCardsInHand().Count > 0)
      {
        ConsoleMessages.PromptToPlayCard(ref battlePack);

#if DEBUG
        var selection = 0;
#else
        var selection = UserInput.GetInt();
#endif
        
        var allCardsInHand = battlePack.hand.GetAllCardsInHand();

        if (selection < 0)
        {
          return;
        }
        else if (selection == 0)
        {
          foreach (var card in allCardsInHand)
          {
            ProcessCardEffects(card, ref battlePack, ref battleTracker);
          }
          battlePack.graveyard.AddRange(allCardsInHand);
          battlePack.hand.Clear();
        }
        else if (selection <= allCardsInHand.Count)
        {
          --selection;
          var selectedCard = allCardsInHand[selection];
          ProcessCardEffects(selectedCard, ref battlePack, ref battleTracker);
          battlePack.graveyard.Add(selectedCard);
          battlePack.hand.Remove_Single(selectedCard);
        }
      }
    }

    private static void ProcessCardEffects(Card card, ref BattlePack battlePack, ref BattleTracker battleTracker)
    {
      var powers = card.defaultActions.Where(x => x.Item1 == CardAttribute.P);
      battleTracker.power += powers.Sum(attack => attack.Item2);

      //TODO - process all card effects/attributes
    }
  }
}
