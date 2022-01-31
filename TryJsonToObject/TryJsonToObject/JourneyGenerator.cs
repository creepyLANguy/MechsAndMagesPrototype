using System;

namespace TryJsonToObject
{
  public static class JourneyGenerator
  {
    public static void GenerateJourney(int journeyLength, int mapWidth, int mapHeight, Random random, ref Journey journey)
    {
      for (var i = 0; i < journeyLength; ++i)
      {
        var map = new Map();
        GenerateMap(mapWidth, mapHeight, random, ref map);
        journey.Maps.Add(map);
      }
    }

    private static void GenerateMap(int mapWidth, int mapHeight, Random random, ref Map map)
    {
      map.Nodes = new Node[mapWidth, mapHeight];
      
      //Populate whole map with blank nodes. 
      for (var x = 0; x < mapWidth; ++x)
      {
        for (var y = 0; y < mapHeight; ++y)
        {
          map.Nodes[x,y] = new Node(NodeType.Blank, false, false, x, y, false);
        }
      }

      //Select random node index at bottom level 
      random.Next(0, mapWidth);

      //Select one of three nodes from from the row above, make sure to protect against out of bounds and crossed edges


      //AL.
    }
  }
}
