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

      var attackValue = 0;

      if (b.playerIsDefending)
      {
        attackValue = b.threat < b.power ? 0 : b.threat - b.power;
        b.power = attackValue > 0 ? 0 : b.power - b.threat;
      }

      if (attackValue > 0)
      {
        b.playerHealth -= attackValue;
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
