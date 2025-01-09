using System.Linq;
using MaM.Definitions;
using MaM.Enums;
using MaM.Helpers;

namespace MaM.PlayLogic;

class CardEffects
{
  public static void Process(Card card)
  {
    //Make sure this power gain happens first in case a card has to stomp itself.
    Battle.Player.power += card.power;

    for (var i = 0; i < card.abilityCount; ++i)
    {
      switch (card.ability)
      {
        case CardAbility.DRAW:
          PerformDraw();
          break;
        case CardAbility.HEAL:
          PerformHeal();
          break;
        case CardAbility.STOMP:
          PerformStomp(card);
          break;
        case CardAbility.CYCLE:
          PerformCycle();
          break;
        case CardAbility.SHUN:
          PerformShun();
          break;
        case CardAbility.NONE:
        default:
          break;
      }
    }
  }

  private static void PerformDraw()
  {
    var drawResult = Battle.Hand.Draw_Single();
    Terminal.ShowDrawResult(Battle.Hand.GetAllCardsInHand(), drawResult);
  }

  private static void PerformHeal()
  {
    Battle.Player.health += 1;
    Terminal.ShowHealResult(Battle.Player.health);
  }

  private static void PerformStomp(Card playedCard)
  {
    var stompCandidate = playedCard;

    var cardsInHandCount = Battle.Hand.GetCurrentCount();

    if (cardsInHandCount == 0 && Battle.Field.Count == 0)
    {
      Terminal.ShowStompFailed();
      return;
    }

    var coinToss = UbiRandom.Next(0, 2);
    var stompFromHand = coinToss == 1 && cardsInHandCount > 0;

    if (stompFromHand)
    {
      StompFromHand();
    }
    else
    {
      if (Battle.Field.Count == 1) //playedCard is the only one on the field
      {
        if (cardsInHandCount > 0)
        {
          stompFromHand = true;
          StompFromHand();
        }
        else
        {
          StompSelf();
        }
      }
      else
      {
        StompFromField();
      }
    }

    Terminal.ShowStompResult(stompCandidate, stompFromHand);

    void StompSelf()
    {
      Battle.Field.Remove(stompCandidate);
      Battle.Scrapheap.Add(stompCandidate);
    }

    void StompFromHand()
    {
      var randomIndex = UbiRandom.Next(0, cardsInHandCount);
      stompCandidate = Battle.Hand.GetCardAtIndex(randomIndex);
      Battle.Hand.Remove_Single(stompCandidate);
      Battle.Scrapheap.Add(stompCandidate);
    }

    void StompFromField()
    {
      var stompableCardsOnField = Battle.Field.Where(card => card.id != playedCard.id).ToList();
      var randomIndex = UbiRandom.Next(0, stompableCardsOnField.Count);
      stompCandidate = stompableCardsOnField[randomIndex];
      Battle.Field.Remove(stompCandidate);
      Battle.Scrapheap.Add(stompCandidate);
    }
  }

  private static void PerformCycle()
  {
    Battle.Market.Cycle();
    Terminal.ShowCycleResult(Battle.Market.GetDisplayedCards_All());
  }

  private static void PerformShun()
  {
    Terminal.PromptShun(Battle.Hand.GetAllCardsInHand());

    if (Battle.Hand.GetCurrentCount() == 0)
    {
      return;
    }

    var selection = UserInput.GetInt(1) - 1;
    var selectedCard = Battle.Hand.GetCardAtIndex(selection);
    Battle.Scrapheap.Add(selectedCard);
    Battle.Hand.Remove_Single(selectedCard);

    //Only allow a card draw if another card was successfully shunned from hand. 
    PerformDraw();
  }
}