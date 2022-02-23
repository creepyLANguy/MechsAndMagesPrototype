using System;
using System.Collections.Generic;
using System.Linq;

namespace MaM.Definitions
{
  public class KeyValuePair<TK, TV>
  {
    public TK Key   ;
    public TV Value ;

    protected KeyValuePair(TK key, TV value)
    {
      Key   = key   ;
      Value = value ;
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

    // Important - assign in the same order as the value for each guild.
    public static readonly List<Guild> All = new List<Guild>() { Neutral, Borg, Mech, Mage, Necro }; 
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

    public static readonly List<Action> All = new List<Action>() { Attack, Trade, Draw, Scrap, Consume, OpponentDiscard, Heal };
  }

  public struct ActionsValuesSet
  {
    public int attack          ;
    public int trade           ;  
    public int draw            ;
    public int scrap           ;
    public int consume         ;
    public int opponentDiscard ;
    public int heal            ;
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
      this.id           = id;
      this.name         = name;
      this.type         = type;
      this.guild        = guild;
      this.cost         = cost;
      this.defense      = defense;
      this.shield       = shield;
      defaultAbilities  = defaultActionsValues;
      guildBonuses      = guildActionsValues;
      allyBonuses       = allyActionsValues;
      scrapBonuses      = scrapActionsValues;
    }

    public string            name              ;
    public CardType          type              ;
    public Guild             guild             ;
    public int               cost              ;
    public int               defense           ;
    public int               shield            ;
    public ActionsValuesSet  defaultAbilities  ;
    public ActionsValuesSet  guildBonuses      ;
    public ActionsValuesSet  allyBonuses       ;
    public ActionsValuesSet  scrapBonuses      ;
    public string            id                ;
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
      List<Card>            tradePool
    )
    {
      this.isComputer              = isComputer              ;
      this.name                    = name                    ;
      this.currentNodeX            = currentNodeX            ;
      this.currentNodeY            = currentNodeY            ;
      this.completedNodeLocations  = completedNodeLocations  ;
      this.completedMapCount       = completedMapCount       ;
      this.activeGuild             = activeGuild             ;
      this.health                  = health                  ;
      this.maxHealth               = maxHealth               ;
      this.coin                    = coin                    ;
      this.vision                  = vision                  ;
      this.awareness               = awareness               ;
      this.insight                 = insight                 ;
      this.tradeRowSize            = tradeRowSize            ;
      this.shield                  = shield                  ;
      this.basicManna              = basicManna              ;
      this.manna                   = manna                   ;
      this.basicHandSize           = basicHandSize           ;
      this.currentHandSize         = currentHandSize         ;
      this.toDiscard               = toDiscard               ;
      this.deckCardIds             = deckCardIds             ;
      this.deck                    = deck                    ;
      this.drawPile                = drawPile                ;
      this.discardPile             = discardPile             ;
      this.tradeRow                = tradeRow                ;
      this.tradePool               = tradePool               ;
    }

    public bool                   isComputer              ;
    public string                 name                    ;
    public int                    currentNodeX            ;
    public int                    currentNodeY            ;
    public List<Tuple<int, int>>  completedNodeLocations  ;
    public int                    completedMapCount       ;
    public Guild                  activeGuild             ;
    public int                    health                  ;
    public int                    maxHealth               ;
    public int                    coin                    ;
    public int                    vision                  ; //How many nodes and paths ahead you can simply see
    public int                    awareness               ; //How many of the visible nodes ahead have their types revealed
    public int                    insight                 ; //How many of the nodes ahead have their Guild distributions revealed
    public int                    tradeRowSize            ;
    public int                    shield                  ;
    public int                    basicManna              ;
    public int                    manna                   ;
    public int                    toDiscard               ;
    public int                    basicHandSize           ;
    public int                    currentHandSize         ;
    public List<string>           deckCardIds             ;
    public List<Card>             deck                    ;
    public List<Card>             drawPile                ;
    public List<Card>             discardPile             ;
    public List<Card>             tradeRow                ;
    public List<Card>             tradePool               ;
  }

  public class Enemy : Player
  {
    public Enemy(
      string name,
      int maxHealth, 
      int tradeRowSize,
      int basicManna, 
      int basicHandSize,
      List<Card> deck
      ) 
      : base(
        true, 
        name, 
        -1,
        -1, 
        null,
        -1,
        null, 
        maxHealth, 
        maxHealth, 
        -1, 
        -1, 
        -1, 
        -1, 
        tradeRowSize, -1,
        -1, 
        basicManna, 
        basicHandSize, 
        -1, 
        -1,
        deck.Select(card => card.id).ToList(), 
        deck, 
        null, 
        null, 
        null, 
        null
        )
    {
    }
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
      nodeType      = node.nodeType;
      isMystery     = node.isMystery;
      x             = node.x;            
      y             = node.y;            
      isComplete    = node.isComplete;
      isDestination = node.isDestination;
      destinations  = node.destinations;
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
      this.nodeType      = nodeType;
      this.isMystery     = isMystery;
      this.x             = x;
      this.y             = y;
      this.isComplete    = isComplete;
      this.isDestination = isDestination;
      this.destinations  = destinations;
    }

    public NodeType                 nodeType       ;
    public bool                     isMystery      ;
    public int                      x              ;
    public int                      y              ;
    public bool                     isComplete     ;    
    public bool                     isDestination  ;
    public HashSet<Tuple<int, int>> destinations   ;
  }
  
  public class Campsite : Node
  {
    public Campsite(
      Node        baseNode,
      List<Card>  recruits
    ) : base(
      baseNode.nodeType,
      baseNode.isMystery,
      baseNode.x,
      baseNode.y,
      baseNode.isComplete,
      baseNode.isDestination,
      baseNode.destinations
    )
    {
      this.recruits   = recruits;
      nodeType        = NodeType.CampSite;
    }

    public List<Card> recruits;

    //TODO - implement these types
    //public List<Potion> Potions;
    //public List<Relic> Relics;
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
      baseNode.nodeType,
      baseNode.isMystery,
      baseNode.x,
      baseNode.y,
      baseNode.isComplete,
      baseNode.isDestination,
      baseNode.destinations
    )
    {
      this.fightType  = fightType;
      this.enemy      = enemy;
      nodeType        = NodeType.Fight;
    }

    public FightType  fightType ;
    public Player     enemy     ;
  }

  public struct Map
  {
    public Map(int index, int width, int height)
    {
      this.index   = index;
      this.width   = width;
      this.height  = height;
      nodes   = new Node[width, height];
    }

    public int      index   ;
    public Node[,]  nodes   ;
    public int      width   ;
    public int      height  ;
  }

  public class Journey
  {
    public Journey()
    {
      maps            = new List <Map>();
      currentNodeX    = -1;
      currentNodeY    = -1;
      currentMapIndex = -1;
    }

    public List<Map>  maps            ;
    public int        currentNodeX    ;
    public int        currentNodeY    ;
    public int        currentMapIndex ;
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
    public int  baseHealth;
    public int  baseManna;
    public int  baseDeckSize;
    public int  baseTradeRowSize;
    public int  baseHandSize;
    public int  minCardCost;
    public int  maxCardCost;
  }

  public class EnemyNames
  {
    public EnemyNames(List<List<string>> blob)
    {
      var i = -1;

      pre = blob[++i];

      neutralDescriptors = blob[++i];
      borgDescriptors    = blob[++i];
      mechDescriptors    = blob[++i];
      mageDescriptors    = blob[++i];
      necroDescriptors   = blob[++i];

      collective         = blob[++i];

      post               = blob[++i];

      place              = blob[++i];

      allLists = new List<List<string>>
      {
        pre, 
        neutralDescriptors, 
        borgDescriptors, 
        mechDescriptors, 
        mageDescriptors, 
        necroDescriptors, 
        collective, 
        post,
        place
      };
    }

    public List<string> pre;

    public List<string> neutralDescriptors;
    public List<string> borgDescriptors;
    public List<string> mechDescriptors;
    public List<string> mageDescriptors;
    public List<string> necroDescriptors;

    public List<string> collective;

    public List<string> post;

    public List<string> place;

    public List<List<string>> allLists;
  }

  public struct PlayerConfig
  {
    public int  health        ;
    public int  vision        ;
    public int  awareness     ;
    public int  insight       ;
    public int  tradeRowSize  ;
    public int  manna         ;
    public int  handSize      ;
  }

  public struct InitialCardSelection
  {
    public int minCost;
    public int maxCost;
    public int cardCount;
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
    public string                     cardsExcelFile;
    public string                     bossesExcelFile;
    public string                     enemyNamesExcelFile;
    public List<MapConfig>            mapConfigs;
    public EnemyConfig                normalEnemyConfig;
    public EnemyConfig                eliteEnemyConfig;
    public PlayerConfig               playerConfig;
    public List<InitialCardSelection> initialCardSelections;
    public int                        journeyLength;
  }

  public struct GameContents
  {
    public Player       player  ;
    public Journey      journey ;
    public List<Card>   cards   ;
    public Random       random  ;
    public int          seed    ;

    public GameContents(Player player, Journey journey, List<Card> cards, Random random, int seed)
    {
      this.player   = player;
      this.journey  = journey;
      this.cards    = cards;
      this.random   = random;
      this.seed     = seed;
    }
  }
}