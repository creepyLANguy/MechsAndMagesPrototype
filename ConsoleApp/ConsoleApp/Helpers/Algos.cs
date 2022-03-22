using System;
using System.Collections.Generic;

namespace MaM.Helpers
{
 public static class Algos
  {
    public static void Shuffle<T>(this IList<T> list, ref Random random)
    {
      for (var i = list.Count - 1; i > 0; --i)
      {
        var randomIndex = random.Next(i + 1);
        var value = list[randomIndex];
        list[randomIndex] = list[i];
        list[i] = value;
      }
    }

    public static int GenerateRandomSeed()
    {
      return Math.Abs((int) DateTime.Now.Ticks);
    }

    public static Random GenerateNewRandom(int? randomSeed = null)
    {
      return new Random(randomSeed ?? GenerateRandomSeed());
    }
  }
}
