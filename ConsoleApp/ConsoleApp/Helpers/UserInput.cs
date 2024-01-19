using System;

namespace MaM.Helpers;

public static class UserInput
{
  public static string Get()
    => Console.ReadLine();

  public static string GetString()
  {   
    return Get();
  }

  public static int GetInt(int? debugModeDefaultValue = null)
  {
#if DEBUG
    if (debugModeDefaultValue != null)
    {
      return (int) debugModeDefaultValue;
    }
#endif

    var input = GetString();

    while (string.IsNullOrEmpty(input))
    {
      input = Get();
    }

    return int.Parse(input);
  }
}