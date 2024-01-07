namespace MaM.Definitions;

public class KeyValuePair<TK, TV>
{
  public TK Key;
  public TV Value;

  protected KeyValuePair(TK key, TV value)
  {
    Key   = key;
    Value = value;
  }
}