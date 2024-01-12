using System;
using System.Linq;
using MaM.Definitions;
using MaM.Helpers;

namespace MaM.GameLogic;

public static class Battle
{
  public static FightResult Run(Fight node, ref GameContents gameContents)
  {
    ConsoleMessages.StartBattle();

    var battlePack = new BattlePack(node, ref gameContents);

    BattlePhases.RunMulliganPhase(ref gameContents.player, ref battlePack, ref gameContents.random);

    var power = 0;
    var manna = 0;

    var threat = 0;

    var fightResult = FightResult.NONE;
    while (fightResult == FightResult.NONE)
    {
      fightResult = RunTurns(ref gameContents, ref battlePack, ref node.enemy, ref power, ref manna, ref threat);
    }

    return fightResult;
  }

  private static FightResult RunTurns(ref GameContents gameContents, ref BattlePack battlePack, ref Enemy enemy, ref int power, ref int manna, ref int threat)
  {
    ConsoleMessages.PrintBattleState(gameContents.player, enemy, power, manna, threat);

    ExecuteTurnForPlayer(ref gameContents.player, ref enemy, ref battlePack, ref power, ref manna, ref threat, ref gameContents.random);

    var resultPlayerAction = GetFightResult(ref gameContents.player, ref enemy);
    if (resultPlayerAction != FightResult.NONE)
    {
      return resultPlayerAction;
    }

    ConsoleMessages.PrintBattleState(gameContents.player, enemy, power, manna, threat);

    ExecuteTurnForComputer(ref gameContents.player, ref enemy, ref power, ref manna, ref threat);

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

  private static void ExecuteTurnForComputer(ref Player player, ref Enemy enemy, ref int power, ref int manna, ref int threat)
  {
    ConsoleMessages.Turn(enemy.name);

    var nextEnemyTurnAction = enemy.turnActions.First();
    enemy.turnActions.Remove(nextEnemyTurnAction);
    enemy.turnActions = enemy.turnActions.Append(nextEnemyTurnAction).ToList();

    var key = nextEnemyTurnAction.Item1;
    var value = nextEnemyTurnAction.Item2;

    switch (key)
    {
      case EnemyTurnAction.B:
        EnemyTurnActionLogic.RunBuffAction(ref threat, value);
        break;
      case EnemyTurnAction.A:
        EnemyTurnActionLogic.RunAttackAction(ref threat, ref power, ref manna, ref player);
        break;
      case EnemyTurnAction.D:
        EnemyTurnActionLogic.RunDefendAction();
        break;
      case EnemyTurnAction.L:
        EnemyTurnActionLogic.RunLeechAction(ref enemy, ref value, ref manna);
        break;
      case EnemyTurnAction.N:
      default:
        EnemyTurnActionLogic.RunPassAction(ref threat);
        break;
    }
  }

  private static void ExecuteTurnForPlayer(
    ref Player player,
    ref Enemy enemy,
    ref BattlePack battlePack,
    ref int power,
    ref int manna,
    ref int threat,
    ref Random random)
  {
    ConsoleMessages.Turn(player.name);

    battlePack.hand.Draw_Full(ref battlePack.deck, ref battlePack.graveyard, ref random);

    BattlePhases.RunPlayCardsPhase(ref battlePack, ref player, ref power, ref manna);

    ConsoleMessages.PrintBattleState(player, enemy, power, manna, threat);

    var playerTurnAction = BattlePhases.RunActionSelectionPhase();

    switch (playerTurnAction)
    {
      case PlayerTurnAction.ATTACK:
        PlayerTurnActionLogic.RunAttackAction(ref power, ref threat, ref enemy);
        break;
      case PlayerTurnAction.DEFEND:
        PlayerTurnActionLogic.RunDefendAction();
        break;
      case PlayerTurnAction.RECRUIT:
        PlayerTurnActionLogic.RunRecruitAction(ref power, ref battlePack);
        break;
      case PlayerTurnAction.NONE:
      default:
        PlayerTurnActionLogic.RunPassAction(ref power);
        break;
    }

    battlePack.MoveHandToGraveyard();
  }
}