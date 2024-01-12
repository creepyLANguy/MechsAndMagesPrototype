using System;

namespace MaM.Helpers
{
  public static class UbiRandom
  {
    [ThreadStatic]
    private static Random _random;

    private static int? _seed;

    private static int GenerateRandomSeed() => Math.Abs((int)DateTime.Now.Ticks);

    private static void EnsureInitialized()
    {
      _seed ??= GenerateRandomSeed();
      _random ??= new Random((int)_seed);
    }

    public static int GetCurrentSeed()
    {
      _seed ??= GenerateRandomSeed();
      return (int)_seed;
    }

    public static int Next()
    {
      EnsureInitialized();
      return _random.Next();
    }

    public static int Next(int upperExclusive)
    {
      EnsureInitialized();
      return _random.Next(upperExclusive);
    }

    public static int Next(int inclusiveLower, int upperExclusive)
    {
      EnsureInitialized();
      return _random.Next(inclusiveLower, upperExclusive);
    }

    public static double NextDouble()
    {
      EnsureInitialized();
      return _random.NextDouble();
    }
  }
}