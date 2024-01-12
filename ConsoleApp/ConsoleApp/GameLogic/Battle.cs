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

    var battleTracker = new BattleTracker(gameContents.player.health, node.enemy.health);

    var fightResult = FightResult.NONE;
    while (fightResult == FightResult.NONE)
    {
      fightResult = RunTurns(
        ref gameContents,
        ref battlePack,
        ref node.enemy,
        ref battleTracker);
    }

    gameContents.player.health = battleTracker.playerHealth;

    return fightResult;
  }

  private static FightResult RunTurns(
    ref GameContents gameContents,
    ref BattlePack battlePack,
    ref Enemy enemy,
    ref BattleTracker battleTracker)
  {
    ConsoleMessages.PrintBattleState(battleTracker);

    ExecuteTurnForPlayer(
      ref gameContents.player,
      ref battlePack,
      ref battleTracker,
      ref gameContents.random);

    var resultPlayerAction = GetFightResult(ref battleTracker);
    if (resultPlayerAction != FightResult.NONE)
    {
      return resultPlayerAction;
    }

    ConsoleMessages.PrintBattleState(battleTracker);
    
    ExecuteTurnForComputer(ref enemy, ref battleTracker);

    return GetFightResult(ref battleTracker);
  }

  private static FightResult GetFightResult(ref BattleTracker battleTracker)
  {
    if (battleTracker.playerHealth <= 0)
    {
      return FightResult.PLAYER_LOSE;
    }
    
    if (battleTracker.enemyHealth <= 0)
    {
      return FightResult.PLAYER_WIN;
    }

    return FightResult.NONE;
  }

  private static void ExecuteTurnForPlayer(
    ref Player player,
    ref BattlePack battlePack,
    ref BattleTracker battleTracker,
    ref Random random)
  {
    ConsoleMessages.Turn(player.name);

    battleTracker.power = 0;

    battlePack.hand.Draw_Full(ref battlePack.deck, ref battlePack.graveyard, ref random);

    BattlePhases.RunPlayCardsPhase(ref battlePack, ref battleTracker);

    ConsoleMessages.PrintBattleState(battleTracker);

    var playerTurnAction = BattlePhases.RunActionSelectionPhase();

    switch (playerTurnAction)
    {
      case PlayerTurnAction.ATTACK:
        PlayerTurnActionLogic.RunAttackAction(ref battleTracker);
        break;
      case PlayerTurnAction.DEFEND:
        PlayerTurnActionLogic.RunDefendAction(ref battleTracker);
        break;
      case PlayerTurnAction.RECRUIT:
        PlayerTurnActionLogic.RunRecruitAction(ref battlePack, ref battleTracker);
        break;
      case PlayerTurnAction.NONE:
      default:
        PlayerTurnActionLogic.RunPassAction(ref battleTracker);
        break;
    }

    battlePack.MoveHandToGraveyard();
  }

  private static void ExecuteTurnForComputer(
    ref Enemy enemy,
    ref BattleTracker battleTracker)
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
        EnemyTurnActionLogic.RunBuffAction(ref battleTracker, value);
        break;
      case EnemyTurnAction.A:
        battleTracker.enemyIsDefending = false;
        EnemyTurnActionLogic.RunAttackAction(ref battleTracker);
        break;
      case EnemyTurnAction.D:
        battleTracker.enemyIsDefending = true;
        EnemyTurnActionLogic.RunDefendAction(ref battleTracker);
        break;
      case EnemyTurnAction.L:
        EnemyTurnActionLogic.RunLeechAction(ref battleTracker);
        break;
      case EnemyTurnAction.N:
      default:
        EnemyTurnActionLogic.RunPassAction(ref battleTracker);
        break;
    }
  }

}