using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Enums;

namespace MaM.Helpers;

public class Hand
{
  private int defaultSize;
  private List<Card> current = new();

  public Hand(int defaultSize)
  {
    this.defaultSize = defaultSize;
  }

  private static void MoveGraveyardToDeck(ref Stack<Card> deck, ref List<Card> graveyard)
  {
    graveyard.Shuffle();
    while (graveyard.Count > 0)
    {
      deck.Push(graveyard[0]);
      graveyard.RemoveAt(0);
    }
  }

  public bool Draw_Full(ref Stack<Card> deck, ref List<Card> graveyard)
  {
    var cardsToDraw = defaultSize - current.Count;

    if (cardsToDraw <= 0)
    {
      return false;
    }

    while (deck.Count > 0)
    {
      Draw_Single(ref deck, ref graveyard);

      if (current.Count == defaultSize)
      {
        break;
      }
    }

    if (current.Count != defaultSize)
    {
      MoveGraveyardToDeck(ref deck, ref graveyard);

      while (deck.Count > 0)
      {
        Draw_Single(ref deck, ref graveyard);

        if (current.Count == defaultSize)
        {
          break;
        }
      }
    }

    return current.Count != defaultSize;
  }

  public bool Draw_Single(ref Stack<Card> deck, ref List<Card> graveyard)
  {
    if (deck.Count == 0 && graveyard.Count > 0)
    {
      MoveGraveyardToDeck(ref deck, ref graveyard);
    }
    else if (deck.Count == 0)
    {
      return false;
    }

    current.Add(deck.Pop());

    return true;
  }

  public void Clear()
  {
    current.Clear();
  }

  public void Remove_Single(Card card)
  {
      current.Remove(card);
  }

  public int GetCurrentCount()
  {
    return current.Count;
  }

  public List<Card> GetAllCardsInHand()
  {
    return current;
  }

  public Card GetCardAtIndex(int index)
  {
    return current[index];
  }

  public bool HasCardsWithOrderSensitiveEffects()
  { 
    var orderSensitiveEffects = new List<CardAbility>()
    {
      CardAbility.SHUN,
      CardAbility.DRAW,
      CardAbility.CYCLE,
      CardAbility.STOMP

    }; 

    return current.Any(card => orderSensitiveEffects.Contains(card.ability));
  }
}