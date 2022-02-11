using System;
using System.Collections.Generic;

namespace MaM
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
    public static readonly Action Attack          = new Action("A", 1);
    public static readonly Action Trade           = new Action("T", 2);
    public static readonly Action Draw            = new Action("D", 3);
    public static readonly Action Scrap           = new Action("S", 4);
    public static readonly Action Consume         = new Action("C", 5);
    public static readonly Action OpponentDiscard = new Action("O", 6);
    public static readonly Action Heal            = new Action("H", 7);

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
    public static readonly CardType Unknown = new CardType("Unknown", 0);
    public static readonly CardType Unit    = new CardType("Unit",    1);
    public static readonly CardType Base    = new CardType("Base",    2);

    public static readonly List<CardType> All = new List<CardType>() { Unit, Base, Unknown };
  }

  public class Card
  {
    public Card(
      string            id,
      string            name,
      CardType          type,
      Guild             guild,
      int               cost,
      int               defense,
      int               shield,
      ActionsValuesSet  defaultActionsValues,
      ActionsValuesSet  guildActionsValues,
      ActionsValuesSet  allyActionsValues,
      ActionsValuesSet  scrapActionsValues
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
    public string            Id                { get; set; }
  }

  public class Potion
  {
    public int Id   { get; set; }
    public int Cost { get; set; }

    //TODO - figure out how to structure the rest of this. Effects gonna be tricky to implement.
  }

  public class Player
  {
    //TODO - remove this empty constructor.
    public Player(){}

    public Player(
      bool          isComputer,
      string        name,
      Guild         activeGuild,
      int           health,
      int           maxHealth,
      int           coin,
      int           vision,
      int           awareness,
      int           insight,
      int           tradeRowSize,
      int           shield,
      int           manna,
      int           basicManna,
      int           basicHandSize,
      int           currentHandSize,
      int           toDiscard,
      List<Card>    deck,
      List<Card>    drawPile,
      List<Card>    discardPile,
      List<Card>    tradeRow,
      List<Card>    tradePool,
      List<Potion>  potions
    )
    {
      IsComputer      = isComputer;
      Name            = name;
      ActiveGuild     = activeGuild;
      Health          = health;
      MaxHealth       = maxHealth;
      Coin            = coin;
      Vision          = vision;
      Awareness       = awareness;
      Insight         = insight;
      TradeRowSize    = tradeRowSize;
      Shield          = shield;
      BasicManna      = basicManna;
      Manna           = manna;
      BasicHandSize   = basicHandSize;
      CurrentHandSize = currentHandSize;
      ToDiscard       = toDiscard;
      Deck            = deck;
      DrawPile        = drawPile;
      DiscardPile     = discardPile;
      TradeRow        = tradeRow;
      TradePool       = tradePool;
      Potions         = potions;
    }

    public bool         IsComputer      { get; set; }
    public string       Name            { get; set; }
    public Guild        ActiveGuild     { get; set; }
    public int          Health          { get; set; }
    public int          MaxHealth       { get; set; }
    public int          Coin            { get; set; }
    public int          Vision          { get; set; } //How many nodes and paths ahead you can simply see
    public int          Awareness       { get; set; } //How many of the visible nodes ahead have their types revealed
    public int          Insight         { get; set; } //How many of the nodes ahead have their Guild distributions revealed
    public int          TradeRowSize    { get; set; }
    public int          Shield          { get; set; }
    public int          BasicManna      { get; set; }
    public int          Manna           { get; set; }
    public int          ToDiscard       { get; set; }
    public int          BasicHandSize   { get; set; }
    public int          CurrentHandSize { get; set; }
    public List<Card>   Deck            { get; set; }
    public List<Card>   DrawPile        { get; set; }
    public List<Card>   DiscardPile     { get; set; }
    public List<Card>   TradeRow        { get; set; }
    public List<Card>   TradePool       { get; set; }
    public List<Potion> Potions         { get; set; }
  }

  public enum NodeType
  {
    Blank,
    CampSite,
    Fight,
  }

  public class Node
  {
    public Node(Node node)
    {
      NodeType      = node.NodeType;
      IsMystery     = node.IsMystery;
      X             = node.X;            
      Y             = node.Y;            
      IsComplete    = node.IsComplete;
      IsDestination = node.IsDestination;
      Destinations  = node.Destinations;
    }

    public Node(
      NodeType                  nodeType,
      bool                      isMystery,
      int                       x,
      int                       y,
      bool                      isComplete,
      bool                      isDestination,
      HashSet<Tuple<int, int>>  destinations
    )
    {
      NodeType      = nodeType;
      IsMystery     = isMystery;
      X             = x;
      Y             = y;
      IsComplete    = isComplete;
      IsDestination = isDestination;
      Destinations  = destinations;
    }

    public NodeType                 NodeType       { get; set; }
    public bool                     IsMystery      { get; set; }
    public int                      X              { get; set; }
    public int                      Y              { get; set; }
    public bool                     IsComplete     { get; set; }    
    public bool                     IsDestination  { get; set; }
    public HashSet<Tuple<int, int>> Destinations   { get; set; }
  }
  
  public class Campsite : Node
  {
    public Campsite(
      Node         baseNode,
      List<Potion> potions
    ) : base(
      baseNode.NodeType,
      baseNode.IsMystery,
      baseNode.X,
      baseNode.Y,
      baseNode.IsComplete,
      baseNode.IsDestination,
      baseNode.Destinations
    )
    {
      Potions   = potions;
      NodeType = NodeType.CampSite;
    }

    public List<Potion> Potions   { get; set; }

    //TODO - implement these types
    //public List<HealingKit> HealingKits { get; set; }
    //public List<VisionUpgrade> VisionUpgrades { get; set; }
    //public List<MaxHealthUpgrade> HealthUpgrades { get; set; }
    //public Scrapper Scrapper { get; set; }
  }

  public enum FightType
  {
    Normal,
    Elite,
    Boss
  }

  public class Fight : Node
  {
    public Fight(
      Node      baseNode,
      FightType fightType,
      Player    enemy
    ) : base(
      baseNode.NodeType,
      baseNode.IsMystery,
      baseNode.X,
      baseNode.Y,
      baseNode.IsComplete,
      baseNode.IsDestination,
      baseNode.Destinations
    )
    {
      FightType = fightType;
      Enemy     = enemy;
      NodeType  = NodeType.Fight;
    }

    public FightType  FightType { get; set; }
    public Player     Enemy   { get; set; }
  }

  public class Map
  {
    public Map(int index, int width, int height)
    {
      Index   = index;
      Width   = width;
      Height  = height;
      Nodes   = new Node[width, height];
    }

    public int      Index   { get; set; }
    public Node[,]  Nodes   { get; set; }
    public int      Width   { get; set; }
    public int      Height  { get; set; }
  }

  public class Journey
  {
    public List<Map> Maps = new List<Map>();
  }

  public struct MapConfig
  {
    public int    width;
    public int    height;
    public int    pathDensity;

    public double campsiteFrequency;
    public double mysteryFrequency;
    public double eliteFrequency;

    public MapConfig(int width, int height, int pathDensity, double campsiteFrequency, double mysteryFrequency, double eliteFrequency)
    {
      this.width              = width;
      this.height             = height;
      this.pathDensity        = pathDensity;
      this.campsiteFrequency  = campsiteFrequency;
      this.mysteryFrequency   = mysteryFrequency;
      this.eliteFrequency     = eliteFrequency;
    }
  }
  
  public struct EnemyConfig
  {
    public int    baseHealth;
    public int    baseManna;
    public int    baseDeckSize;
    public int    baseTradeRowSize;
    public int    baseHandSize;
    public int    minCardCost;
    public int    maxCardCost;

    public EnemyConfig(int baseHealth, int baseManna, int baseDeckSize, int baseTradeRowSize, int baseHandSize, int minCardCost, int maxCardCost)
    {
      this.baseHealth       = baseHealth;
      this.baseManna        = baseManna;
      this.baseDeckSize     = baseDeckSize;
      this.baseTradeRowSize = baseTradeRowSize;
      this.baseHandSize     = baseHandSize;
      this.minCardCost      = minCardCost;
      this.maxCardCost      = maxCardCost;
    }
  }

}