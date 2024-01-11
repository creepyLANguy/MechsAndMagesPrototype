using System;
using System.Collections.Generic;
using MaM.Helpers;

namespace MaM.Definitions;

public class Hand
{
  private int defaultSize;
  private List<Card> current = new();

  public Hand(int defaultSize)
  {
    this.defaultSize = defaultSize;
  }

  public bool Draw_Full(ref Stack<Card> deck, ref List<Card> graveyard, ref Random random)
  {
    var cardsToDraw = defaultSize - current.Count;

    if (cardsToDraw <= 0)
    {
        return false;
    }

    while (deck.Count > 0)
    {
      Draw_Single(ref deck);

      if (current.Count == defaultSize)
      {
        break;
      }
    }

    if (current.Count != defaultSize)
    {
      graveyard.Shuffle(ref random);
      deck = new Stack<Card>(new List<Card>(graveyard));

      while (deck.Count > 0)
      {
        Draw_Single(ref deck);

        if (current.Count == defaultSize)
        {
          break;
        }
      }
    }

    return current.Count != defaultSize;
  }

  public void Draw_Single(ref Stack<Card> deck)
  {
    current.Add(deck.Pop());
  }

  public void Clear()
  {
    current.Clear();
  }

  public void Remove_Single(Card card)
  {
    current.Remove(card);
  }

  public List<Card> GetAllCardsInHand()
  {
    return current;
  }

  public Card GetCardAtIndex(int index)
  {
    return current[index];
  }
}