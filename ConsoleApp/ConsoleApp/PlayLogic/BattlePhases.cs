using MaM.Definitions;
using MaM.Enums;
using MaM.Helpers;
using static MaM.Enums.YesNoChoice;

namespace MaM.PlayLogic;

class BattlePhases
{
  public static void RunMulliganPhase(ref Player player)
  {
    var mulliganCost = 1;

    while (true)
    {
      Terminal.PrintHand(Battle.Hand.GetAllCardsInHand());

      Terminal.PrintMarket(Battle.Market.GetDisplayedCards_All());

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
      Battle.Mulligan();
      ++mulliganCost;
    }
  }

  public static PlayerTurnAction RunActionSelectionPhase(bool canRecruit)
  {
    Terminal.PromptForAction(canRecruit);
    return (PlayerTurnAction)UserInput.GetInt();
  }

  public static void RunPlayCardsPhase()
  {
    while (Battle.Hand.GetCurrentCount() > 0)
    {
      RunPlayCardsPhase_Helper();
    }
  }

  private static void RunPlayCardsPhase_Helper()
  {
    var canPlayAll = Battle.Hand.HasCardsWithOrderSensitiveEffects() == false;

    Terminal.PromptToPlayCard(canPlayAll);
      
    var selection = UserInput.GetInt(canPlayAll ? 0 : 1);
    switch (selection)
    {
      case < 0:
        return;
      case 0:
      {
        var allCardsInHand = Battle.Hand.GetAllCardsInHand();
        Battle.Field.AddRange(allCardsInHand);
        Battle.Hand.Clear();
        for (var index = 0; index < Battle.Field.Count; index++) //foreach may crash as battlefield changes due to card effects 
        {
          var card = Battle.Field[index];
          CardEffects.Process(card);
        }

        break;
      }
      default:
      {
        if (selection <= Battle.Hand.GetCurrentCount())
        {
          --selection;
          var selectedCard = Battle.Hand.GetCardAtIndex(selection);
          Battle.Field.Add(selectedCard);
          Battle.Hand.Remove_Single(selectedCard);
          CardEffects.Process(selectedCard);
        }

        break;
      }
    }
  }
}