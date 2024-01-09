using System;
using System.Collections.Generic;
using System.Linq;
using MaM.GameLogic;
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

    SetupMarket();

    SetupDeck();

    SetupHand();

    SetupGraveyard();

    SetupScrapheap();
  }

  private void SetupMarket()
  {
    var m1 = gameContents.player.deck.Where(card => card.guild != Guild.NEUTRAL).ToList();
    var m2 = gameContents.cards.Where(card => card.guild == node.guild).ToList();

    market = new Market(
      node.enemy.marketSize,
      m1,
      m2,
      ref gameContents.random
    );

    market.Fill();
  }

  private void SetupDeck()
  {
    var startingCards = gameContents.cards.Where(card => card.guild == Guild.NEUTRAL).ToList();
    startingCards.Shuffle(ref gameContents.random);
    deck = new Stack<Card>(startingCards);
  }

  private void SetupHand()
  {
    hand = new Hand(gameContents.handSize);
    hand.Fill(ref deck);
  }

  private void SetupGraveyard()
  {
    graveyard = new List<Card>();
  }

  private void SetupScrapheap()
  {
    scrapheap = new List<Card>();
  }

  public void Mulligan(ref Random random)
    {
      var cardsInHand = hand.GetAllCardsInHand();
      foreach (var card in cardsInHand)
      {
        deck.Push(card);
      }

      var shuffledDeck = deck.ToList();
      shuffledDeck.Shuffle(ref random);

      hand.Clear();
      hand.Fill(ref deck);

      market.Cycle();
    }
}