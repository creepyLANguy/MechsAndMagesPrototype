using System;

namespace MaM.Helpers;

public static class UserInput
{
  private static string GetString_Force()
  {
    var input = Console.ReadLine();

    while (string.IsNullOrEmpty(input))
    {
      Console.ReadLine();
    }

    return input;
  }

  public static string GetString(string debugModeDefaultValue = null)
  {
#if DEBUG
    return debugModeDefaultValue ?? GetString_Force();
#else
    return GetString_Force();
#endif
  }

  public static int GetInt(int? debugModeDefaultValue = null)
  {
#if DEBUG
    return debugModeDefaultValue ?? int.Parse(GetString());
#else
    return int.Parse(GetString_Force());
#endif
  }
}