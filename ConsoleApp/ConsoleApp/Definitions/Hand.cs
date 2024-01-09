using System.Collections.Generic;

namespace MaM.Definitions;

public class Hand
{
  private int handSize;
  private List<Card> cards = new();

  public Hand(int handSize)
  {
    this.handSize = handSize;
  }

  public bool Fill(ref Stack<Card> deck)
  {
    var cardsToDraw = handSize - cards.Count;

    if (cardsToDraw <= 0)
    {
        return false;
    }

    while (deck.Count > 0)
    {
      cards.Add(deck.Pop());

      if (cards.Count == handSize)
      {
        break;
      }
    }

    return cards.Count == handSize;
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