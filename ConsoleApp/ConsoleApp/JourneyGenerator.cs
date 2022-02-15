using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Utilities;

namespace MaM
{
  public static class JourneyGenerator
  {
    public static Journey GenerateJourney(
      int journeyLength, 
      List<MapConfig> mapConfigs, 
      EnemyConfig normalEnemyConfig, 
      EnemyConfig eliteEnemyConfig,
      ref List<Player> bosses,
      List<Card> cards, 
      ref Random random
    )
    {
      var journey = new Journey();

      for (var index = 0; index < journeyLength; ++index)
      {
        var map = GenerateMap(index, mapConfigs[index], normalEnemyConfig, eliteEnemyConfig, ref bosses, ref cards, ref random);
        journey.Maps.Add(map);
      }

      return journey;
    }

    private static Map GenerateMap(
      int index, 
      MapConfig mapConfig, 
      EnemyConfig normalEnemyConfig, 
      EnemyConfig eliteEnemyConfig,
      ref List<Player> bosses,
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

      CompleteSetupOfAllNodes(ref map, normalEnemyConfig, eliteEnemyConfig, ref bosses, ref cards, ref random);

      return map;
    }

    private static void PopulateMapWithBlankNodes(ref Map map)
    {
      for (var x = 0; x < map.Width; ++x)
      {
        for (var y = 0; y < map.Height; ++y)
        {
          map.Nodes[x, y] = new Node(NodeType.Blank, false, x, y, false, false, null);
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
          if (map.Nodes[x, y].IsDestination == false)
          {
            continue;
          }

          SetDestinationForNode(ref map, x, y, ref random);
        }
      }
    }

    private static void SetDestinationForNode(ref Map map, int x, int y, ref Random random)
    {
      if (map.Nodes[x, y].Destinations != null && map.Nodes[x, y].Destinations.Count == 3)
      {
        return;
      }

      map.Nodes[x, y].Destinations ??= new HashSet<Tuple<int, int>>();

      var min = -1;
      var max = 1;

      if (x == 0)
      {
        min = 0;
      }
      else if (x == map.Width - 1)
      {
        max = 0;
      }

      var chosenDestinationX = x + random.Next(min, max + 1);
      map.Nodes[x, y].Destinations.Add(new Tuple<int, int>(chosenDestinationX, y + 1));
      map.Nodes[chosenDestinationX, y + 1].IsDestination = true;
    }

    private static void NullifyUnconnectedNodes(ref Map map)
    {
      for (var x = 0; x < map.Width; ++x)
      {
        for (var y = 0; y < map.Height; ++y)
        {
          if (map.Nodes[x, y].Destinations == null || (map.Nodes[x, y].Destinations.Count == 0 && map.Nodes[x, y].IsDestination == false))
          {
            map.Nodes[x, y] = null;
          }
        }
      }
    }

    private static void SetFirstFloorActiveNodesAsNormalFights(ref Map map)
    {
      for (var x = 0; x < map.Width; ++x)
      {
        if (map.Nodes[x, 0] == null)
        {
          continue;
        }

        var baseNode = new Node(map.Nodes[x, 0]);
        map.Nodes[x, 0] = new Fight(baseNode, FightType.Normal, null);
      }
    }

    private static List<NodeType> GenerateNodeTypeDistributionBag(ref Map map, MapConfig mapConfig, ref Random random)
    {
      var bag = new List<NodeType>();

      var bagSize = 0;
      for (var x = 0; x < map.Width; ++x)
      {
        for (var y = 0; y < map.Height; ++y)
        {
          if (map.Nodes[x, y] != null && map.Nodes[x, y].NodeType == NodeType.Blank)
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
      for (var x = 0; x < map.Width; ++x)
      {
        for (var y = 1; y < map.Height; ++y)
        {
          if (map.Nodes[x, y] == null || map.Nodes[x, y].NodeType != NodeType.Blank)
          {
            continue;
          }

          var baseNode = new Node(map.Nodes[x, y]);

          switch (bag.First())
          {
            case NodeType.CampSite:
              map.Nodes[x, y] = new Campsite(baseNode, null);
              break;
            case NodeType.Fight:
              map.Nodes[x, y] = new Fight(baseNode, FightType.Elite, null);
              break;
          }

          bag.Remove(bag.First());
        }
      }
    }

    private static void AssignNormalFightsToRemainingBlankNodes(ref Map map)
    {
      for (var x = 0; x < map.Width; ++x)
      {
        for (var y = 0; y < map.Height; ++y)
        {
          if (map.Nodes[x, y] == null || map.Nodes[x, y].NodeType != NodeType.Blank)
          {
            continue;
          }

          var baseNode = new Node(map.Nodes[x, y]);
          map.Nodes[x, y] = new Fight(baseNode, FightType.Normal, null);
        }
      }
    }

    private static void AssignMysteryNodes(ref Map map, MapConfig mapConfig, ref Random random)
    {
      for (var x = 0; x < map.Width; ++x)
      {
        for (var y = 1; y < map.Height; ++y)
        {
          if (map.Nodes[x, y] == null)
          {
            continue;
          }

          var isMystery = random.NextDouble() <= mapConfig.mysteryFrequency;
          map.Nodes[x, y].IsMystery = isMystery;
        }
      }
    }

    private static void AttachFinalCampsiteNode(ref Map map)
    {
      var baseNodeForFinalCampsite = new Node(NodeType.CampSite, false, 0, map.Height - 2, false, true, new HashSet<Tuple<int, int>>());
      var finalCampsite = new Campsite(baseNodeForFinalCampsite, null);

      for (var x = 0; x < map.Width; ++x)
      {
        if (map.Nodes[x, finalCampsite.Y - 1] == null)
        {
          continue;
        }

        map.Nodes[x, finalCampsite.Y - 1].Destinations.Clear();
        map.Nodes[x, finalCampsite.Y - 1].Destinations.Add(new Tuple<int, int>(finalCampsite.X, finalCampsite.Y));
      }

      map.Nodes[finalCampsite.X, finalCampsite.Y] = finalCampsite;
    }

    private static void AttachBossNode(ref Map map)
    {
      var baseNodeForBoss = new Node(NodeType.Fight, false, 0, map.Height - 1, false, true, null);

      var bossNode = new Fight(baseNodeForBoss, FightType.Boss, null);

      map.Nodes[0, map.Height - 2].Destinations.Add(new Tuple<int, int>(bossNode.X, bossNode.Y));

      map.Nodes[bossNode.X, bossNode.Y] = bossNode;
    }

    private static void CompleteSetupOfAllNodes(
      ref Map map, 
      EnemyConfig normalEnemyConfig, 
      EnemyConfig eliteEnemyConfig,
      ref List<Player> bosses,
      ref List<Card> cards,
      ref Random random
      )
    {
      for (var x = 0; x < map.Width; ++x)
      {
        for (var y = 0; y < map.Height; ++y)
        {
          var node = map.Nodes[x, y];

          if (node == null || node.NodeType == NodeType.Blank)
          {
            continue;
          }

          switch (node.NodeType)
          {
            case NodeType.CampSite:
              SetupCampsite(ref node);
              break;
            case NodeType.Fight:
            {
              switch (((Fight)node).FightType)
              {
                case FightType.Normal:
                  SetupEnemy(ref node, normalEnemyConfig, map.Index, cards, ref random);
                  break;
                case FightType.Elite:
                  SetupEnemy(ref node, eliteEnemyConfig, map.Index, cards, ref random);
                  break;
                case FightType.Boss:
                  SetupBoss(ref node, map.Index, ref bosses, ref random);
                  break;
              }

              break;
            }
          }
        }
      }
    }

    private static void SetupCampsite(ref Node node)
    {
      //TODO - implement
      ((Campsite) node).Potions = null;
    }
    
    private static void SetupEnemy(ref Node node, EnemyConfig enemyConfig, int mapIndex, List<Card> cards, ref Random random)
    {
      cards.Shuffle(ref random);

      var deck = cards
        .Where(card => card.Cost >= enemyConfig.minCardCost && card.Cost <= enemyConfig.maxCardCost)
        .Take(enemyConfig.baseDeckSize)
        .ToList();

      var enemy = new Player(
        true,
        "",
        -1,
        -1,
        null,
        -1,
        null,
        enemyConfig.baseHealth * (1 + mapIndex),
        enemyConfig.baseHealth * (1 + mapIndex), 
        0, 
        0, 
        0, 
        0,
        enemyConfig.baseTradeRowSize + mapIndex, 
        0, 
        0, 
        enemyConfig.baseManna + mapIndex, 
        enemyConfig.baseHandSize + mapIndex, 
        0, 
        0,
        deck.Select(card => card.Id).ToList(),
        deck, 
        null, 
        null, 
        null, 
        null, 
        null
        );

      ((Fight) node).Enemy = enemy;
    }

    private static void SetupBoss(ref Node node, int mapIndex, ref List<Player> bosses, ref Random random)
    {
      bosses.Shuffle(ref random);

      var boss = bosses.First();

      boss.BasicHandSize += mapIndex;
      boss.BasicManna += mapIndex;
      boss.Health *= (1 + mapIndex);
      boss.TradeRowSize += mapIndex;

      ((Fight) node).Enemy = boss;

      bosses.Remove(boss);
    }

  }
}
