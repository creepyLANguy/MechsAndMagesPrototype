using MaM.Definitions;
using MaM.Enums;
using MaM.Helpers;
using static MaM.Enums.YesNoChoice;

namespace MaM.GameplayLogic;

class BattlePhases
{
  public static void RunMulliganPhase(ref Player player, ref BattlePack b)
  {
    var mulliganCost = 1;

    while (true)
    {
      Terminal.PrintHand(b.hand.GetAllCardsInHand());

      Terminal.PrintMarket(b.market.GetDisplayedCards_All());

      if (player.health - mulliganCost <= 0)
      {
        return;
      }

      Terminal.OfferMulligan(player.health, mulliganCost);

      var choice = (YesNoChoice) UserInput.GetInt((int?) NO);
      if (choice == NO)
      {
        break;
      }

      player.health -= mulliganCost;
      b.Mulligan();
      ++mulliganCost;
    }
  }

  public static PlayerTurnAction RunActionSelectionPhase(bool canRecruit)
  {
    Terminal.PromptForAction(canRecruit);
    return (PlayerTurnAction)UserInput.GetInt();
  }

  public static void RunPlayCardsPhase(ref BattlePack b)
  {
    while (b.hand.GetCurrentCount() > 0)
    {
      RunPlayCardsPhase_Helper(ref b);
    }
  }

  private static void RunPlayCardsPhase_Helper(ref BattlePack b)
  {
    var canPlayAll = b.hand.HasCardsWithOrderSensitiveEffects() == false;

    Terminal.PromptToPlayCard(ref b, canPlayAll);
      
    var selection = UserInput.GetInt(canPlayAll ? 0 : 1);
    switch (selection)
    {
      case < 0:
        return;
      case 0:
      {
        var allCardsInHand = b.hand.GetAllCardsInHand();
        b.field.AddRange(allCardsInHand);
        b.hand.Clear();
        for (var index = 0; index < b.field.Count; index++) //foreach may crash as battlefield changes due to card effects 
        {
          var card = b.field[index];
          CardEffects.Process(card, ref b);
        }

        break;
      }
      default:
      {
        if (selection <= b.hand.GetCurrentCount())
        {
          --selection;
          var selectedCard = b.hand.GetCardAtIndex(selection);
          b.field.Add(selectedCard);
          b.hand.Remove_Single(selectedCard);
          CardEffects.Process(selectedCard, ref b);
        }

        break;
      }
    }
  }
}