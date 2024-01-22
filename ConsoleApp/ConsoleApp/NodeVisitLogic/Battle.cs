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

    var fightResult = FightResult.NONE;
    while (fightResult == FightResult.NONE)
    {
      fightResult = RunTurns(
        ref gameContents,
        ref battlePack,
        ref node.enemy);
    }

    Terminal.PrintFightResult(fightResult);

    if (node.fightType == FightType.ELITE || node.isMystery)
    {
      OfferReward(ref gameContents, node.guild, node.enemy.marketSize);
    }

    gameContents.player.health = battlePack.player.health;

    return fightResult;
  }

  private static FightResult RunTurns(
    ref GameContents gameContents,
    ref BattlePack b,
    ref Enemy enemy)
  {
    Terminal.PrintBattleState(b);

    Terminal.Turn(gameContents.player.name);
    ExecuteTurnForPlayer(ref b);

    var resultPlayerAction = GetFightResult(ref b);
    if (resultPlayerAction != FightResult.NONE)
    {
      return resultPlayerAction;
    }

    Terminal.PrintBattleState(b);

    Terminal.Turn(enemy.name);
    ExecuteTurnForComputer(ref enemy, ref b);

    return GetFightResult(ref b);
  }

  private static FightResult GetFightResult(ref BattlePack b)
  {
    if (b.player.health <= 0)
    {
      return FightResult.PLAYER_LOSE;
    }
    
    if (b.enemy.health <= 0)
    {
      return FightResult.PLAYER_WIN;
    }

    return FightResult.NONE;
  }

  private static void ExecuteTurnForPlayer(ref BattlePack b)
  {
    b.hand.Draw_Full(ref b.deck, ref b.graveyard);

    BattlePhases.RunPlayCardsPhase(ref b);

    Terminal.PrintBattleState(b);

    var canRecruit = b.market.GetDisplayedCards_Affordable(b.player.power, b.player.manna).Count > 0;
    var playerTurnAction = BattlePhases.RunActionSelectionPhase(canRecruit);

    b.player.isDefending = playerTurnAction == PlayerTurnAction.DEFEND;

    switch (playerTurnAction)
    {
      case PlayerTurnAction.ATTACK:
        PlayerTurnActionLogic.RunAttackAction(ref b);
        break;
      case PlayerTurnAction.DEFEND:
        PlayerTurnActionLogic.RunDefendAction(ref b);
        break;
      case PlayerTurnAction.RECRUIT:
        PlayerTurnActionLogic.RunRecruitAction(ref b);
        break;
      case PlayerTurnAction.NONE:
      default:
        PlayerTurnActionLogic.RunPassAction(ref b);
        break;
    }

    b.MoveHandToGraveyard();
    b.MoveFieldToGraveyard();
  }

  private static void ExecuteTurnForComputer(
    ref Enemy enemy,
    ref BattlePack b)
  {
    var nextEnemyTurnAction = enemy.turnActions.First();
    enemy.turnActions.Remove(nextEnemyTurnAction);
    enemy.turnActions = enemy.turnActions.Append(nextEnemyTurnAction).ToList();

    var key = nextEnemyTurnAction.Item1;
    var value = nextEnemyTurnAction.Item2;

    b.enemy.isDefending = key == EnemyTurnAction.D;

    switch (key)
    {
      case EnemyTurnAction.B:
        EnemyTurnActionLogic.RunBuffAction(ref b, value);
        break;
      case EnemyTurnAction.A:
        EnemyTurnActionLogic.RunAttackAction(ref b);
        break;
      case EnemyTurnAction.D:
        EnemyTurnActionLogic.RunDefendAction(ref b);
        break;
      case EnemyTurnAction.L:
        EnemyTurnActionLogic.RunLeechAction(ref b);
        break;
      case EnemyTurnAction.N:
      default:
        EnemyTurnActionLogic.RunPassAction(ref b);
        break;
    }
  }

  private static void OfferReward(ref GameContents gameContents, Guild guild, int numberOfChoices)
  {
    var possibleRewards = gameContents.cards.Where(card => card.guild == guild).ToList();
    possibleRewards.Shuffle();

    var offeredRewards = possibleRewards.Take(numberOfChoices).ToList();

    Terminal.PromptToChooseReward(offeredRewards);

    var choice = UserInput.GetInt(1) - 1;
    while (choice >= offeredRewards.Count)
    {
      Terminal.PromptInvalidChoiceTryAgain();
      choice = UserInput.GetInt() - 1;
    }

    var cardChosen = offeredRewards[choice];
    gameContents.player.AddToDeck(cardChosen);
  }
}