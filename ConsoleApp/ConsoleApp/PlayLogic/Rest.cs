using System.Linq;
using MaM.Definitions;
using MaM.Enums;
using MaM.Helpers;

namespace MaM.PlayLogic;

public static class Rest
{
  public static FightResult Run(Campsite node, ref GameContents gameContents)
  {
    Terminal.PrintHealth(gameContents.player.health);
    Terminal.OfferCampsiteExchange();

    var choice = UserInput.GetInt();

    if (choice == 1)
    {
      return ExecuteCardForLifeExchange(ref gameContents);
    }
    else
    {
      return ExecuteLifeForCardExchange(ref gameContents, node.countCardsOnOffer);
    }
  }

  private static FightResult ExecuteCardForLifeExchange(ref GameContents gameContents)
  {
    var cardsToSacrifice = gameContents.player.GetDeck()
      .Where(card => (card.powerCost + card.mannaCost) > 0 && card.guild != Guild.NEUTRAL).ToList();

    Terminal.PromptExchangeCardForLife(cardsToSacrifice);

    var cardChosenIndex = UserInput.GetInt() - 1;
    var cardChosen = cardsToSacrifice[cardChosenIndex];

    gameContents.player.health += (cardChosen.mannaCost + cardChosen.powerCost) * 2;
    gameContents.player.RemoveFromDeck(cardChosen);

    Terminal.PrintHealth(gameContents.player.health);

    return FightResult.NONE;
  }

  private static FightResult ExecuteLifeForCardExchange(ref GameContents gameContents, int countCardsOnOffer)
  {
    var nonNeutralCards = gameContents.cards
      .Where(card => (card.powerCost + card.mannaCost) > 0 && card.guild != Guild.NEUTRAL).ToList();
    nonNeutralCards.Shuffle();

    var cardsOnOffer = nonNeutralCards.Take(countCardsOnOffer).ToList();

    Terminal.PromptExchangeLifeForCard(ref cardsOnOffer);

    var cardChosenIndex = UserInput.GetInt() - 1;
    var cardChosen = cardsOnOffer[cardChosenIndex];

    gameContents.player.health -= (cardChosen.mannaCost + cardChosen.powerCost) * 2;
    gameContents.player.AddToDeck(cardChosen);

    if (gameContents.player.health <= 0)
    {
      return FightResult.PLAYER_LOSE;
    }

    Terminal.PrintHealth(gameContents.player.health);

    return FightResult.NONE;
  }
}