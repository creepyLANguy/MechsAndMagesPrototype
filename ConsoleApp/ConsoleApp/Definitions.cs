using System;
using System.Collections.Generic;

namespace MaM
{
  public class KeyValuePair<TK, TV>
  {
    public TK Key   ;
    public TV Value ;

    protected KeyValuePair(TK key, TV value)
    {
      Key   = key;
      Value = value;
    }
  }

  public class Guild: KeyValuePair<string, int> { public Guild(string key, int val) : base(key, val) { } }
  public struct Guilds
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

  public struct ActionsValuesSet
  {
    public int Attack          ;
    public int Trade           ;  
    public int Draw            ;
    public int Scrap           ;
    public int Consume         ;
    public int OpponentDiscard ;
    public int Heal            ;
  }

  public class CardType : KeyValuePair<string, int> { public CardType(string key, int val) : base(key, val) { } }
  public struct CardTypes
  {
    public static readonly CardType Unknown = new CardType("Unknown", 0);
    public static readonly CardType Unit    = new CardType("Unit",    1);
    public static readonly CardType Base    = new CardType("Base",    2);

    public static readonly List<CardType> All = new List<CardType>() { Unit, Base, Unknown };
  }

  public struct Card
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

    public string            Name              ;
    public CardType          Type              ;
    public Guild             Guild             ;
    public int               Cost              ;
    public int               Defense           ;
    public int               Shield            ;
    public ActionsValuesSet  DefaultAbilities  ;
    public ActionsValuesSet  GuildBonuses      ;
    public ActionsValuesSet  AllyBonuses       ;
    public ActionsValuesSet  ScrapBonuses      ;
    public string            Id                ;
  }

  public struct Potion
  {
    public int Id   ;
    public int Cost ;

    //TODO - figure out how to structure the rest of this. Effects gonna be tricky to implement.
  }

  public class Player
  {
    public Player(
      bool                  isComputer,
      string                name,
      int                   currentNodeX,
      int                   currentNodeY,
      List<Tuple<int, int>> completedNodeLocations,
      int                   completedMapCount,
      Guild                 activeGuild,
      int                   health,
      int                   maxHealth,
      int                   coin,
      int                   vision,
      int                   awareness,
      int                   insight,
      int                   tradeRowSize,
      int                   shield,
      int                   manna,
      int                   basicManna,
      int                   basicHandSize,
      int                   currentHandSize,
      int                   toDiscard,
      List<string>          deckCardIds,
      List<Card>            deck,
      List<Card>            drawPile,
      List<Card>            discardPile,
      List<Card>            tradeRow,
      List<Card>            tradePool,
      List<Potion>          potions
    )
    {
      IsComputer              = isComputer;
      Name                    = name;
      CurrentNodeX            = currentNodeX;
      CurrentNodeY            = currentNodeY;
      CompletedNodeLocations  = completedNodeLocations;
      completedMapCount       = completedMapCount;
      ActiveGuild             = activeGuild;
      Health                  = health;
      MaxHealth               = maxHealth;
      Coin                    = coin;
      Vision                  = vision;
      Awareness               = awareness;
      Insight                 = insight;
      TradeRowSize            = tradeRowSize;
      Shield                  = shield;
      BasicManna              = basicManna;
      Manna                   = manna;
      BasicHandSize           = basicHandSize;
      CurrentHandSize         = currentHandSize;
      ToDiscard               = toDiscard;
      DeckCardIds             = deckCardIds;
      Deck                    = deck;
      DrawPile                = drawPile;
      DiscardPile             = discardPile;
      TradeRow                = tradeRow;
      TradePool               = tradePool;
      Potions                 = potions;
    }

    public bool                   IsComputer              ;
    public string                 Name                    ;
    public int                    CurrentNodeX            ;
    public int                    CurrentNodeY            ;
    public List<Tuple<int, int>>  CompletedNodeLocations  ;
    public int                    CompletedMapCount       ;
    public Guild                  ActiveGuild             ;
    public int                    Health                  ;
    public int                    MaxHealth               ;
    public int                    Coin                    ;
    public int                    Vision                  ; //How many nodes and paths ahead you can simply see
    public int                    Awareness               ; //How many of the visible nodes ahead have their types revealed
    public int                    Insight                 ; //How many of the nodes ahead have their Guild distributions revealed
    public int                    TradeRowSize            ;
    public int                    Shield                  ;
    public int                    BasicManna              ;
    public int                    Manna                   ;
    public int                    ToDiscard               ;
    public int                    BasicHandSize           ;
    public int                    CurrentHandSize         ;
    public List<string>           DeckCardIds             ;
    public List<Card>             Deck                    ;
    public List<Card>             DrawPile                ;
    public List<Card>             DiscardPile             ;
    public List<Card>             TradeRow                ;
    public List<Card>             TradePool               ;
    public List<Potion>           Potions                 ;
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

    public NodeType                 NodeType       ;
    public bool                     IsMystery      ;
    public int                      X              ;
    public int                      Y              ;
    public bool                     IsComplete     ;    
    public bool                     IsDestination  ;
    public HashSet<Tuple<int, int>> Destinations   ;
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

    public List<Potion> Potions;

    //TODO - implement these types
    //public List<HealingKit> HealingKits;
    //public List<VisionUpgrade> VisionUpgrades;
    //public List<MaxHealthUpgrade> HealthUpgrades;
    //public Scrapper Scrapper;
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

    public FightType  FightType ;
    public Player     Enemy     ;
  }

  public struct Map
  {
    public Map(int index, int width, int height)
    {
      Index   = index;
      Width   = width;
      Height  = height;
      Nodes   = new Node[width, height];
    }

    public int      Index   ;
    public Node[,]  Nodes   ;
    public int      Width   ;
    public int      Height  ;
  }

  public class Journey
  {
    public Journey()
    {
      Maps = new List <Map>();
      CurrentNodeX = -1;
      CurrentNodeY = -1;
      CurrentMapIndex = -1;
    }

    public List<Map>  Maps            ;
    public int        CurrentNodeX    ;
    public int        CurrentNodeY    ;
    public int        CurrentMapIndex ;
  }

  public struct MapConfig
  {
    public int    width;
    public int    height;
    public int    pathDensity;

    public double campsiteFrequency;
    public double mysteryFrequency;
    public double eliteFrequency;
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
  }

  public struct GameState
  {
    public DateTime time;
    public int      randomSeed;
    public Player   player;

    public GameState(DateTime time, int randomSeed, Player player)
    {
      this.time       = time;
      this.randomSeed = randomSeed;
      this.player     = player;
    }
  }

  public struct GameConfig
  {
    public string           cardsExcelFile;
    public string           bossesExcelFile;
    public List<MapConfig>  mapConfigs;
    public EnemyConfig      normalEnemyConfig;
    public EnemyConfig      eliteEnemyConfig;
    public int              journeyLength;

    public GameConfig(
      string cardsExcelFile, 
      string bossesExcelFile, 
      List<MapConfig> mapConfigs, 
      EnemyConfig normalEnemyConfig, 
      EnemyConfig eliteEnemyConfig, 
      int journeyLength
      )
    {
      this.cardsExcelFile     = cardsExcelFile;
      this.bossesExcelFile    = bossesExcelFile;
      this.mapConfigs         = mapConfigs;
      this.normalEnemyConfig  = normalEnemyConfig;
      this.eliteEnemyConfig   = eliteEnemyConfig;
      this.journeyLength      = journeyLength;
    }
  }
}