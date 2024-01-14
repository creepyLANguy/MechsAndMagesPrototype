﻿using MaM.Definitions;
using MaM.Helpers;

namespace MaM.NodeVisitLogic
{
  class PlayerTurnActionLogic
  {
    public static void RunPassAction(ref BattleTracker b)
    {
      b.power = 0;
    }

    public static void RunAttackAction(ref BattleTracker b)
    {
      int attackValue;
      if (b.enemyIsDefending)
      {
        attackValue = b.power < b.threat ? 0 : b.power - b.threat;
        b.threat = attackValue > 0 ? 0 : b.threat - b.power;
      }
      else
      {
        attackValue = b.power;
      }
      
      if (attackValue > 0)
      {
        b.enemyHealth -= attackValue;
      }

      b.power = 0;
    }

    public static void RunDefendAction(ref BattleTracker battleTracker)
    {
    }

    public static void RunRecruitAction(ref BattlePack battlePack, ref BattleTracker b)
    {
      while (battlePack.market.GetDisplayedCards_Affordable(b.power, b.manna).Count > 0)
      {
        Terminal.PrintBattleState(b);
        Terminal.PromptToRecruit(battlePack.market.GetDisplayedCards_All());

        var choice = UserInput.GetInt();
        if (choice == 0)
        {
          break;
        }
        --choice;

        while (choice >= battlePack.market.GetDisplayedCards_All().Count)
        {
          Terminal.PromptInvalidChoiceTryAgain();
          choice = UserInput.GetInt();
        }

        var chosenCard = battlePack.market.TryFetch(choice, ref b);
        if (chosenCard == null)
        {
          var intendedCard = battlePack.market.GetDisplayedCards_All()[choice];
          Terminal.ShowRecruitFailed(intendedCard);
          continue;
        }

        battlePack.graveyard.Add((Card)chosenCard);
      }
    }
  }
}
