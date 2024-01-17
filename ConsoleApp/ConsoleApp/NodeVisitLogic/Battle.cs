using System.Linq;
using MaM.Definitions;
using MaM.Enums;
using MaM.Helpers;

namespace MaM.NodeVisitLogic;

public static class Battle
{
  public static FightResult Run(Fight node, ref GameContents gameContents)
  {
    Terminal.StartBattle();

    var battlePack = new BattlePack(node, ref gameContents);

    BattlePhases.RunMulliganPhase(ref gameContents.player, ref battlePack);

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

    Terminal.PrintFightResult(fightResult);

    if (node.fightType == FightType.ELITE)
    {
      OfferReward(ref gameContents, node.guild, node.enemy.marketSize);
    }

    gameContents.player.health = battleTracker.player.health;

    return fightResult;
  }

  private static FightResult RunTurns(
    ref GameContents gameContents,
    ref BattlePack battlePack,
    ref Enemy enemy,
    ref BattleTracker battleTracker)
  {
    Terminal.PrintBattleState(battleTracker);

    ExecuteTurnForPlayer(
      ref gameContents.player,
      ref battlePack,
      ref battleTracker);

    var resultPlayerAction = GetFightResult(ref battleTracker);
    if (resultPlayerAction != FightResult.NONE)
    {
      return resultPlayerAction;
    }

    Terminal.PrintBattleState(battleTracker);
    
    ExecuteTurnForComputer(ref enemy, ref battleTracker);

    return GetFightResult(ref battleTracker);
  }

  private static FightResult GetFightResult(ref BattleTracker battleTracker)
  {
    if (battleTracker.player.health <= 0)
    {
      return FightResult.PLAYER_LOSE;
    }
    
    if (battleTracker.enemy.health <= 0)
    {
      return FightResult.PLAYER_WIN;
    }

    return FightResult.NONE;
  }

  private static void ExecuteTurnForPlayer(
    ref Player player,
    ref BattlePack battlePack,
    ref BattleTracker battleTracker)
  {
    Terminal.Turn(player.name);

    battlePack.hand.Draw_Full(ref battlePack.deck, ref battlePack.graveyard);

    BattlePhases.RunPlayCardsPhase(ref battlePack, ref battleTracker);

    Terminal.PrintBattleState(battleTracker);

    var canRecruit = battlePack.market.GetDisplayedCards_Affordable(battleTracker.player.power, battleTracker.player.manna).Count > 0;
    var playerTurnAction = BattlePhases.RunActionSelectionPhase(canRecruit);

    battleTracker.player.isDefending = playerTurnAction == PlayerTurnAction.DEFEND;

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
    battlePack.MoveFieldToGraveyard();
  }

  private static void ExecuteTurnForComputer(
    ref Enemy enemy,
    ref BattleTracker battleTracker)
  {
    Terminal.Turn(enemy.name);

    var nextEnemyTurnAction = enemy.turnActions.First();
    enemy.turnActions.Remove(nextEnemyTurnAction);
    enemy.turnActions = enemy.turnActions.Append(nextEnemyTurnAction).ToList();

    var key = nextEnemyTurnAction.Item1;
    var value = nextEnemyTurnAction.Item2;

    battleTracker.enemy.isDefending = key == EnemyTurnAction.D;

    switch (key)
    {
      case EnemyTurnAction.B:
        EnemyTurnActionLogic.RunBuffAction(ref battleTracker, value);
        break;
      case EnemyTurnAction.A:
        EnemyTurnActionLogic.RunAttackAction(ref battleTracker);
        break;
      case EnemyTurnAction.D:
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

  private static void OfferReward(ref GameContents gameContents, Guild guild, int numberOfChoices)
  {
    var possibleRewards = gameContents.cards.Where(card => card.guild == guild).ToList();
    possibleRewards.Shuffle();

    var offeredRewards = possibleRewards.Take(numberOfChoices).ToList();

    Terminal.PromptToChooseReward(offeredRewards);

#if DEBUG
    var choice = 0;
#else
    var choice = UserInput.GetInt() - 1;
#endif

    while (choice >= offeredRewards.Count)
    {
      Terminal.PromptInvalidChoiceTryAgain();
      choice = UserInput.GetInt() - 1;
    }

    var cardChosen = offeredRewards[choice];
    gameContents.player.AddToDeck(cardChosen);
  }
}