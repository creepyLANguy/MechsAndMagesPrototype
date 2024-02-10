using System.Linq;
using MaM.Definitions;
using MaM.Enums;
using MaM.Helpers;

namespace MaM.PlayLogic;

public static class BattleTurns
{
  public static FightResult Run(Fight node, ref GameContents gameContents)
  {
    Terminal.StartBattle();

    Battle.Start(node, ref gameContents);

    BattlePhases.RunMulliganPhase(ref gameContents.player);

    var fightResult = FightResult.NONE;
    while (fightResult == FightResult.NONE)
    {
      fightResult = RunTurns(ref gameContents, ref node.enemy);
    }

    Terminal.PrintFightResult(fightResult);

    if (node.fightType == FightType.ELITE || node.isMystery)
    {
      OfferReward(ref gameContents, node.guild, node.enemy.marketSize);
    }

    gameContents.player.health = Battle.Player.health;

    Battle.End();

    return fightResult;
  }

  private static FightResult RunTurns(
    ref GameContents gameContents,
    ref Enemy enemy)
  {
    Terminal.PrintBattleState();

    Terminal.Turn(gameContents.player.name);
    ExecuteTurnForPlayer();

    var resultPlayerAction = GetFightResult();
    if (resultPlayerAction != FightResult.NONE)
    {
      return resultPlayerAction;
    }

    Terminal.PrintBattleState();

    Terminal.Turn(enemy.name);
    ExecuteTurnForComputer(ref enemy);

    return GetFightResult();
  }

  private static FightResult GetFightResult()
  {
    if (Battle.Player.health <= 0)
    {
      return FightResult.PLAYER_LOSE;
    }
    
    if (Battle.Player.health <= 0)
    {
      return FightResult.PLAYER_WIN;
    }

    return FightResult.NONE;
  }

  private static void ExecuteTurnForPlayer()
  {
    Battle.Hand.Draw_Full();

    BattlePhases.RunPlayCardsPhase();


    Terminal.PrintMarket(Battle.Market.GetDisplayedCards_All());

    Terminal.PrintBattleState();

    var canRecruit = Battle.Market.GetDisplayedCards_Affordable(Battle.Player.power, Battle.Player.manna).Count > 0;
    var playerTurnAction = BattlePhases.RunActionSelectionPhase(canRecruit);

    Battle.Player.isDefending = playerTurnAction == PlayerTurnAction.DEFEND;

    switch (playerTurnAction)
    {
      case PlayerTurnAction.ATTACK:
        PlayerTurnActionLogic.RunAttackAction();
        break;
      case PlayerTurnAction.DEFEND:
        PlayerTurnActionLogic.RunDefendAction();
        break;
      case PlayerTurnAction.RECRUIT:
        PlayerTurnActionLogic.RunRecruitAction();
        break;
      case PlayerTurnAction.NONE:
      default:
        PlayerTurnActionLogic.RunPassAction();
        break;
    }

    Battle.MoveHandToGraveyard();
    Battle.MoveFieldToGraveyard();
  }

  private static void ExecuteTurnForComputer(ref Enemy enemy)
  {
    var nextEnemyTurnAction = enemy.turnActions.First();
    enemy.turnActions.Remove(nextEnemyTurnAction);
    enemy.turnActions = enemy.turnActions.Append(nextEnemyTurnAction).ToList();

    var key = nextEnemyTurnAction.Item1;
    var value = nextEnemyTurnAction.Item2;

    Battle.Enemy.isDefending = key == EnemyTurnAction.D;

    switch (key)
    {
      case EnemyTurnAction.B:
        EnemyTurnActionLogic.RunBuffAction(value);
        break;
      case EnemyTurnAction.A:
        EnemyTurnActionLogic.RunAttackAction();
        break;
      case EnemyTurnAction.D:
        EnemyTurnActionLogic.RunDefendAction();
        break;
      case EnemyTurnAction.L:
        EnemyTurnActionLogic.RunLeechAction();
        break;
      case EnemyTurnAction.N:
      default:
        EnemyTurnActionLogic.RunPassAction();
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