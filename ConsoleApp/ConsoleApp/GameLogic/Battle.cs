using System;
using MaM.Definitions;
using MaM.Helpers;

namespace MaM.GameLogic;

public static class Battle
{
  public static FightResult Run(Fight node, ref GameContents gameContents)
  {
    Console.WriteLine("\n[Start Battle]\n");

    var battlePack = new BattlePack(node, ref gameContents);

    BattlePhases.RunMulliganPhase(ref gameContents.player, ref battlePack, ref gameContents.random);

    var fightResult = FightResult.NONE;
    while (fightResult == FightResult.NONE)
    {
      fightResult = RunTurns(ref gameContents, ref battlePack, ref node.enemy);
    }

    return fightResult;
  }

  private static FightResult RunTurns(ref GameContents gameContents, ref BattlePack battlePack, ref Enemy enemy)
  {
    var power = 0;

    ConsoleMessages.PrintBattleState(ref gameContents.player, ref enemy, ref power);

    ExecuteTurnForPlayer(ref gameContents.player, ref enemy, ref battlePack, ref power, ref gameContents.random);

    var resultPlayerAction = GetFightResult(ref gameContents.player, ref enemy);
    if (resultPlayerAction != FightResult.NONE)
    {
      return resultPlayerAction;
    }

    ConsoleMessages.PrintBattleState(ref gameContents.player, ref enemy, ref power);

    ExecuteTurnForComputer(ref gameContents.player, ref enemy);

    return GetFightResult(ref gameContents.player, ref enemy);
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
    ref int power,
    ref Random random)
  {
    Console.WriteLine("\n[Turn]\t\t" + player.name);

    //TODO
    //AL.
    enemy.health -= new Random((int)(DateTime.Now.Ticks)).Next(0, 5) == 0 ? 1 : 0;

    battlePack.hand.Draw_Full(ref battlePack.deck, ref battlePack.graveyard, ref random);

    BattlePhases.RunPlayCardsPhase(ref battlePack, ref player, ref power);

    var turnAction = BattlePhases.RunActionSelectionPhase();

    switch (turnAction)
    {
      case TurnAction.ATTACK:
        BattleActions.RunAttackAction(ref power, ref enemy);
        break;
      case TurnAction.DEFEND:
        BattleActions.RunDefendAction();
        break;
      case TurnAction.RECRUIT:
        BattleActions.RunRecruitAction(ref power, ref battlePack);
        break;
      case TurnAction.NONE:
      default:
        BattleActions.RunPassAction(ref power);
        break;
    }

    battlePack.MoveHandToGraveyard();
  }
}