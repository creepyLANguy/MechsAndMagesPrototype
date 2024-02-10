using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Enums;

namespace MaM.Helpers;

public class Hand
{
  private readonly int _baseSize;
  private readonly List<Card> _current = new();

  public Hand(int baseSize)
  {
    _baseSize = baseSize;
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
    var cardsToDraw = _baseSize - _current.Count;

    if (cardsToDraw <= 0)
    {
      return false;
    }

    while (Battle.Deck.Count > 0)
    {
      Draw_Single();

      if (_current.Count == _baseSize)
      {
        break;
      }
    }

    if (_current.Count != _baseSize)
    {
      MoveGraveyardToDeck();

      while (Battle.Deck.Count > 0)
      {
        Draw_Single();

        if (_current.Count == _baseSize)
        {
          break;
        }
      }
    }

    return _current.Count != _baseSize;
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

    _current.Add(Battle.Deck.Pop());

    return true;
  }

  public void Clear()
  {
    _current.Clear();
  }

  public void Remove_Single(Card card)
  {
      _current.Remove(card);
  }

  public int GetCurrentCount()
  {
    return _current.Count;
  }

  public List<Card> GetAllCardsInHand()
  {
    return _current;
  }

  public Card GetCardAtIndex(int index)
  {
    return _current[index];
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

    return _current.Any(card => orderSensitiveEffects.Contains(card.ability));
  }
}