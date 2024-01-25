using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Enums;
using MaM.Helpers;

namespace MaM.Generators;

//TODO - consider refactoring to use generic distribution bag class for guilds and nodetypes.
public static class JourneyGenerator
{
  public static Journey GenerateJourney(
    int journeyLength,
    List<MapConfig> mapConfigs,
    ref List<Enemy> normalEnemies,
    ref List<Enemy> eliteEnemies,
    ref List<Enemy> bosses,
    int campsiteCardsOnOfferCount
  )
  {
    var journey = new Journey();

    for (var index = 0; index < journeyLength; ++index)
    {
      var mapConfig = index < mapConfigs.Count ? mapConfigs[index] : mapConfigs[^1];

      var map = GenerateMap(
        index,
        mapConfig,
        ref normalEnemies,
        ref eliteEnemies,
        ref bosses,
        campsiteCardsOnOfferCount);

      journey.maps.Add(map);
    }

    return journey;
  }

  private static Map GenerateMap(
    int index,
    MapConfig mapConfig,
    ref List<Enemy> normalEnemies,
    ref List<Enemy> eliteEnemies,
    ref List<Enemy> bosses,
    int campsiteCardsOnOfferCount
  )
  {
    var map = new Map(index, mapConfig.width, mapConfig.height);

    PopulateMapWithBlankNodes(ref map);

    SetDestinationsForFirstRowNodes(ref map, mapConfig);

    //Row 1 -> third row from the top
    CompletePathsForEffectiveNodes(ref map, mapConfig);

    NullifyUnconnectedNodes(ref map);

    var guildDistributionBag = new DistributionBag<Guild>(GetGuildDistributionTemplate(mapConfig));

    SetFirstFloorActiveNodesAsNormalFights(ref map, ref guildDistributionBag);

    //Note, fights in the bag are all Elites. 
    //NORMAL fights are filled in later for all unassigned nodes.
    var bagSize = CalculateBagSize(ref map);
    var nodeTypeDistributionBag = GenerateNodeTypeDistributionBag(mapConfig, bagSize);

    //Row 1 -> top row
    AssignBagItems(ref map, ref nodeTypeDistributionBag, ref guildDistributionBag);

    AssignNormalFightsToRemainingBlankNodes(ref map, ref guildDistributionBag);

    AssignMysteryNodes(ref map, mapConfig);

    AttachFinalCampsiteNode(ref map);

    AttachBossNode(ref map, ref guildDistributionBag);

    CompleteSetupOfAllNodes(
      ref map,
      ref normalEnemies,
      ref eliteEnemies,
      ref bosses,
      campsiteCardsOnOfferCount);

    return map;
  }

  private static Dictionary<Guild, int> GetGuildDistributionTemplate(MapConfig mapConfig)
  {
    //TODO - consider making this read values from config or something.
    var defaultGuildWeight = Math.Max(1, mapConfig.width / 2);
    var guildDistributionTemplate = new Dictionary<Guild, int>
    {
      {Guild.RED, defaultGuildWeight},
      {Guild.BLUE, defaultGuildWeight},
      {Guild.GREEN, defaultGuildWeight},
      {Guild.BLACK, defaultGuildWeight}
    };
    return guildDistributionTemplate;
  }

  private static void PopulateMapWithBlankNodes(ref Map map)
  {
    for (var x = 0; x < map.width; ++x)
    {
      for (var y = 0; y < map.height; ++y)
      {
        map.nodes[x, y] = new Node(NodeType.BLANK, false, x, y, false, false, null);
      }
    }
  }

  private static void SetDestinationsForFirstRowNodes(ref Map map, MapConfig mapConfig)
  {
    var shuffledNodeIndexes = Enumerable.Range(0, mapConfig.width).ToList();
    shuffledNodeIndexes.Shuffle();

    foreach (var x in shuffledNodeIndexes)
    {
      SetDestinationsForNode(ref map, x, 0, mapConfig);
    }
  }

  private static void CompletePathsForEffectiveNodes(ref Map map, MapConfig mapConfig)
  {
    for (var y = 1; y < mapConfig.height - 2; ++y)
    {
      var shuffledNodeIndexes = Enumerable.Range(0, mapConfig.width).ToList();
      shuffledNodeIndexes.Shuffle();

      foreach (var x in shuffledNodeIndexes)
      {
        if (map.nodes[x, y].isDestination == false) continue;
        
        SetDestinationsForNode(ref map, x, y, mapConfig);
      }
    }
  }

  private static void SetDestinationsForNode(ref Map map, int x, int y, MapConfig mapConfig)
  {
    map.nodes[x, y].destinations ??= new HashSet<Tuple<int, int>>();

    for (var i = -1; i <= 1; ++i)
    {
      var candidateDestination = new Tuple<int, int>(x + i, y + 1);

      if (ShouldSkipThisIteration(i, candidateDestination, ref map)) continue;

      map.nodes[x, y].destinations.Add(candidateDestination);
      map.nodes[x + i, y + 1].isDestination = true;
    }

    //TODO - confirm if forcing connections here are probably leading to crossovers anyway. 
    if (map.nodes[x, y].destinations.Count == 0)
    {
      var randomX = UbiRandom.Next(x - 1 < 0 ? 0 : x - 1, x + 1 >= map.width ? map.width : x + 1);
      map.nodes[x, y].destinations.Add(new Tuple<int, int>(randomX, y + 1));
      map.nodes[randomX, y + 1].isDestination = true;
    }


    bool ShouldSkipThisIteration(int i, Tuple<int, int> candidateDestination, ref Map map)
    {
      if (x + i < 0 || x + i >= map.width) return true;

      if (UbiRandom.NextDouble() <= mapConfig.edgeDropProbability) return true;

      //prevents crossovers.
      return i != 0 && 
             map.nodes[x + i, y].destinations != null && 
             map.nodes[x + i, y].destinations.Contains(candidateDestination);
    }
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

  private static void SetFirstFloorActiveNodesAsNormalFights(ref Map map, ref DistributionBag<Guild> guildDistributionBag)
  {
    for (var x = 0; x < map.width; ++x)
    {
      if (map.nodes[x, 0] == null) continue;

      var baseNode = new Node(map.nodes[x, 0]);
      map.nodes[x, 0] = new Fight(baseNode, FightType.NORMAL, guildDistributionBag.Take());
    }
  }

  private static int CalculateBagSize(ref Map map)
  {
    var bagSize = 0;
    for (var x = 0; x < map.width; ++x)
    {
      for (var y = 0; y < map.height; ++y)
      {
        if (map.nodes[x, y] != null && map.nodes[x, y].nodeType == NodeType.BLANK)
        {
          ++bagSize;
        }
      }
    }
    return bagSize;
  }

  private static List<NodeType> GenerateNodeTypeDistributionBag(MapConfig mapConfig, int bagSize)
  {
    var bag = new List<NodeType>();

    var campsiteCount = mapConfig.campsiteFrequency * bagSize;
    for (var i = 0; i < campsiteCount; ++i)
    {
      bag.Add(NodeType.CAMPSITE);
    }

    var eliteCount = mapConfig.eliteFrequency * bagSize;
    for (var i = 0; i < eliteCount; ++i)
    {
      bag.Add(NodeType.FIGHT);
    }

    while (bag.Count < bagSize)
    {
      bag.Add(NodeType.BLANK);
    }

    bag.Shuffle();

    return bag;
  }

  private static void AssignBagItems(ref Map map, ref List<NodeType> nodeTypeDistributionBag, ref DistributionBag<Guild> guildDistributionBag)
  {
    for (var x = 0; x < map.width; ++x)
    {
      for (var y = 1; y < map.height; ++y)
      {
        if (map.nodes[x, y] == null || map.nodes[x, y].nodeType != NodeType.BLANK) continue;

        var baseNode = new Node(map.nodes[x, y]);

        switch (nodeTypeDistributionBag.First())
        {
          case NodeType.CAMPSITE:
            map.nodes[x, y] = new Campsite(baseNode);
            break;
          case NodeType.FIGHT:
            map.nodes[x, y] = new Fight(baseNode, FightType.ELITE, guildDistributionBag.Take());
            break;
        }

        nodeTypeDistributionBag.Remove(nodeTypeDistributionBag.First());
      }
    }
  }

  private static void AssignNormalFightsToRemainingBlankNodes(ref Map map, ref DistributionBag<Guild> guildDistributionBag)
  {
    for (var x = 0; x < map.width; ++x)
    {
      for (var y = 0; y < map.height; ++y)
      {
        if (map.nodes[x, y] == null || map.nodes[x, y].nodeType != NodeType.BLANK) continue;

        var baseNode = new Node(map.nodes[x, y]);
        map.nodes[x, y] = new Fight(baseNode, FightType.NORMAL, guildDistributionBag.Take());
      }
    }
  }

  private static void AssignMysteryNodes(ref Map map, MapConfig mapConfig)
  {
    for (var x = 0; x < map.width; ++x)
    {
      for (var y = 1; y < map.height; ++y)
      {
        if (map.nodes[x, y] == null) continue;

        var isMystery = UbiRandom.NextDouble() <= mapConfig.mysteryFrequency;
        map.nodes[x, y].isMystery = isMystery;
      }
    }
  }

  private static void AttachFinalCampsiteNode(ref Map map)
  {
    var baseNodeForFinalCampsite = new Node(NodeType.CAMPSITE, false, 0, map.height - 2, false, true, new HashSet<Tuple<int, int>>());
    var finalCampsite = new Campsite(baseNodeForFinalCampsite);

    for (var x = 0; x < map.width; ++x)
    {
      if (map.nodes[x, finalCampsite.y - 1] == null) continue;

      map.nodes[x, finalCampsite.y - 1].destinations.Clear();
      map.nodes[x, finalCampsite.y - 1].destinations.Add(new Tuple<int, int>(finalCampsite.x, finalCampsite.y));
    }

    map.nodes[finalCampsite.x, finalCampsite.y] = finalCampsite;
  }

  private static void AttachBossNode(ref Map map, ref DistributionBag<Guild> guildDistributionBag)
  {
    var baseNodeForBoss = new Node(NodeType.FIGHT, false, 0, map.height - 1, false, true, null);

    var bossNode = new Fight(baseNodeForBoss, FightType.BOSS, guildDistributionBag.Take());

    map.nodes[0, map.height - 2].destinations.Add(new Tuple<int, int>(bossNode.x, bossNode.y));

    map.nodes[bossNode.x, bossNode.y] = bossNode;
  }

  private static void CompleteSetupOfAllNodes(
    ref Map map,
    ref List<Enemy> normalEnemies,
    ref List<Enemy> eliteEnemies,
    ref List<Enemy> bosses,
    int campsiteCardsOnOfferCount
  )
  {
    for (var x = 0; x < map.width; ++x)
    {
      for (var y = 0; y < map.height; ++y)
      {
        var node = map.nodes[x, y];

        if (node == null || node.nodeType == NodeType.BLANK) continue;

        switch (node.nodeType)
        {
          case NodeType.CAMPSITE:
            SetupCampsite(ref node, campsiteCardsOnOfferCount);
            break;
          case NodeType.FIGHT:
            SetupFight(ref node, ref map, ref normalEnemies, ref eliteEnemies, ref bosses);
            break;
        }
      }
    }
  }

  private static void SetupCampsite(ref Node node, int campsiteCardsOnOfferCount)
  {
    ((Campsite)node).countCardsOnOffer = campsiteCardsOnOfferCount;
  }

  private static void SetupFight(
    ref Node node,
    ref Map map,
    ref List<Enemy> normalEnemies,
    ref List<Enemy> eliteEnemies,
    ref List<Enemy> bosses
  )
  {
    switch (((Fight)node).fightType)
    {
      case FightType.NORMAL:
        SetupEnemy(ref node, map.index, ref normalEnemies);
        break;
      case FightType.ELITE:
        SetupEnemy(ref node, map.index, ref eliteEnemies);
        break;
      case FightType.BOSS:
        SetupBoss(ref node, map.index, ref bosses);
        break;
    }
  }

  private static void SetupEnemy(ref Node node, int mapIndex, ref List<Enemy> enemies)
  {
    enemies.Shuffle();
    var enemy = enemies.First();
    enemy.health *= (1 + mapIndex);
    ((Fight)node).enemy = enemy;
    enemies.Remove(enemy);
  }

  private static void SetupBoss(ref Node node, int mapIndex, ref List<Enemy> bosses)
  {
    bosses.Shuffle();
    var boss = bosses.First();
    boss.health *= (1 + mapIndex);
    ((Fight)node).enemy = boss;
    bosses.Remove(boss);
  }

}