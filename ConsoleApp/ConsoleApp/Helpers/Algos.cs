using System.Collections.Generic;

namespace MaM.Helpers;

public static class Algos
{
  public static void Shuffle<T>(this IList<T> list)
  {
    var temp = new List<T>();

    while (list.Count > 0)
    {
      var randomIndex = UbiRandom.Next(list.Count);
      temp.Add(list[randomIndex]);
      list.RemoveAt(randomIndex);
    }

    list.Clear();
    foreach (var item in temp)
    {
      list.Add(item);
    }
  }
}