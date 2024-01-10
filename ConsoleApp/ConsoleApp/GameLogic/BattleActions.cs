using MaM.Definitions;

namespace MaM.GameLogic
{
  class BattleActions
  {
    public static void RunPassAction(ref int power)
    {
      power = 0;
    }

    public static void RunAttackAction(ref int power, ref Enemy enemy)
    {
      enemy.health -= power;
      power = 0;
    }

    public static void RunDefendAction()
    {
    }

    public static void RunBuyAction(ref int power, ref BattlePack battlePack)
    {
      //TODO - Buy from market, add those cards to graveyard.
    }
  }
}
