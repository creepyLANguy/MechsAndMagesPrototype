using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Helpers;

namespace MaM.Definitions;

public class Market
{
  private int marketSize;
  private Stack<Card> pool = new();
  private List<Card> display = new();

  public Market(int marketSize, List<Card> cards_player, List<Card> cards_enemy)
  {
    this.marketSize = marketSize;

    cards_player.Shuffle();
    cards_enemy.Shuffle();

    var mergedList = MergeListsAlternating(cards_player, cards_enemy);
    mergedList.Reverse(0, mergedList.Count);

    pool = new Stack<Card>(mergedList);
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

  public Card? TryFetch(int index, ref BattlePack b)
  {
    var card = display[index];

    if (card.powerCost > b.player.power)
    {
      return null;
    }

    if (card.mannaCost > b.player.manna)
    {
      return null;
    }

    if (pool.Count > 0)
    {
      display[index] = pool.Pop();
    }
    else
    {
      marketSize -= 1;
      display.Remove(card);
    }

    b.player.power -= card.powerCost;
    b.player.manna -= card.mannaCost;

    return card;
  }

  public void Cycle()
  {
    var tempPool = pool.ToList();
    tempPool.AddRange(display);
    tempPool.Reverse(0, tempPool.Count);
    pool = new Stack<Card>(tempPool);

    display.Clear();
    Fill();
  }

  public List<Card> GetDisplayedCards_All()
  {
    return display;
  }

  public List<Card> GetDisplayedCards_Affordable(int power, int manna)
    {
      return display.Where(card => power >= card.powerCost && manna >= card.mannaCost).ToList();
    }
}