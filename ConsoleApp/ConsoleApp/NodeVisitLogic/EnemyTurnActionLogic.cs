﻿using System;
using MaM.Definitions;
using MaM.Helpers;

namespace MaM.NodeVisitLogic
{
    class EnemyTurnActionLogic
  {
    public static void RunPassAction(ref BattleTracker b)
    {
      Terminal.EnemyTurnActionPass();

      b.enemy.power = 0;
    }

    public static void RunBuffAction(ref BattleTracker b, int value)
    {
      Terminal.EnemyTurnActionBuff(value);

      b.enemy.power += value;
    }

    public static void RunAttackAction(ref BattleTracker b)
    {
      Terminal.EnemyTurnActionAttack(b.enemy.power);

      int attackValue;
      if (b.player.isDefending)
      {
        attackValue = b.enemy.power < b.player.power ? 0 : b.enemy.power - b.player.power;
        b.player.power = attackValue > 0 ? 0 : b.player.power - b.enemy.power;
      }
      else
      {
        attackValue = b.enemy.power;
      }

      if (attackValue > 0)
      {
        b.player.health -= attackValue;
        b.player.manna += attackValue;
      }

      b.enemy.power = 0;
    }

    public static void RunDefendAction(ref BattleTracker b)
    {
      Terminal.EnemyTurnActionDefend();
    }

    public static void RunLeechAction(ref BattleTracker b)
    {
      var leechable = Math.Min(b.enemy.power, b.player.manna);

      Terminal.EnemyTurnActionLeech(leechable);

      if (leechable <= 0)
      {
        return;
      }

      b.enemy.power= Math.Max(0, b.enemy.power- leechable);
      b.player.manna = Math.Max(0, b.player.manna - leechable);
      b.enemy.health += leechable;
    }
  }
}
