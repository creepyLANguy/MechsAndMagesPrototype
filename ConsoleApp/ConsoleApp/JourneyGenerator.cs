using System;
using System.Collections.Generic;

namespace MaM
{
  public static class JourneyGenerator
  {
    public static Journey GenerateJourney(int journeyLength, MapConfig mapConfig, int randomSeed)
    {
      var random = new Random(randomSeed);

      var journey = new Journey();

      for (var i = 0; i < journeyLength; ++i)
      {
        var map = GenerateMap(mapConfig, ref random);
        journey.Maps.Add(map);
      }

      return journey;
    }

    private static Map GenerateMap(MapConfig mapConfig, ref Random random)
    {
      var map = new Map(mapConfig.width, mapConfig.height);

      PopulateMapWithBlankNodes(ref map);

      //Randomly set the destinations for first row nodes.
      SetDestinationsForFirstRowNodes(ref map, mapConfig, ref random);

      //Complete paths from row 1 -> third row from the top
      CompletePathsForEffectiveNodes(ref map, mapConfig, ref random);

      NullifyUnconnectedNodes(ref map);

      //Set first floor's active nodes as normal fights
      SetFirstFloorActiveNodesAsNormalFights(ref map);

      //Note, fights in the bag are actually representative of Elites. 
      //Normal fights are filled in later for all unassigned nodes.
      var bag = GenerateNodeTypeDistributionBag(ref map, mapConfig, ref random);

      //Assign bag items to non-null nodes from row 1 -> top row
      AssignBagItems(ref map, ref bag);

      //Just set the rest of the blank nodes as normal fights.
      AssignNormalFightsToRemainingBlankNodes(ref map);

      //Set some of the nodes as mysteries
      AssignMysteryNodes(ref map, mapConfig, ref random);

      //Attach final campsite
      AttachFinalCampsiteNode(ref map);

      //Attach BOSS node
      AttachBossNode(ref map);

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
        if (map.Nodes[x, 0] != null)
        {
          var baseNode = new Node(map.Nodes[x, 0]);
          map.Nodes[x, 0] = new Fight(baseNode, FightType.Normal, null, null, 0, 0, null, null);
        }
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

          switch (bag[0])
          {
            case NodeType.CampSite:
              map.Nodes[x, y] = new Campsite(baseNode, null);
              break;
            case NodeType.Fight:
              map.Nodes[x, y] = new Fight(baseNode, FightType.Elite, null, null, 0, 0, null, null);
              break;
          }

          bag.RemoveAt(0);
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
          map.Nodes[x, y] = new Fight(baseNode, FightType.Normal, null, null, 0, 0, null, null);
        }
      }
    }

    private static void AssignMysteryNodes(ref Map map, MapConfig mapConfig, ref Random random)
    {
      for (var x = 0; x < map.Width; ++x)
      {
        for (var y = 1; y < map.Height; ++y)
        {
          if (map.Nodes[x, y] == null) //|| map.Nodes[x, y].NodeType != NodeType.Blank)
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
      var bossNode = new Fight(baseNodeForBoss, FightType.Boss, null, null, 0, 0, null, null);
      map.Nodes[0, map.Height - 2].Destinations.Add(new Tuple<int, int>(bossNode.X, bossNode.Y));
      map.Nodes[bossNode.X, bossNode.Y] = bossNode;
    }

  }
}
