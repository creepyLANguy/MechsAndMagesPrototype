using System.Collections.Generic;
using System.Linq;
using MaM.Enums;
using MaM.Helpers;

namespace MaM.Definitions;

public class BattlePack
{
  public Market market;

  public Stack<Card> deck;

  public Hand hand;

  public List<Card> graveyard;

  public List<Card> scrapheap;

  private Fight node;
  private GameContents gameContents;

  public BattlePack(Fight node, ref GameContents gameContents)
  { 
    this.node = node;
    this.gameContents = gameContents;

    SetupMarket(node.fightType);

    SetupDeck();

    SetupHand();

    SetupGraveyard();

    SetupScrapheap();
  }

  private void SetupMarket(FightType fightType)
  {
    var playerCardsContribution = gameContents.player.GetDeck().Where(card => card.guild != Guild.NEUTRAL).ToList();

    var enemyCardsContribution =
      fightType == FightType.BOSS
        ? gameContents.cards
        : gameContents.cards.Where(card => card.guild == node.guild).ToList();

        market = new Market(
      node.enemy.marketSize,
      playerCardsContribution,
      enemyCardsContribution
    );

    market.Fill();
  }

  private void SetupDeck()
  {
    var startingCards = gameContents.player.GetDeck().Where(card => card.guild == Guild.NEUTRAL).ToList();
    startingCards.Shuffle();
    deck = new Stack<Card>(startingCards);
  }

  private void SetupHand()
  {
    hand = new Hand(gameContents.handSize);
    hand.Draw_Full(ref deck, ref graveyard);
  }

  private void SetupGraveyard()
  {
    graveyard = new List<Card>();
  }

  private void SetupScrapheap()
  {
    scrapheap = new List<Card>();
  }

  public void Mulligan()
    {
      var cardsInHand = hand.GetAllCardsInHand();
      foreach (var card in cardsInHand)
      {
        deck.Push(card);
      }

      var shuffledDeck = deck.ToList();
      shuffledDeck.Shuffle();

      hand.Clear();
      hand.Draw_Full(ref deck, ref graveyard);

      market.Cycle();
    }

  public void MoveHandToGraveyard()
  {
    graveyard.AddRange(hand.GetAllCardsInHand());
    hand.Clear();
  }
}