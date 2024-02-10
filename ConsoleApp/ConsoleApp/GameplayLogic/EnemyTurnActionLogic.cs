using System;
using MaM.Helpers;

namespace MaM.GameplayLogic;

class EnemyTurnActionLogic
{
  public static void RunPassAction()
  {
    Terminal.EnemyTurnActionPass();

    Battle.Enemy.power = 0;
  }

  public static void RunBuffAction(int value)
  {
    Terminal.EnemyTurnActionBuff(value);

    Battle.Enemy.power += value;
  }

  public static void RunAttackAction()
  {
    Terminal.EnemyTurnActionAttack(Battle.Enemy.power);

    int attackValue;
    if (Battle.Player.isDefending)
    {
      attackValue = Battle.Enemy.power < Battle.Player.power ? 0 : Battle.Enemy.power - Battle.Player.power;
      Battle.Player.power = attackValue > 0 ? 0 : Battle.Player.power - Battle.Enemy.power;
    }
    else
    {
      attackValue = Battle.Enemy.power;
    }

    if (attackValue > 0)
    {
      Battle.Player.health -= attackValue;
      Battle.Player.manna += attackValue;
    }

    Battle.Enemy.power = 0;
  }

  public static void RunDefendAction()
  {
    Terminal.EnemyTurnActionDefend();
  }

  public static void RunLeechAction()
  {
    var leechable = Math.Min(Battle.Enemy.power, Battle.Player.manna);

    Terminal.EnemyTurnActionLeech(leechable);

    if (leechable <= 0)
    {
      return;
    }

    Battle.Enemy.power= Math.Max(0, Battle.Enemy.power- leechable);
    Battle.Player.manna = Math.Max(0, Battle.Player.manna - leechable);
    Battle.Enemy.health += leechable;
  }
}