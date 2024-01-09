using System;
using MaM.Definitions;
using System.Collections.Generic;
using MaM.Helpers;

namespace MaM.GameLogic;

public class Market
{
  private int marketSize;
  private Stack<Card> pool = new();
  private List<Card> display = new();

  public Market(int marketSize, List<Card> cards_player, List<Card> cards_enemy, ref Random random)
  {
    this.marketSize = marketSize;

    cards_player.Shuffle(ref random);
    cards_enemy.Shuffle(ref random);

    var mergedList = MergeListsAlternating(cards_player, cards_enemy);
    mergedList.Reverse(0, mergedList.Count);

    pool = new(mergedList);
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
    if (pool.Count == 0)
    {
      return false;
    }

    while (display.Count < marketSize)
    {
      display.Add(pool.Pop());
    }

    return true;
  }

  public bool Fetch(int index)
  {
    if (pool.Count == 0)
    {
      marketSize -= 1;
      display.RemoveAt(index);
      return false;
    }

    display[index] = pool.Pop();
    return true;
  }

  public List<Card> GetDisplayedCards()
  {
    return display;
  }
}