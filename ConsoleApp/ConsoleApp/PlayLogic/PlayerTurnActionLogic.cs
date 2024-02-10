using MaM.Definitions;
using MaM.Helpers;

namespace MaM.PlayLogic;

class PlayerTurnActionLogic
{
  public static void RunPassAction()
  {
    Battle.Player.power = 0;
  }

  public static void RunAttackAction()
  {
    int attackValue;
    if (Battle.Enemy.isDefending)
    {
      attackValue = Battle.Player.power < Battle.Enemy.power ? 0 : Battle.Player.power - Battle.Enemy.power;
      Battle.Enemy.power = attackValue > 0 ? 0 : Battle.Enemy.power - Battle.Player.power;
    }
    else
    {
      attackValue = Battle.Player.power;
    }
      
    if (attackValue > 0)
    {
      Battle.Enemy.health -= attackValue;
    }

    Battle.Player.power = 0;
  }

  public static void RunDefendAction()
  {
  }

  public static void RunRecruitAction()
  {
    while (Battle.Market.GetDisplayedCards_Affordable(Battle.Player.power, Battle.Player.manna).Count > 0)
    {
      Terminal.PrintBattleState();
      Terminal.PromptToRecruit(Battle.Market.GetDisplayedCards_All());

      var choice = UserInput.GetInt();
      if (choice == 0)
      {
        break;
      }
      --choice;

      while (choice >= Battle.Market.GetDisplayedCards_All().Count)
      {
        Terminal.PromptInvalidChoiceTryAgain();
        choice = UserInput.GetInt();
      }

      var chosenCard = Battle.Market.TryFetch(choice);
      if (chosenCard == null)
      {
        var intendedCard = Battle.Market.GetDisplayedCards_All()[choice];
        Terminal.ShowRecruitFailed(intendedCard);
        continue;
      }

      Battle.Graveyard.Add((Card)chosenCard);
      Terminal.ShowRecruited((Card) chosenCard);
    }
  }
}