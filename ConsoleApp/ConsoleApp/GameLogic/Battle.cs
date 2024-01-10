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

    var fightResult = FightResult.NONE;
    while (fightResult == FightResult.NONE)
    {
      fightResult = RunTurns(ref gameContents, ref battlePack, ref node.enemy);
    }

    return fightResult;
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
      var choice = UserInput.GetInt("Mulligan this hand and cycle the market by paying " + mulliganCost + " life?\n1) Yes\n2) No");
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

  private static FightResult RunTurns(ref GameContents gameContents, ref BattlePack battlePack, ref Enemy enemy)
  {
    var turnPools = new TurnPools();

    PrintStats(ref gameContents.player, ref enemy, ref turnPools);

    ExecuteTurnForPlayer(ref gameContents.player, ref enemy, ref battlePack, ref turnPools);

    var resultPlayerAction = GetFightResult(ref gameContents.player, ref enemy);
    if (resultPlayerAction != FightResult.NONE)
    {
      return resultPlayerAction;
    }

    PrintStats(ref gameContents.player, ref enemy, ref turnPools);

    ExecuteTurnForComputer(ref gameContents.player, ref enemy);

    return GetFightResult(ref gameContents.player, ref enemy);
  }

  private static void PrintStats(ref Player player, ref Enemy enemy, ref TurnPools turnPools)
  {
    Console.WriteLine("Your Life:\t" + player.health);
    
    Console.WriteLine("Your Might:\t" + turnPools.might);

    Console.WriteLine("Enemy Life:\t" + enemy.health);
    
    Console.WriteLine("Enemy Manna:\t" + enemy.manna);
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

  private static FightResult GetFightResult(ref Player player, ref Enemy enemy)
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

  private static void ExecuteTurnForComputer(ref Player player, ref Enemy enemy)
  {
    Console.WriteLine("\n[Turn]\t\t" + enemy.name);

    //TODO
    player.health = new Random((int)(DateTime.Now.Ticks)).Next(0, 5);

  }

  private static void ExecuteTurnForPlayer(ref Player player,
    ref Enemy enemy,
    ref BattlePack battlePack,
    ref TurnPools turnPools,
    ref Random random)
  {
    Console.WriteLine("\n[Turn]\t\t" + player.name);

    //TODO
    //AL.
    enemy.health -= new Random((int)(DateTime.Now.Ticks)).Next(0, 5) == 0 ? 1 : 0;

    battlePack.hand.Fill(ref battlePack.deck, ref battlePack.graveyard, ref random);

    RunPlayCardsPhase(ref battlePack, ref turnPools, ref player);

    var turnAction = RunActionSelectionPhase();

    switch (turnAction)
    {
      case TurnAction.ATTACK:
        RunAttackAction(ref turnPools, ref enemy);
        break;
      case TurnAction.DEFEND:
        RunDefendAction(ref turnPools);
        break;
      case TurnAction.BUY:
        RunBuyAction(ref turnPools, ref battlePack);
        break;
      case TurnAction.NONE:
      default:
        RunPassAction(ref turnPools);
        break;
    }
  }

  private static TurnAction RunActionSelectionPhase()
  {
    var message = "\nSelect an action:";
    foreach (TurnAction turnAction in Enum.GetValues(typeof(TurnAction)))
    {
      message += "\n" + turnAction.ToString("D") + ") " + turnAction.ToString().ToSentenceCase();
    }
    return (TurnAction)UserInput.GetInt(message);
  }

  private static void RunPlayCardsPhase(ref BattlePack battlePack, ref TurnPools turnPools, ref Player player)
  {
    //TODO
  } 
  
  private static void RunPassAction(ref TurnPools turnPools)
  {
    turnPools.might = 0;
    turnPools.moneny = 0;
  }

  private static void RunAttackAction(ref TurnPools turnPools, ref Enemy enemy)
  {
    enemy.health -= turnPools.might;
    turnPools.might = 0;
    turnPools.moneny = 0;
  }

  private static void RunDefendAction(ref TurnPools turnPools)
  {
    turnPools.moneny = 0;
  }

  private static void RunBuyAction(ref TurnPools turnPools, ref BattlePack battlePack)
  {
    turnPools.might = 0;
    
    //TODO - Buy from market, add those cards to graveyard.
  }
}