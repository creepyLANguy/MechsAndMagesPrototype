using System.Collections.Generic;
using System.Linq;
using MaM.GameLogic;
using MaM.Helpers;

namespace MaM.Definitions;

public class BattlePack
{
    public Traderow traderow;

    public Stack<Card> deck;

    public Hand hand;

    public List<Card> graveyard;

    public List<Card> scrapheap;

    public BattlePack(Fight node, ref GameContents gameContents)
    {
        traderow = new Traderow(
          node.enemy.tradeRowSize,
          gameContents.player.deck.Where(card => card.guild != Guild.NEUTRAL).ToList(),
          gameContents.cards.Where(card => card.guild == node.guild).ToList(),
          ref gameContents.random
        );

        var startingCards = gameContents.player.deck.Where(card => card.guild == Guild.NEUTRAL).ToList();
        startingCards.Shuffle(ref gameContents.random);
        deck = new Stack<Card>(startingCards);

        hand = new Hand(gameContents.handSize);
        hand.Fill(ref deck);

        graveyard = new List<Card>();

        scrapheap = new List<Card>();
    }
}