using System;
using System.Collections.Generic;

namespace TryJsonToObject
{
  public static class JourneyGenerator
  {
    public static Journey GenerateJourney(int journeyLength, int mapWidth, int mapHeight, int pathDensity, Random random)
    {
      var journey = new Journey();

      for (var i = 0; i < journeyLength; ++i)
      {
        var map = GenerateMap(mapWidth, mapHeight, pathDensity, random);
        journey.Maps.Add(map);
      }

      return journey;
    }

    private static Map GenerateMap(int width, int height, int pathDensity, Random random)
    {
      var map = new Map(width, height);

      PopulateMapWithBlankNodes(ref map);

      //Randomly set the destinations for first row nodes.
      for (var i = 0; i < pathDensity; ++i)
      {
        var x = random.Next(0, width);

        SetDestinationForNode(map, x, 0, random);
      }

      //Complete paths from row 1 -> top
      for (var y = 1; y < height - 2; ++y)
      {
        for (var x = 0; x < width; ++x)
        {
          if (map.Nodes[x, y].IsDestination == false)
          {
            continue;
          }

          SetDestinationForNode(map, x, y, random);
        }
      }

      NullifyUnconnectedNodes(ref map);
      
      //TODO - attach BOSS node 
      
      return map;
    }

    private static void PopulateMapWithBlankNodes(ref Map map)
    {
      for (var x = 0; x < map.Width; ++x)
      {
        for (var y = 0; y < map.Height; ++y)
        {
          map.Nodes[x, y] = new Node(NodeType.Blank, false, false, x, y, false, false, null);
        }
      }
    }
    private static void SetDestinationForNode(Map map, int x, int y, Random random)
    {      
      //TODO - investigate preventing crossed edges

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
          if (map.Nodes[x, y].Destinations != null && map.Nodes[x, y].Destinations.Count == 0 && map.Nodes[x, y].IsDestination == false)
          {
            map.Nodes[x, y] = null;
          }
        }
      }
    }
  }
}
