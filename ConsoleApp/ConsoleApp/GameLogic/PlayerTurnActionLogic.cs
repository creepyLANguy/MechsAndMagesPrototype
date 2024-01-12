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
      var attackValue = power;

      if (threat > 0)
      {
        attackValue -= threat;
        threat -= power;
      }

      if (attackValue > 0)
      {
        enemy.health -= attackValue;
        threat = 0;
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
