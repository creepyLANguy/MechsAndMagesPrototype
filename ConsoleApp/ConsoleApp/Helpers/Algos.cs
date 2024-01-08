using System;
using System.Collections.Generic;

namespace MaM.Helpers;

public static class Algos
{
  public static void Shuffle<T>(this IList<T> list, ref Random random)
  {
    for (var i = list.Count - 1; i > 0; --i)
    {
      var randomIndex = random.Next(i + 1);
      (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
    }
  }

  public static int GenerateRandomSeed() => 
    Math.Abs((int)DateTime.Now.Ticks);

  public static Random GenerateNewRandom(int? randomSeed = null)
    => new Random(randomSeed ?? GenerateRandomSeed());
}