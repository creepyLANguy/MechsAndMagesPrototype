using MaM.Definitions;
using MaM.Helpers;

namespace MaM.GameLogic
{
  class EnemyTurnActionLogic
  {
    public static void RunBuffAction(ref int threat, int value)
    {
      ConsoleMessages.EnemyTurnActionBuff(value);

      threat += value;
    }

    public static void RunAttackAction(ref int threat, ref int power, ref int manna, ref Player player)
    {
      ConsoleMessages.EnemyTurnActionAttack(threat);

      if (power > 0)
      {
        threat -= power;
      }
      if (threat > 0)
      {
        player.health -= threat;
        manna += threat;
      }

      threat = 0;
      power = 0;
    }

    public static void RunDefendAction()
    {
      ConsoleMessages.EnemyTurnActionDefend();
    }

    public static void RunLeechAction(ref Enemy enemy, ref int threat, ref int manna)
    {
      ConsoleMessages.EnemyTurnActionLeech(threat);

      enemy.health += threat;
      manna -= threat;
      threat = 0;
    }

    public static void RunPassAction(ref int threat)
    {
      ConsoleMessages.EnemyTurnActionPass();

      threat = 0;
    }
  }
}
