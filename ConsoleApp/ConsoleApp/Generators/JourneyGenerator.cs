using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Helpers;
using MaM.Readers;

namespace MaM.Generators;

public static class JourneyGenerator
{
  public static Journey GenerateJourney(
    int journeyLength, 
    List<MapConfig> mapConfigs, 
    EnemyConfig normalEnemyConfig, 
    EnemyConfig eliteEnemyConfig,
    ref EnemyNames enemyNames,
    ref List<Enemy> bosses,
    List<Card> cards, 
    ref Random random
  )
  {
    var journey = new Journey();

    for (var index = 0; index < journeyLength; ++index)
    {
      var map = GenerateMap(index, mapConfigs[index], normalEnemyConfig, eliteEnemyConfig, ref enemyNames, ref bosses, ref cards, ref random);
      journey.maps.Add(map);
    }

    return journey;
  }

  private static Map GenerateMap(
    int index, 
    MapConfig mapConfig, 
    EnemyConfig normalEnemyConfig, 
    EnemyConfig eliteEnemyConfig,
    ref EnemyNames enemyNames,
    ref List<Enemy> bosses,
    ref List<Card> cards, 
    ref Random random
  )
  {
    var map = new Map(index, mapConfig.width, mapConfig.height);

    PopulateMapWithBlankNodes(ref map);

    SetDestinationsForFirstRowNodes(ref map, mapConfig, ref random);

    //Row 1 -> third row from the top
    CompletePathsForEffectiveNodes(ref map, mapConfig, ref random);

    NullifyUnconnectedNodes(ref map);

    SetFirstFloorActiveNodesAsNormalFights(ref map);

    //Note, fights in the bag are all Elites. 
    //Normal fights are filled in later for all unassigned nodes.
    var bag = GenerateNodeTypeDistributionBag(ref map, mapConfig, ref random);

    //Row 1 -> top row
    AssignBagItems(ref map, ref bag);

    AssignNormalFightsToRemainingBlankNodes(ref map);

    AssignMysteryNodes(ref map, mapConfig, ref random);

    AttachFinalCampsiteNode(ref map);

    AttachBossNode(ref map);

    CompleteSetupOfAllNodes(ref map, normalEnemyConfig, eliteEnemyConfig, ref enemyNames, ref bosses, ref cards, ref random);

    return map;
  }

  private static void PopulateMapWithBlankNodes(ref Map map)
  {
    for (var x = 0; x < map.width; ++x)
    {
      for (var y = 0; y < map.height; ++y)
      {
        map.nodes[x, y] = new Node(NodeType.Blank, false, x, y, false, false, null);
      }
    }
  }

  private static void SetDestinationsForFirstRowNodes(ref Map map, MapConfig mapConfig, ref Random random)
  {
    for (var i = 0; i < mapConfig.pathDensity; ++i)
    {
      var x = random.Next(0, mapConfig.width);

      SetDestinationForNode(ref map, x, 0, ref random);
    }
  }

  private static void CompletePathsForEffectiveNodes(ref Map map, MapConfig mapConfig, ref Random random)
  {
    for (var y = 1; y < mapConfig.height - 2; ++y)
    {
      for (var x = 0; x < mapConfig.width; ++x)
      {
        if (map.nodes[x, y].isDestination == false)
        {
          continue;
        }

        SetDestinationForNode(ref map, x, y, ref random);
      }
    }
  }

  private static void SetDestinationForNode(ref Map map, int x, int y, ref Random random)
  {
    if (map.nodes[x, y].destinations != null && map.nodes[x, y].destinations.Count == 3)
    {
      return;
    }

    map.nodes[x, y].destinations ??= new HashSet<Tuple<int, int>>();

    var min = -1;
    var max = 1;

    if (x == 0)
    {
      min = 0;
    }
    else if (x == map.width - 1)
    {
      max = 0;
    }

    var chosenDestinationX = x + random.Next(min, max + 1);
    map.nodes[x, y].destinations.Add(new Tuple<int, int>(chosenDestinationX, y + 1));
    map.nodes[chosenDestinationX, y + 1].isDestination = true;
  }

  private static void NullifyUnconnectedNodes(ref Map map)
  {
    for (var x = 0; x < map.width; ++x)
    {
      for (var y = 0; y < map.height; ++y)
      {
        if (map.nodes[x, y].destinations == null || (map.nodes[x, y].destinations.Count == 0 && map.nodes[x, y].isDestination == false))
        {
          map.nodes[x, y] = null;
        }
      }
    }
  }

  private static void SetFirstFloorActiveNodesAsNormalFights(ref Map map)
  {
    for (var x = 0; x < map.width; ++x)
    {
      if (map.nodes[x, 0] == null)
      {
        continue;
      }

      var baseNode = new Node(map.nodes[x, 0]);
      map.nodes[x, 0] = new Fight(baseNode, FightType.Normal);
    }
  }

  private static List<NodeType> GenerateNodeTypeDistributionBag(ref Map map, MapConfig mapConfig, ref Random random)
  {
    var bag = new List<NodeType>();

    var bagSize = 0;
    for (var x = 0; x < map.width; ++x)
    {
      for (var y = 0; y < map.height; ++y)
      {
        if (map.nodes[x, y] != null && map.nodes[x, y].nodeType == NodeType.Blank)
        {
          ++bagSize;
        }
      }
    }

    var campsiteCount = mapConfig.campsiteFrequency * bagSize;
    for (var i = 0; i < campsiteCount; ++i)
    {
      bag.Add(NodeType.CampSite);
    }
      
    var eliteCount = mapConfig.eliteFrequency * bagSize;
    for (var i = 0; i < eliteCount; ++i)
    {
      bag.Add(NodeType.Fight);
    }

    while (bag.Count < bagSize)
    {
      bag.Add(NodeType.Blank);
    }

    bag.Shuffle(ref random);

    return bag;
  }

  private static void AssignBagItems(ref Map map, ref List<NodeType> bag)
  {
    for (var x = 0; x < map.width; ++x)
    {
      for (var y = 1; y < map.height; ++y)
      {
        if (map.nodes[x, y] == null || map.nodes[x, y].nodeType != NodeType.Blank)
        {
          continue;
        }

        var baseNode = new Node(map.nodes[x, y]);

        switch (bag.First())
        {
          case NodeType.CampSite:
            map.nodes[x, y] = new Campsite(baseNode, null);
            break;
          case NodeType.Fight:
            map.nodes[x, y] = new Fight(baseNode, FightType.Elite);
            break;
        }

        bag.Remove(bag.First());
      }
    }
  }

  private static void AssignNormalFightsToRemainingBlankNodes(ref Map map)
  {
    for (var x = 0; x < map.width; ++x)
    {
      for (var y = 0; y < map.height; ++y)
      {
        if (map.nodes[x, y] == null || map.nodes[x, y].nodeType != NodeType.Blank)
        {
          continue;
        }

        var baseNode = new Node(map.nodes[x, y]);
        map.nodes[x, y] = new Fight(baseNode, FightType.Normal);
      }
    }
  }

  private static void AssignMysteryNodes(ref Map map, MapConfig mapConfig, ref Random random)
  {
    for (var x = 0; x < map.width; ++x)
    {
      for (var y = 1; y < map.height; ++y)
      {
        if (map.nodes[x, y] == null)
        {
          continue;
        }

        var isMystery = random.NextDouble() <= mapConfig.mysteryFrequency;
        map.nodes[x, y].isMystery = isMystery;
      }
    }
  }

  private static void AttachFinalCampsiteNode(ref Map map)
  {
    var baseNodeForFinalCampsite = new Node(NodeType.CampSite, false, 0, map.height - 2, false, true, new HashSet<Tuple<int, int>>());
    var finalCampsite = new Campsite(baseNodeForFinalCampsite, null);

    for (var x = 0; x < map.width; ++x)
    {
      if (map.nodes[x, finalCampsite.y - 1] == null)
      {
        continue;
      }

      map.nodes[x, finalCampsite.y - 1].destinations.Clear();
      map.nodes[x, finalCampsite.y - 1].destinations.Add(new Tuple<int, int>(finalCampsite.x, finalCampsite.y));
    }

    map.nodes[finalCampsite.x, finalCampsite.y] = finalCampsite;
  }

  private static void AttachBossNode(ref Map map)
  {
    var baseNodeForBoss = new Node(NodeType.Fight, false, 0, map.height - 1, false, true, null);

    var bossNode = new Fight(baseNodeForBoss, FightType.Boss);

    map.nodes[0, map.height - 2].destinations.Add(new Tuple<int, int>(bossNode.x, bossNode.y));

    map.nodes[bossNode.x, bossNode.y] = bossNode;
  }

  private static void CompleteSetupOfAllNodes(
    ref Map map, 
    EnemyConfig normalEnemyConfig, 
    EnemyConfig eliteEnemyConfig,
    ref EnemyNames enemyNames,
    ref List<Enemy> bosses,
    ref List<Card> cards,
    ref Random random
  )
  {
    for (var x = 0; x < map.width; ++x)
    {
      for (var y = 0; y < map.height; ++y)
      {
        var node = map.nodes[x, y];

        if (node == null || node.nodeType == NodeType.Blank)
        {
          continue;
        }

        switch (node.nodeType)
        {
          case NodeType.CampSite:
            SetupCampsite(ref node);
            break;
          case NodeType.Fight:
          {
            SetupFight(ref node, ref map, normalEnemyConfig, eliteEnemyConfig, ref enemyNames, ref bosses, ref cards, ref random);
            break;
          }
        }
      }
    }
  }

  private static void SetupCampsite(ref Node node)
  {
    //TODO - implement
    ((Campsite) node).recruits = null;
  }

  private static void SetupFight(
    ref Node node, 
    ref Map map,
    EnemyConfig normalEnemyConfig,
    EnemyConfig eliteEnemyConfig,
    ref EnemyNames enemyNames,
    ref List<Enemy> bosses,
    ref List<Card> cards,
    ref Random random
  )
  {
    switch (((Fight)node).fightType)
    {
      case FightType.Normal:
        SetupEnemy(ref node, normalEnemyConfig, ref enemyNames, map.index, cards, ref random);
        break;
      case FightType.Elite:
        SetupEnemy(ref node, eliteEnemyConfig, ref enemyNames, map.index, cards, ref random);
        break;
      case FightType.Boss:
        SetupBoss(ref node, map.index, ref bosses, ref random);
        break;
    }
  }

  private static void SetupEnemy(ref Node node, EnemyConfig enemyConfig, ref EnemyNames enemyNames, int mapIndex, List<Card> cards, ref Random random)
  {
    cards.Shuffle(ref random);

    var deck = cards
      .Where(card => card.cost >= enemyConfig.minCardCost && card.cost <= enemyConfig.maxCardCost)
      .Take(enemyConfig.baseDeckSize)
      .ToList();

    var dominantGuild = DeckInspector.GetDominantGuild(ref deck);

    var name = EnemyReader.GetEnemyName(dominantGuild, ref enemyNames, ref random);

    var enemy = new Enemy(
      name,
      enemyConfig.baseHealth * (1 + mapIndex),
      enemyConfig.baseTradeRowSize + mapIndex,
      enemyConfig.baseManna + mapIndex, 
      enemyConfig.baseHandSize + mapIndex,
      enemyConfig.initiative + mapIndex,
      deck
    );

    ((Fight) node).enemy = enemy;
  }

  private static void SetupBoss(ref Node node, int mapIndex, ref List<Enemy> bosses, ref Random random)
  {
    bosses.Shuffle(ref random);

    var boss = bosses.First();

    boss.basicHandSize += mapIndex;
    boss.basicManna += mapIndex;
    boss.health *= (1 + mapIndex);
    boss.tradeRowSize += mapIndex;

    ((Fight) node).enemy = boss;

    bosses.Remove(boss);
  }

}