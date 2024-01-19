using System;

namespace MaM.Helpers;

public static class UserInput
{
  public static string GetString()
  {
    var input = Console.ReadLine();

    while (string.IsNullOrEmpty(input))
    {
      Console.ReadLine();
    }

    return input;
  }

  public static int GetInt(int? debugModeDefaultValue = null)
  {
#if DEBUG
    return debugModeDefaultValue ?? int.Parse(GetString());
#else
    return int.Parse(GetString());
#endif
  }
}