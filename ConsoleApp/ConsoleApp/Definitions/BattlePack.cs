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

    public BattlePack(Fight node, ref GameContents gameContents)
    {
        market = new Market(
          node.enemy.marketSize,
          gameContents.player.deck.Where(card => card.guild != Guild.NEUTRAL).ToList(),
          gameContents.cards.Where(card => card.guild == node.guild).ToList(),
          ref gameContents.random
        );

        var startingCards = gameContents.cards.Where(card => card.guild == Guild.NEUTRAL).ToList();
        startingCards.Shuffle(ref gameContents.random);
        deck = new Stack<Card>(startingCards);

        hand = new Hand(gameContents.handSize);
        hand.Fill(ref deck);

        graveyard = new List<Card>();

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
  }
}