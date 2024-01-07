using System.Collections.Generic;

namespace MaM.Definitions;

public struct CardTypes
{
  public static readonly CardType Unknown = new("Unknown", 0);
  public static readonly CardType Unit    = new("Unit",    1);
  public static readonly CardType Base    = new("Base",    2);

  public static readonly List<CardType> All = new() { Unit, Base, Unknown };
}