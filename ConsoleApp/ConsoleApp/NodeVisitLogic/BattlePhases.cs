using System.Linq;
using MaM.Definitions;
using MaM.Enums;
using MaM.Helpers;
using static MaM.Enums.YesNoChoice;

namespace MaM.NodeVisitLogic
{
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
          foreach (var card in b.field)
          {
            ProcessCardEffects(card, ref b);
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
            ProcessCardEffects(selectedCard, ref b);
          }

          break;
        }
      }
    }

    private static void ProcessCardEffects(Card card, ref BattlePack b)
    {
      //Make sure this power gain happens first in case a card has to stomp itself.
      b.player.power += card.power;

      for (var i = 0; i < card.abilityCount; ++i)
      {
        switch (card.ability)
        {
          case CardAbility.DRAW:
            PerformDraw(ref b);
            break;
          case CardAbility.HEAL:
            PerformHeal(ref b);
            break;
          case CardAbility.STOMP:
            PerformStomp(ref b, card);
            break;
          case CardAbility.CYCLE:
            PerformCycle(ref b);
            break;
          case CardAbility.SHUN:
            PerformShun(ref b);
            break;
          case CardAbility.NONE:
          default:
            break;
        }
      }
    }

    private static void PerformDraw(ref BattlePack b)
    {
      b.hand.Draw_Single(ref b.deck, ref b.graveyard);
      Terminal.ShowDrew(b.hand.GetAllCardsInHand());
    }

    private static void PerformHeal(ref BattlePack b)
    {
      b.player.health += 1;
      Terminal.ShowHealed(b.player.health);
    }

    private static void PerformStomp(ref BattlePack b, Card playedCard)
    {
      var stompedCard = playedCard;

      var cardsInHandCount = b.hand.GetCurrentCount();

      var coinToss = UbiRandom.Next(0, 2);
      var stompFromHand = coinToss == 1 && cardsInHandCount > 0;

      if (stompFromHand)
      {
        StompFromHand(ref b);
      }
      else
      {
        if (b.field.Count == 1) //playedCard is the only one on the field
        {
          if (cardsInHandCount > 0)
          {
            stompFromHand = true;
            StompFromHand(ref b);
          }
          else
          {
            StompSelf(ref b);
          }
        }
        else
        {
          StompFromField(ref b);
        }
      }
      
      Terminal.ShowStomped(ref b, stompedCard, stompFromHand);

      void StompSelf(ref BattlePack b)
      {
        b.field.Remove(stompedCard);
        b.scrapheap.Add(stompedCard);
      }

      void StompFromHand(ref BattlePack b)
      {
        var randomIndex = UbiRandom.Next(0, cardsInHandCount);
        stompedCard = b.hand.GetCardAtIndex(randomIndex);
        b.hand.Remove_Single(stompedCard);
        b.scrapheap.Add(stompedCard);
      }      
      
      void StompFromField(ref BattlePack b)
      {
        var stompableCardsOnField = b.field.Where(card => card.id != playedCard.id).ToList();
        var randomIndex = UbiRandom.Next(0, stompableCardsOnField.Count);
        stompedCard = stompableCardsOnField[randomIndex];
        b.field.Remove(stompedCard);
        b.scrapheap.Add(stompedCard);
      }
    }

    private static void PerformCycle(ref BattlePack b)
    {
      b.market.Cycle();
      Terminal.ShowCycled(b.market.GetDisplayedCards_All());
    }

    private static void PerformShun(ref BattlePack b)
    {
      Terminal.PromptShun(b.hand.GetAllCardsInHand());

      if (b.hand.GetCurrentCount() == 0)
      {
        return;
      }
      
      var selection = UserInput.GetInt(1) - 1;
      var selectedCard = b.hand.GetCardAtIndex(selection);
      b.scrapheap.Add(selectedCard);
      b.hand.Remove_Single(selectedCard);

      PerformDraw(ref b);
    }
  }
}
