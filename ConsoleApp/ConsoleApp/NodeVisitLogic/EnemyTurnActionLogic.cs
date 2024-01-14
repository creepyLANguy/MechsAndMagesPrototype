using System;
using MaM.Definitions;
using MaM.Helpers;

namespace MaM.NodeVisitLogic
{
    class EnemyTurnActionLogic
  {
    public static void RunPassAction(ref BattleTracker b)
    {
      Terminal.EnemyTurnActionPass();

      b.threat = 0;
    }

    public static void RunBuffAction(ref BattleTracker b, int value)
    {
      Terminal.EnemyTurnActionBuff(value);

      b.threat += value;
    }

    public static void RunAttackAction(ref BattleTracker b)
    {
      Terminal.EnemyTurnActionAttack(b.threat);

      int attackValue;
      if (b.playerIsDefending)
      {
        attackValue = b.threat < b.power ? 0 : b.threat - b.power;
        b.power = attackValue > 0 ? 0 : b.power - b.threat;
      }
      else
      {
        attackValue = b.threat;
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
      Terminal.EnemyTurnActionDefend();
    }

    public static void RunLeechAction(ref BattleTracker b)
    {
      var leechable = Math.Min(b.threat, b.manna);

      Terminal.EnemyTurnActionLeech(leechable);

      if (leechable <= 0)
      {
        return;
      }

      b.threat = Math.Max(0, b.threat - leechable);
      b.manna = Math.Max(0, b.manna - leechable);
      b.enemyHealth += leechable;
    }
  }
}
