using System;
using System.Collections.Generic;
using MaM.Definitions;
using MaM.Helpers;

namespace MaM.GameLogic;

public static class Battle
{
  public static FightResult Run(Fight node, ref GameContents gameContents)
  {
    Console.WriteLine("\n[Start Battle]\n");

    var battlePack = new BattlePack(node, ref gameContents);

    RunMulliganPhase(ref gameContents.player, ref battlePack, ref gameContents.random);
    
    while (true)
    {
      Console.WriteLine("[Turn]\t\t" + gameContents.player.name);
      var resultPlayerAction = ExecuteTurnForPlayer(ref gameContents.player, ref node.enemy, ref battlePack);
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

  private static void RunMulliganPhase(ref Player player, ref BattlePack battlePack, ref Random random)
  {
    var mulliganCost = 1;

    while (true)
    {
      Console.WriteLine("\nYour Hand:");
      PrintCards(battlePack.hand.GetAllCardsInHand());

      Console.WriteLine("\nThe Market:");
      PrintCards(battlePack.market.GetDisplayedCards());

      if (player.health - mulliganCost <= 0)
      {
        return;
      }

      Console.WriteLine("Life : " + player.health);
      var choice = UserInput.GetInt("Mulligan this hand and market by paying " + mulliganCost + " life?\n1) Yes\n2) No");
      if (choice == 1)
      {
        player.health -= mulliganCost;

        battlePack.Mulligan(ref random);

        ++mulliganCost;
      }
      else
      {
        break;
      }
    }
  }

  private static void PrintCards(List<Card> cards)
  {
    const char tab = '\t';
    const char pipe = '|';

    foreach (var card in cards)
    {
      Console.WriteLine(
        UserInput.GetPrintableCardName(card.name) +
        tab + tab + pipe + tab +
        "Manna Cost :" + card.cost +
        tab + pipe + tab +
        "Type:" + card.type +
        tab + pipe + tab +
        "Guild:" + card.guild
      );
    }

    Console.WriteLine();
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

  private static FightResult ExecuteTurnForComputer(ref Player player, ref Enemy enemy)
  {
    //TODO
    enemy.health = new Random((int)(DateTime.Now.Ticks)).Next(0, 5);

    return GetFightFightResult(ref player, ref enemy);
  }

  private static FightResult ExecuteTurnForPlayer(ref Player player, ref Enemy enemy, ref BattlePack battlePack)
  {
    //TODO
    player.health -= new Random((int)(DateTime.Now.Ticks)).Next(0, 5) == 0 ? 1 : 0;

    return GetFightFightResult(ref player, ref enemy);
  }
}