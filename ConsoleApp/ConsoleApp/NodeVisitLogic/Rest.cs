using System.Linq;
using MaM.Definitions;
using MaM.Enums;
using MaM.Helpers;

namespace MaM.NodeVisitLogic;

public static class Rest
{
  public static FightResult Run(Campsite node, ref GameContents gameContents)
  {
    ConsoleMessages.PrintHealth(gameContents.player.health);
    ConsoleMessages.OfferCampsiteExchange();

    var choice = UserInput.GetInt();

    if (choice == 1)
    {
      return ExecuteCardForLifeExchange(node, ref gameContents);
    }
    else
    {
      return ExecuteLifeForCardExchange(node, ref gameContents);
    }
  }

  private static FightResult ExecuteCardForLifeExchange(Campsite node, ref GameContents gameContents)
  {
    var cardsToSacrifice = gameContents.player.GetDeck()
      .Where(card => (card.powerCost + card.mannaCost) > 0 && card.guild != Guild.NEUTRAL).ToList();

    ConsoleMessages.PromptExchangeCardForLife(ref gameContents.player, cardsToSacrifice);

    var cardChosenIndex = UserInput.GetInt() - 1;
    var cardChosen = cardsToSacrifice[cardChosenIndex];

    gameContents.player.health += (cardChosen.mannaCost + cardChosen.powerCost) * 2;
    gameContents.player.RemoveFromDeck(cardChosen);

    ConsoleMessages.PrintHealth(gameContents.player.health);

    return FightResult.NONE;
  }

  private static FightResult ExecuteLifeForCardExchange(Campsite node, ref GameContents gameContents)
  {
    var nonNeutralCards = gameContents.cards
      .Where(card => (card.powerCost + card.mannaCost) > 0 && card.guild != Guild.NEUTRAL).ToList();
    nonNeutralCards.Shuffle();

    var cardsOnOffer = nonNeutralCards.Take(node.countCardsOnOffer).ToList();

    ConsoleMessages.PromptExchangeLifeForCard(ref cardsOnOffer);

    var cardChosenIndex = UserInput.GetInt() - 1;
    var cardChosen = cardsOnOffer[cardChosenIndex];

    gameContents.player.health -= (cardChosen.mannaCost + cardChosen.powerCost) * 2;
    gameContents.player.AddToDeck(cardChosen);

    if (gameContents.player.health <= 0)
    {
      return FightResult.PLAYER_LOSE;
    }

    ConsoleMessages.PrintHealth(gameContents.player.health);

    return FightResult.NONE;
  }
}