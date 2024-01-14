using System.Linq;
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
        Terminal.PrintHand(battlePack.hand.GetAllCardsInHand());

        Terminal.PrintMarket(battlePack.market.GetDisplayedCards_All());

        if (player.health - mulliganCost <= 0)
        {
          return;
        }

        Terminal.OfferMulligan(player.health, mulliganCost);
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
      Terminal.PromptForAction(canRecruit);
      return (PlayerTurnAction)UserInput.GetInt();
    }

    public static void RunPlayCardsPhase(ref BattlePack battlePack, ref BattleTracker battleTracker)
    {
      while (battlePack.hand.GetAllCardsInHand().Count > 0)
      {
        var canPlayAll = battlePack.hand.HasCardsWithOrderSensitiveEffects() == false;

        Terminal.PromptToPlayCard(ref battlePack, canPlayAll);

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
          battlePack.field.Add(selectedCard);
          battlePack.hand.Remove_Single(selectedCard);
        }
      }
    }

    private static void ProcessCardEffects(Card card, ref BattlePack battlePack, ref BattleTracker battleTracker)
    {
      //Make sure this power gain happens first in case a card has to stomp itself.
      battleTracker.power += card.power;

      for (var i = 0; i < card.abilityCount; ++i)
      {
        switch (card.ability)
        {
          case CardAbility.DRAW:
            PerformDraw(ref battlePack);
            break;
          case CardAbility.HEAL:
            PerformHeal(ref battleTracker);
            break;
          case CardAbility.STOMP:
            PerformStomp(ref battlePack, card);
            break;
          case CardAbility.CYCLE:
            PerformCycle(ref battlePack);
            break;
          case CardAbility.SHUN:
            PerformShun(ref battlePack);
            break;
          case CardAbility.NONE:
          default:
            break;
        }
      }
    }

    private static void PerformDraw(ref BattlePack battlePack)
    {
      battlePack.hand.Draw_Single(ref battlePack.deck, ref battlePack.graveyard);
      Terminal.ShowDrew(battlePack.hand.GetAllCardsInHand());
    }

    private static void PerformHeal(ref BattleTracker battleTracker)
    {
      battleTracker.playerHealth += 1;
      Terminal.ShowHealed(battleTracker.playerHealth);
    }

    private static void PerformStomp(ref BattlePack battlePack, Card playedCard)
    {
      //TODO - test all stomp flows. 

      var stompedCard = playedCard;

      var cardsInHandCount = battlePack.hand.GetAllCardsInHand().Count;

      var coinToss = UbiRandom.Next(0, 2);
      var stompFromHand = coinToss == 1 && cardsInHandCount > 0;

      if (stompFromHand)
      {
        StompFromHand(ref battlePack);
      }
      else
      {
        if (battlePack.field.Count == 1) //playedCard is the only one on the field
        {
          if (cardsInHandCount > 0)
          {
            stompFromHand = true;
            StompFromHand(ref battlePack);
          }
          else
          {
            StompSelf(ref battlePack);
          }
        }
        else
        {
          StompFromField(ref battlePack);
        }
      }
      
      Terminal.ShowStomped(ref battlePack, stompedCard, stompFromHand);

      void StompSelf(ref BattlePack battlePack)
      {
        battlePack.field.Remove(stompedCard);
        battlePack.scrapheap.Add(stompedCard);
      }

      void StompFromHand(ref BattlePack battlePack)
      {
        var randomIndex = UbiRandom.Next(0, cardsInHandCount);
        stompedCard = battlePack.hand.GetCardAtIndex(randomIndex);
        battlePack.hand.Remove_Single(stompedCard);
        battlePack.scrapheap.Add(stompedCard);
      }      
      
      void StompFromField(ref BattlePack battlePack)
      {
        var stompableCardsOnField = battlePack.field.Where(card => card.id != playedCard.id).ToList();
        var randomIndex = UbiRandom.Next(0, stompableCardsOnField.Count);
        stompedCard = stompableCardsOnField[randomIndex];
        battlePack.field.Remove(stompedCard);
        battlePack.scrapheap.Add(stompedCard);
      }
    }

    private static void PerformCycle(ref BattlePack battlePack)
    {
      battlePack.market.Cycle();
      Terminal.ShowCycled(battlePack.market.GetDisplayedCards_All());
    }

    private static void PerformShun(ref BattlePack battlePack)
    {
      //TODO - test shun
      Terminal.PromptShun(battlePack.hand.GetAllCardsInHand());

#if DEBUG
      var selection = 0;
#else
        var selection = UserInput.GetInt() - 1;
#endif

      var selectedCard = battlePack.hand.GetCardAtIndex(selection);
      battlePack.scrapheap.Add(selectedCard);
      battlePack.hand.Remove_Single(selectedCard);

      PerformDraw(ref battlePack);
    }
  }
}
