using System.Collections.Generic;

namespace TryJsonToObject
{
  public class KeyValuePair<TK, TV>
  {
    internal TK Key   { get; set; }
    internal TV Value { get; set; }

    protected KeyValuePair(TK key, TV value)
    {
      Key   = key;
      Value = value;
    }
  }

  public class Guild: KeyValuePair<string, int> { public Guild(string key, int val) : base(key, val) { } }
  public static class Guilds
  {
    public static readonly Guild Neutral = new Guild("Neutral", 0);
    public static readonly Guild Borg    = new Guild("Borg",    1);
    public static readonly Guild Mech    = new Guild("Mech",    2);
    public static readonly Guild Mage    = new Guild("Mage",    3);
    public static readonly Guild Necro   = new Guild("Necro",   4);

    public static readonly List<Guild> All = new List<Guild>() { Borg, Mech, Mage, Necro, Neutral };
  }

  public class Action : KeyValuePair<string, int> { public Action(string key, int val) : base(key, val) { } }
  public static class Actions
  {
    public static Action Attack          = new Action("A", 1);
    public static Action Trade           = new Action("T", 2);
    public static Action Draw            = new Action("D", 3);
    public static Action Scrap           = new Action("S", 4);
    public static Action Consume         = new Action("C", 5);
    public static Action OpponentDiscard = new Action("O", 6);
    public static Action Heal            = new Action("H", 7);

    public static List<Action> All = new List<Action>() { Attack, Trade, Draw, Scrap, Consume, OpponentDiscard, Heal };
  }

  public class ActionsValuesSet
  {
    public int Attack          { get; set; }
    public int Trade           { get; set; }  
    public int Draw            { get; set; }
    public int Scrap           { get; set; }
    public int Consume         { get; set; }
    public int OpponentDiscard { get; set; }
    public int Heal            { get; set; }
  }

  public class CardType : KeyValuePair<string, int> { public CardType(string key, int val) : base(key, val) { } }
  public static class CardTypes
  {
    public static CardType Unknown = new CardType("Unknown", 0);
    public static CardType Unit    = new CardType("Unit",    1);
    public static CardType Base    = new CardType("Base",    2);

    public static readonly List<CardType> All = new List<CardType>() { Unit, Base, Unknown };
  }

  public class Card
  {
    public Card() {}

    public Card(
      int id,
      string name,
      CardType type,
      Guild guild,
      int cost,
      int defense,
      int shield,
      ActionsValuesSet defaultActionsValues,
      ActionsValuesSet guildActionsValues,
      ActionsValuesSet allyActionsValues,
      ActionsValuesSet scrapActionsValues
    )
    {
      Id                = id;
      Name              = name;
      Type              = type;
      Guild             = guild;
      Cost              = cost;
      Defense           = defense;
      Shield            = shield;
      DefaultAbilities  = defaultActionsValues;
      GuildBonuses      = guildActionsValues;
      AllyBonuses       = allyActionsValues;
      ScrapBonuses      = scrapActionsValues;
    }

    public string            Name              { get; set; }
    public CardType          Type              { get; set; }
    public Guild             Guild             { get; set; }
    public int               Cost              { get; set; }
    public int               Defense           { get; set; }
    public int               Shield            { get; set; }
    public ActionsValuesSet  DefaultAbilities  { get; set; }
    public ActionsValuesSet  GuildBonuses      { get; set; }
    public ActionsValuesSet  AllyBonuses       { get; set; }
    public ActionsValuesSet  ScrapBonuses      { get; set; }
    public int               Id                { get; set; }
  }

}
