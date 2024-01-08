using System;
using MaM.Definitions;
using System.Collections.Generic;
using MaM.Helpers;

namespace MaM.GameLogic;

public class Traderow
{
  private int traderowSize;
  private Stack<Card> pool = new();
  private List<Card> display = new();

  public Traderow(int traderowSize, List<Card> cards_player, List<Card> cards_enemy, ref Random random)
  {
    this.traderowSize = traderowSize;

    cards_player.Shuffle(ref random);
    cards_enemy.Shuffle(ref random);

    var mergedList = MergeListsAlternating(cards_player, cards_enemy);
    mergedList.Reverse(0, mergedList.Count);

    pool = new(mergedList);

    for (int i = 0; i < 10; i++)
    {
      Fill();
      Fetch(0);
      Fetch(0);
      Fetch(0);
      Fetch(0);
    }
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

    while (display.Count < traderowSize)
    {
      display.Add(pool.Pop());
    }

    return true;
  }

  public bool Fetch(int index)
  {
    if (pool.Count == 0)
    {
      traderowSize -= 1;
      display.RemoveAt(index);
      return false;
    }

    display[index] = pool.Pop();
    return true;
  }
}