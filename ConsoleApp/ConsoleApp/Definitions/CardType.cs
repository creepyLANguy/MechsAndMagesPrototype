namespace MaM.Definitions;

public class CardType : KeyValuePair<string, int>
{
  public CardType(string key, int val) : base(key, val) { }
}