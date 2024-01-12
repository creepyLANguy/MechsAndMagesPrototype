using System;
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
    var manna = 0;

    ConsoleMessages.PrintBattleState(ref gameContents.player, ref enemy, ref power, ref manna);

    ExecuteTurnForPlayer(ref gameContents.player, ref enemy, ref battlePack, ref power, ref manna, ref gameContents.random);

    var resultPlayerAction = GetFightResult(ref gameContents.player, ref enemy);
    if (resultPlayerAction != FightResult.NONE)
    {
      return resultPlayerAction;
    }

    ConsoleMessages.PrintBattleState(ref gameContents.player, ref enemy, ref power, ref manna);

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
    ConsoleMessages.Turn(enemy.name);

    //TODO - enemy turn
    //AL.
    player.health -= new Random((int)(DateTime.Now.Ticks)).Next(0, 5) == 0 ? 10 : 0;

  }

  private static void ExecuteTurnForPlayer(
    ref Player player,
    ref Enemy enemy,
    ref BattlePack battlePack,
    ref int power,
    ref int manna,
    ref Random random)
  {
    ConsoleMessages.Turn(player.name);

    battlePack.hand.Draw_Full(ref battlePack.deck, ref battlePack.graveyard, ref random);

    BattlePhases.RunPlayCardsPhase(ref battlePack, ref player, ref power, ref manna);

    ConsoleMessages.PrintBattleState(ref player, ref enemy, ref power, ref manna);

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