using MaM.Definitions;
using MaM.Helpers;

namespace MaM.NodeVisitLogic
{
  class PlayerTurnActionLogic
  {
    public static void RunPassAction(ref BattlePack b)
    {
      b.player.power = 0;
    }

    public static void RunAttackAction(ref BattlePack b)
    {
      int attackValue;
      if (b.enemy.isDefending)
      {
        attackValue = b.player.power < b.enemy.power ? 0 : b.player.power - b.enemy.power;
        b.enemy.power = attackValue > 0 ? 0 : b.enemy.power - b.player.power;
      }
      else
      {
        attackValue = b.player.power;
      }
      
      if (attackValue > 0)
      {
        b.enemy.health -= attackValue;
      }

      b.player.power = 0;
    }

    public static void RunDefendAction(ref BattlePack b)
    {
    }

    public static void RunRecruitAction(ref BattlePack b)
    {
      while (b.market.GetDisplayedCards_Affordable(b.player.power, b.player.manna).Count > 0)
      {
        Terminal.PrintBattleState(b);
        Terminal.PromptToRecruit(b.market.GetDisplayedCards_All());

        var choice = UserInput.GetInt();
        if (choice == 0)
        {
          break;
        }
        --choice;

        while (choice >= b.market.GetDisplayedCards_All().Count)
        {
          Terminal.PromptInvalidChoiceTryAgain();
          choice = UserInput.GetInt();
        }

        var chosenCard = b.market.TryFetch(choice, ref b);
        if (chosenCard == null)
        {
          var intendedCard = b.market.GetDisplayedCards_All()[choice];
          Terminal.ShowRecruitFailed(intendedCard);
          continue;
        }

        b.graveyard.Add((Card)chosenCard);
      }
    }
  }
}
