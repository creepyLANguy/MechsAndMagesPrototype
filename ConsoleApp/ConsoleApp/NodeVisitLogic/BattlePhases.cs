using MaM.Definitions;
using MaM.Enums;
using MaM.Helpers;
using static MaM.Enums.YesNoChoice;

namespace MaM.NodeVisitLogic
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
        var choice = NO;
#else
      var choice = (YesNoChoice)UserInput.GetInt();
#endif
        if (choice == NO)
        {
          break;
        }

        player.health -= mulliganCost;
        battlePack.Mulligan();
        ++mulliganCost;
      }
    }

    public static PlayerTurnAction RunActionSelectionPhase(bool canRecruit)
    {
      ConsoleMessages.PromptForAction(canRecruit);
      return (PlayerTurnAction)UserInput.GetInt();
    }

    public static void RunPlayCardsPhase(ref BattlePack battlePack, ref BattleTracker battleTracker)
    {
      while (battlePack.hand.GetAllCardsInHand().Count > 0)
      {
        var canPlayAll = battlePack.hand.HasCardsWithOrderSensitiveEffects() == false;

        ConsoleMessages.PromptToPlayCard(ref battlePack, canPlayAll);

#if DEBUG
        var selection = canPlayAll ? 0 : 1;
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
      battleTracker.power += card.power;

      for (var i = 0; i < card.abilityCount; ++i)
      {
        switch (card.ability)
        {
          case CardAbility.DRAW:
            battlePack.hand.Draw_Single(ref battlePack.deck, ref battlePack.graveyard);
            break;
          case CardAbility.HEAL:
            battleTracker.playerHealth += 1;
            break;
          case CardAbility.STOMP:
            //TODO - process stomp ability
            break;
          case CardAbility.CYCLE:
            battlePack.market.Cycle();
            break;
          case CardAbility.SHUN:
            //TODO - process shun ability
            break;
          case CardAbility.NONE:
          default:
            break;
        }
      }
    }
  }
}
