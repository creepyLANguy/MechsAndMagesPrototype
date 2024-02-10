using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Enums;

namespace MaM.Helpers;

public class Hand
{
  private int baseSize;
  private List<Card> current = new();

  public Hand(int baseSize)
  {
    this.baseSize = baseSize;
  }

  private static void MoveGraveyardToDeck()
  {
    Battle.Graveyard.Shuffle();
    while (Battle.Graveyard.Count > 0)
    {
      Battle.Deck.Push(Battle.Graveyard[0]);
      Battle.Graveyard.RemoveAt(0);
    }
  }

  public bool Draw_Full()
  {
    var cardsToDraw = baseSize - current.Count;

    if (cardsToDraw <= 0)
    {
      return false;
    }

    while (Battle.Deck.Count > 0)
    {
      Draw_Single();

      if (current.Count == baseSize)
      {
        break;
      }
    }

    if (current.Count != baseSize)
    {
      MoveGraveyardToDeck();

      while (Battle.Deck.Count > 0)
      {
        Draw_Single();

        if (current.Count == baseSize)
        {
          break;
        }
      }
    }

    return current.Count != baseSize;
  }

  public bool Draw_Single()
  {
    if (Battle.Deck.Count == 0 && Battle.Graveyard.Count > 0)
    {
      MoveGraveyardToDeck();
    }
    else if (Battle.Deck.Count == 0)
    {
      return false;
    }

    current.Add(Battle.Deck.Pop());

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