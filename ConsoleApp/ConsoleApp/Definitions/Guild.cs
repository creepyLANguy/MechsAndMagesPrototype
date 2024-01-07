using System;
using System.Collections.Generic;

namespace MaM.Definitions;

public class Guild : Tuple<string, int>
{
  public Guild(string key, int val) : base(key, val) { }
}

public struct Guilds
{
  public static readonly Guild Neutral = new("Neutral", 0);
  public static readonly Guild Green = new("Green", 1);
  public static readonly Guild Red = new("Red", 2);
  public static readonly Guild Blue = new("Blue", 3);
  public static readonly Guild Black = new("Black", 4);

  // Important - assign in the same order as the value for each guild.
  public static readonly List<Guild> All = new() { Neutral, Green, Red, Blue, Black};
}