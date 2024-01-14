using System;
using MaM.Definitions;

namespace MaM.Helpers;

public static class UserInput
{
  public static string Get()
    => Console.ReadLine();

  public static string GetString()
  {   
    return Get();
  }

  public static int GetInt()
  {
    var input = GetString();

    while (string.IsNullOrEmpty(input))
    {
      input = Get();
    }

    return int.Parse(input);
  }
}