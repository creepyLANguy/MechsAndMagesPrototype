using System;
using System.Collections.Generic;
using MaM.Helpers;

namespace MaM.Definitions;

public class Hand
{
  private int handSize;
  private List<Card> cards = new();

  public Hand(int handSize)
  {
    this.handSize = handSize;
  }

  public bool Draw_Full(ref Stack<Card> deck, ref List<Card> graveyard, ref Random random)
  {
    var cardsToDraw = handSize - cards.Count;

    if (cardsToDraw <= 0)
    {
        return false;
    }

    while (deck.Count > 0)
    {
      Draw_Single(ref deck);

      if (cards.Count == handSize)
      {
        break;
      }
    }

    if (cards.Count != handSize)
    {
      graveyard.Shuffle(ref random);
      deck = new Stack<Card>(new List<Card>(graveyard));

      while (deck.Count > 0)
      {
        Draw_Single(ref deck);

        if (cards.Count == handSize)
        {
          break;
        }
      }
    }

    return cards.Count != handSize;
  }

  public void Draw_Single(ref Stack<Card> deck)
  {
    cards.Add(deck.Pop());
  }

  public void Clear()
  {
    cards.Clear();
  }

  public void Remove_Single(Card card)
  {
    cards.Remove(card);
  }

  public List<Card> GetAllCardsInHand()
  {
    return cards;
  }
}