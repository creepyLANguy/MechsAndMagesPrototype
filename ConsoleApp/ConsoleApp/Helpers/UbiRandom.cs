using System;

namespace MaM.Helpers
{
  public static class UbiRandom
  {
    [ThreadStatic]
    private static Random _random;

    private static int? _seed;

    private static void MakeSureIsAlive()
    {
      _seed ??= Algos.GenerateRandomSeed();
      _random ??= new Random((int)_seed);
    }
    public static int GetSeed()
    {
      MakeSureIsAlive();
      return (int)_seed;
    }

    public static int Next()
    {
      MakeSureIsAlive();
      return _random.Next();
    }

    public static int Next(int upperExclusive)
    {
      MakeSureIsAlive();
      return _random.Next(upperExclusive);
    }

    public static int Next(int inclusiveLower, int upperExclusive)
    {
      MakeSureIsAlive();
      return _random.Next(inclusiveLower, upperExclusive);
    }

    public static double NextDouble()
    {
      MakeSureIsAlive();
      return _random.NextDouble();
    }
  }
}

