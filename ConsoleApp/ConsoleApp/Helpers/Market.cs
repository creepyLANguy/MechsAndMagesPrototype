using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;

namespace MaM.Helpers;

public class Market
{
  private int _marketSize;
  private Stack<Card> _pool;
  private List<Card> _display = new();

  public Market(int marketSize, List<Card> playerCards, List<Card> enemyCards)
  {
    _marketSize = marketSize;

    playerCards.Shuffle();
    enemyCards.Shuffle();

    var mergedList = MergeListsAlternating(playerCards, enemyCards);
    mergedList.Reverse(0, mergedList.Count);

    _pool = new Stack<Card>(mergedList);
  }

  private static List<T> MergeListsAlternating<T>(List<T> list1, List<T> list2)
  {
    var mergedList = new List<T>();

    var maxLength = Math.Max(list1.Count, list2.Count);

    for (var i = 0; i < maxLength; i++)
    {
      if (i < list1.Count)
      {
        mergedList.Add(list1[i]);
      }

      if (i < list2.Count)
      {
        mergedList.Add(list2[i]);
      }
    }

    return mergedList;
  }

  public bool Fill()
  {
    if (_pool.Count == 0)
    {
      return false;
    }

    while (_display.Count < _marketSize)
    {
      _display.Add(_pool.Pop());
    }

    return true;
  }

  public Card? TryFetch(int index)
  {
    var card = _display[index];

    if (card.powerCost > Battle.Player.power)
    {
      return null;
    }

    if (card.mannaCost > Battle.Player.manna)
    {
      return null;
    }

    if (_pool.Count > 0)
    {
      _display[index] = _pool.Pop();
    }
    else
    {
      _marketSize -= 1;
      _display.Remove(card);
    }

    Battle.Player.power -= card.powerCost;
    Battle.Player.manna -= card.mannaCost;

    return card;
  }

  public void Cycle()
  {
    var tempPool = _pool.ToList();
    tempPool.AddRange(_display);
    tempPool.Reverse(0, tempPool.Count);
    _pool = new Stack<Card>(tempPool);

    _display.Clear();
    Fill();
  }

  public List<Card> GetDisplayedCards_All()
  {
    return _display;
  }

  public List<Card> GetDisplayedCards_Affordable(int power, int manna)
  {
    return _display.Where(card => power >= card.powerCost && manna >= card.mannaCost).ToList();
  }
}