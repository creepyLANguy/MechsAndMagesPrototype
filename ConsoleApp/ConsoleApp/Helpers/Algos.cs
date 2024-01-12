using System;
using System.Collections.Generic;

namespace MaM.Helpers;

public static class Algos
{
  public static void Shuffle<T>(this IList<T> list)
  {
    for (var i = list.Count - 1; i > 0; --i)
    {
      var randomIndex = UbiRandom.Next(i + 1);
      (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
    }
  }
}