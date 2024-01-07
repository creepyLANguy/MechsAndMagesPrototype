using System.Collections.Generic;

namespace MaM.Definitions;

public static class Actions
{
  public static readonly Action Attack          = new("A", 1);
  public static readonly Action Trade           = new("T", 2);
  public static readonly Action Draw            = new("D", 3);
  public static readonly Action Scrap           = new("S", 4);
  public static readonly Action Consume         = new("C", 5);
  public static readonly Action OpponentDiscard = new("O", 6);
  public static readonly Action Heal            = new("H", 7);

  public static readonly List<Action> All = new() { Attack, Trade, Draw, Scrap, Consume, OpponentDiscard, Heal };
}