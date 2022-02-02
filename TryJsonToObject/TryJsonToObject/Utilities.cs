using System;
using System.Collections.Generic;
using System.IO;

namespace TryJsonToObject
{
  static class Utilities
  {
    public static void Shuffle<T>(this IList<T> list, ref Random random)
    {
      var n = list.Count;
      while (n > 1)
      {
        n--;
        var k = random.Next(n + 1);
        var value = list[k];
        list[k] = list[n];
        list[n] = value;
      }
    }

    public static string GenerateDotFileString(Map map, string mapName)
    {
      var mainBuffer = "digraph " + mapName + " {" + "\n";

      for (var y = 0; y < map.Height; ++y)
      {
        for (var x = 0; x < map.Width; ++x)
        {
          var node = map.Nodes[x, y];

          if (node == null)
          {
            continue;
          }

          var nodeName = GetNodeName(node);

          if (node.Destinations == null || node.Destinations.Count == 0)
          {
            mainBuffer += nodeName + ";" + "\n";
            continue;
          }

          foreach (var destination in node.Destinations)
          {
            var destinationName = GetNodeName(map.Nodes[destination.Item1, destination.Item2]);
            mainBuffer += nodeName + "->" + destinationName + ";" + "\n";
          }
        }
      }

      mainBuffer += "}";

      return mainBuffer;
    }

    private static string GetNodeName(Node node)
    {
      if (node == null || node.NodeType == NodeType.Blank)
      {
        return "???????";
      }

      var nodeName = "";

      nodeName = "x" + node.X + "y" + node.Y + "_";

      switch (node.NodeType)
      {
        case NodeType.CampSite:
          nodeName += "C";
          break;
        case NodeType.Fight:
          {
            var fight = (Fight)node;
            switch (fight.FightType)
            {
              case FightType.Normal:
                nodeName += "N";
                break;
              case FightType.Elite:
                nodeName += "E";
                break;
              case FightType.Boss:
                nodeName += "B";
                break;
            }
            break;
          }
      }

      if (node.IsMystery)
      {
        nodeName += "_?";
      }

      return nodeName;
    }

    public static void SaveFile(string filename, string content)
    {
      Console.WriteLine("Saving " + filename);
      File.WriteAllTextAsync(filename, content);
      Console.WriteLine("Saved");
    }
  }
}
