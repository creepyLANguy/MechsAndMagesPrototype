using System;
using System.Collections.Generic;

namespace MaM.Helpers
{
 public static class Algos
  {
    public static void Shuffle<T>(this IList<T> list, ref Random random)
    {
      var i = list.Count;
      while (i > 1)
      {
        --i;
        var randomIndex = random.Next(i + 1);
        var value = list[randomIndex];
        list[randomIndex] = list[i];
        list[i] = value;
      }
    }
  }
}
