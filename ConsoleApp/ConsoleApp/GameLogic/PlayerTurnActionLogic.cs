using MaM.Definitions;

namespace MaM.GameLogic
{
  class PlayerTurnActionLogic
  {
    public static void RunPassAction(ref int power)
    {
      power = 0;
    }

    public static void RunAttackAction(ref int power, ref int threat, ref Enemy enemy)
    {
      if (threat > 0)
      {
        power -= threat;
      }

      if (power > 0)
      {
        enemy.health -= power;
      }

      power = 0;
    }

    public static void RunDefendAction()
    {
    }

    public static void RunRecruitAction(ref int power, ref BattlePack battlePack)
    {
      //TODO - Buy from market, add those cards to graveyard.

      power = 0;
    }
  }
}
