using System;

namespace MaM.Definitions;

public class CardType : Tuple<string, int>
{
  public CardType(string key, int val) : base(key, val) { }
}