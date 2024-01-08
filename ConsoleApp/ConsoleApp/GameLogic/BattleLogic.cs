using System;
using System.Linq;
using MaM.Definitions;

namespace MaM.GameLogic;

public static class Battle
{
  public static FightResult Run(Fight node, ref GameContents gameContents)
  {
    Console.WriteLine("\n[Start Battle]");

    var traderow = new Traderow(
      node.enemy.tradeRowSize,
      gameContents.player.deck.Where(card => card.guild != Guild.NEUTRAL).ToList(),
      gameContents.cards.Where(card => card.guild == node.guild).ToList(),
      ref gameContents.random
    );

    while (true)
    {
      Console.WriteLine("[Turn]\t\t" + gameContents.player.name);
      var resultPlayerAction = ExecuteTurnForPlayer(ref gameContents.player, ref node.enemy);
      if (resultPlayerAction != FightResult.NONE)
      {
        return resultPlayerAction;
      }

      Console.WriteLine("[Turn]\t\t" + node.enemy.name);
      var resultEnemyAction = ExecuteTurnForComputer(ref gameContents.player, ref node.enemy);
      if (resultEnemyAction != FightResult.NONE)
      {
        return resultEnemyAction;
      }
    }
  }

  private static FightResult GetFightFightResult(ref Player player, ref Enemy enemy)
  {
    if (player.health <= 0)
    {
      return FightResult.PLAYER_LOSE;
    }
    
    if (enemy.health <= 0)
    {
      return FightResult.PLAYER_WIN;
    }

    return FightResult.NONE;
  }

  private static FightResult ExecuteTurnForPlayer(ref Player player, ref Enemy enemy)
  {
    //TODO
    player.health -= new Random((int)(DateTime.Now.Ticks)).Next(0, 5) == 0 ? 1 : 0;

    return GetFightFightResult(ref player, ref enemy);
  }

  private static FightResult ExecuteTurnForComputer(ref Player player, ref Enemy enemy)
  {
    //TODO
    enemy.health = new Random((int)(DateTime.Now.Ticks)).Next(0, 5);

    return GetFightFightResult(ref player, ref enemy);
  }
}