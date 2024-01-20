using System;
using System.Collections.Generic;

namespace MaM.Helpers
{
  public enum UbiRandomCallType
  {
    NEXT,
    NEXT_UPPER,
    Next_LOWER_UPPER,
    Next_DOUBLE
  }

  public static class UbiRandom
  {
    [ThreadStatic]
    private static Random _random;

    private static int? _seed;

    private static int GenerateRandomSeed() => Math.Abs((int)DateTime.Now.Ticks);

    private static UbiRandomCallHistory _callHistory = new();

    private static void EnsureInitialized()
    {
      _seed ??= GenerateRandomSeed();
      _random ??= new Random((int)_seed);
    }

    public static void ForceInitialise(int seed, UbiRandomCallHistory history = null)
    {
      _seed = seed;
      _random = new Random((int)_seed);
      
      _callHistory.Clear();

      if (history == null)
      {
        return;
      }

      foreach (var call in history)
      {
        switch (call.Item1)
        {
          case UbiRandomCallType.NEXT:
            Next();
            break;
          case UbiRandomCallType.NEXT_UPPER:
            Next(call.Item2[0]);
            break;
          case UbiRandomCallType.Next_LOWER_UPPER:
            Next(call.Item2[0], call.Item2[1]);
            break;
          case UbiRandomCallType.Next_DOUBLE:
            NextDouble();
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    public static UbiRandomCallHistory GetCallHistory()
      => _callHistory;

    public static int GetCurrentSeed()
    {
      _seed ??= GenerateRandomSeed();
      return (int)_seed;
    }

    public static int Next()
    {
      EnsureInitialized();

      _callHistory.Add(new UbiRandomCall(UbiRandomCallType.NEXT, null));

      return _random.Next();
    }

    public static int Next(int upperExclusive)
    {
      EnsureInitialized();

      _callHistory.Add(new UbiRandomCall(UbiRandomCallType.NEXT_UPPER, new List<int>{ upperExclusive }));

      return _random.Next(upperExclusive);
    }

    public static int Next(int inclusiveLower, int upperExclusive)
    {
      EnsureInitialized();

      _callHistory.Add(new UbiRandomCall(UbiRandomCallType.Next_LOWER_UPPER, new List<int>{inclusiveLower, upperExclusive}));

      return _random.Next(inclusiveLower, upperExclusive);
    }

    public static double NextDouble()
    {
      EnsureInitialized();

      _callHistory.Add(new UbiRandomCall(UbiRandomCallType.Next_DOUBLE, null));

      return _random.NextDouble();
    }
  }
}