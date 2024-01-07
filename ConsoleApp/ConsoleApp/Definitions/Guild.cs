using System.Collections.Generic;

namespace MaM.Definitions;

public class Guild : KeyValuePair<string, int>
{
  public Guild(string key, int val) : base(key, val) { }
}

public struct Guilds
{
  public static readonly Guild Neutral = new("Neutral", 0);
  public static readonly Guild Borg = new("Borg", 1);
  public static readonly Guild Mech = new("Mech", 2);
  public static readonly Guild Mage = new("Mage", 3);
  public static readonly Guild Necro = new("Necro", 4);

  // Important - assign in the same order as the value for each guild.
  public static readonly List<Guild> All = new() { Neutral, Borg, Mech, Mage, Necro };
}