using MaM.Definitions;
using MaM.Helpers;

namespace MaM.GameLogic
{
    class EnemyTurnActionLogic
  {
    public static void RunPassAction(ref BattleTracker b)
    {
      ConsoleMessages.EnemyTurnActionPass();

      b.threat = 0;
    }

    public static void RunBuffAction(ref BattleTracker b, int value)
    {
      ConsoleMessages.EnemyTurnActionBuff(value);

      b.threat += value;
    }

    public static void RunAttackAction(ref BattleTracker b)
    {
      ConsoleMessages.EnemyTurnActionAttack(b.threat);

      var attackValue = b.threat;

      if (b.playerIsDefending && b.power > 0)
      {
        attackValue -= b.power;
        b.power -= attackValue;

        if (b.power < 0)
        {
          b.power = 0;
        }
      }

      if (attackValue > 0)
      {
        b.playerHealth-= attackValue;
        b.manna += attackValue;
      }

      b.threat = 0;
    }

    public static void RunDefendAction(ref BattleTracker b)
    {
      ConsoleMessages.EnemyTurnActionDefend();
    }

    public static void RunLeechAction(ref BattleTracker b)
    {
      var leechable = (b.threat > b.manna) ? b.manna : b.threat;

      ConsoleMessages.EnemyTurnActionLeech(leechable);

      if (leechable <= 0)
      {
        return;
      }

      if (b.threat <= leechable)
      {
        b.threat = 0;
      }
      else
      {
        b.threat -= leechable;
      }

      if (b.manna <= leechable)
      {
        b.manna = 0;
      }
      else
      {
        b.manna -= leechable;
      }

      b.enemyHealth += leechable;
    }
  }
}
