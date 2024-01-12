using MaM.Definitions;

namespace MaM.GameLogic
{
  class PlayerTurnActionLogic
  {
    public static void RunPassAction(ref BattleTracker b)
    {
      b.power = 0;
    }

    public static void RunAttackAction(ref BattleTracker b)
    {
      var attackValue = b.power;

      if (b.enemyIsDefending && b.threat > 0)
      {
        attackValue -= b.threat;
        b.threat -= b.power;

        if (b.threat < 0)
        {
          b.threat = 0;
        }
      }
      
      if (attackValue > 0)
      {
        b.enemyHealth -= attackValue;
      }

      b.power = 0;
    }

    public static void RunDefendAction(ref BattleTracker b)
    {
    }

    public static void RunRecruitAction(ref BattlePack battlePack, ref BattleTracker b)
    {
      //TODO - Buy from market, add those cards to graveyard.

      b.power = 0;
    }
  }
}
