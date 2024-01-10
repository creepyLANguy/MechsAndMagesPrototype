using MaM.Definitions;

namespace MaM.GameLogic
{
  class BattleActions
  {
    public static void RunPassAction(ref TurnPools turnPools)
    {
      turnPools.might = 0;
      turnPools.moneny = 0;
    }

    public static void RunAttackAction(ref TurnPools turnPools, ref Enemy enemy)
    {
      enemy.health -= turnPools.might;
      turnPools.might = 0;
      turnPools.moneny = 0;
    }

    public static void RunDefendAction(ref TurnPools turnPools)
    {
      turnPools.moneny = 0;
    }

    public static void RunBuyAction(ref TurnPools turnPools, ref BattlePack battlePack)
    {
      turnPools.might = 0;

      //TODO - Buy from market, add those cards to graveyard.
    }
  }
}
