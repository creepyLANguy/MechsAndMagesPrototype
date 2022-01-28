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
    public Card() { }

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

  public class Potion
  {
    public int Id   { get; set; }
    public int Cost { get; set; }

    //TODO - figure out how to structure the rest of this. Effects gonna be tricky to implement.
  }

  public class Player
  {
    public Player() { }

    public Player(
      bool          isComputer,
      string        name,
      List<Guild>   guilds,
      int           health,
      int           maxHealth,
      int           coin,
      int           vision,
      int           tradeRowSize,
      int           shield,
      int           manna,
      Stack<Card>   deck,
      Stack<Card>   drawPile,
      Stack<Card>   discardPile,
      Stack<Card>   tradeRow,
      Stack<Card>   tradePool,
      List<Potion>  potions
    )
    {
      IsComputer    = isComputer;
      Name          = name;
      Guilds        = guilds;
      Health        = health;
      MaxHealth     = maxHealth;
      Coin          = coin;
      Vision        = vision;
      TradeRowSize  = tradeRowSize;
      Shield        = shield;
      Manna         = manna;
      Deck          = deck;
      DrawPile      = drawPile;
      DiscardPile   = discardPile;
      TradeRow      = tradeRow;
      TradePool     = tradePool;
      Potions       = potions;
    }

    public bool         IsComputer    { get; set; }
    public string       Name          { get; set; }
    public List<Guild>  Guilds        { get; set; }
    public int          Health        { get; set; }
    public int          MaxHealth     { get; set; }
    public int          Coin          { get; set; }
    public int          Vision        { get; set; }
    public int          TradeRowSize  { get; set; }
    public int          Shield        { get; set; }
    public int          Manna         { get; set; }
    public Stack<Card>  Deck          { get; set; }
    public Stack<Card>  DrawPile      { get; set; }
    public Stack<Card>  DiscardPile   { get; set; }
    public Stack<Card>  TradeRow      { get; set; }
    public Stack<Card>  TradePool     { get; set; }
    public List<Potion> Potions       { get; set; }
  }

  public class Node
  {
    public Node() { }

    //TODO - full constructor

    public bool         IsMystery     { get; set; }
    public bool         IsVisible     { get; set; }
    public int          Index          { get; set; }
    public bool         IsComplete    { get; set; }
    public List<Guild>  Guilds        { get; set; }
  }
  
  public class Campsite : Node
  {
    public Campsite()
    {
    }

    //TODO - full constructor and call to base constructor

    public List<Card>   Recruits  { get; set; }
    public List<Potion> Potions   { get; set; }

    //TODO - implement these types
    //public List<HealingKit> HealingKits { get; set; }
    //public List<VisionUpgrade> VisionUpgrades { get; set; }
    //public List<HealthUpgrade> HealthUpgrades { get; set; }
    //public Scrapper Scrapper { get; set; }
  }
  public enum ConflictType
  {
    Normal,
    Elite,
    Boss
  }

  public class Conflict : Node
  {
    public Conflict() { }

    public Conflict(
      List<Player>  opponents,
      int           maxRounds,
      int           currentRound,
      List<Potion>  rewardPotions,
      List<Potion>  rewardRecruits,
      ConflictType  conflictType
      //TODO - complete this constructor and call to base constructor
    )
    {
      Opponents       = opponents;
      MaxRounds       = maxRounds;
      CurrentRound    = currentRound;
      RewardPotions   = rewardPotions;
      RewardRecruits  = rewardRecruits;
      ConflictType    = conflictType;
    }

    public List<Player> Opponents       { get; set; }
    public int          MaxRounds       { get; set; }
    public int          CurrentRound    { get; set; }
    public List<Potion> RewardPotions   { get; set; }
    public List<Potion> RewardRecruits  { get; set; }
    public ConflictType ConflictType    { get; set; }
  }

  public class Map
  {
    public Map() { }
    public Map(LinkedList<Node> nodes)
    {
      Nodes = nodes;
    }

    //TODO - implement the directed graph version of this. 
    //public DirectedGraph<Node> Nodes { get; set; }
    public LinkedList<Node> Nodes { get; set; }
  }

  public class Journey
  {
    public Journey() { }

    public Journey(List<Map> maps)
    {
      Maps = maps;
    }

    public List<Map> Maps { get; set; }
  }

}