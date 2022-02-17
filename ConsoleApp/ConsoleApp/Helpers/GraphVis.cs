﻿using System;
using System.Text;

namespace MaM.Helpers
{
  internal static class GraphVis
  {
    public static void SaveMapsAsDotFiles(ref Journey journey, bool verbose)
    {
      for (var i = 0; i < journey.Maps.Count; ++i)
      {
        var dotFileString = GenerateDotFileContents(journey.Maps[i], "Map_" + (i + 1), verbose);
        var dotFilename = "Map_" + (i + 1) + "_" + DateTime.Now.Ticks + ".dot";
        FileHelper.WriteFileToDrive(dotFilename, dotFileString.ToString());
      }
    }

    private static StringBuilder GenerateDotFileContents(Map map, string mapName, bool verbose)
    {
      var mainBuffer = new StringBuilder(); 
      
      mainBuffer.Append("digraph " + mapName + " {" + "\n");

      mainBuffer.Append("\n//Relationships : \n");
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
            mainBuffer.Append(nodeName + ";" + "\n");
            continue;
          }

          foreach (var (destX, destY) in node.Destinations)
          {
            var destinationName = GetNodeName(map.Nodes[destX, destY]);
            mainBuffer.Append(nodeName + "->" + destinationName + ";" + "\n");
          }
        }
      }

      if (verbose == false)
      {
        AddNodeLabelsSection(ref mainBuffer, ref map);
      }


      mainBuffer.Append('}');

      return mainBuffer;
    }

    private static void AddNodeLabelsSection(ref StringBuilder mainBuffer, ref Map map)
    {
      mainBuffer.Append("\n//Labels : \n");
      for (var y = 0; y < map.Height; ++y)
      {
        for (var x = 0; x < map.Width; ++x)
        {
          var node = map.Nodes[x, y];

          if (node == null)
          {
            continue;
          }

          var nodeLabel = GetNodeLabel(node);

          mainBuffer.Append(nodeLabel + "\n");
        }
      }
    }

    private static string GetNodeName(Node node)
    {
      var nodeName = "x" + node.X + "y" + node.Y + "_";

      nodeName += GetNodeTypeDescriptor(node);

      return nodeName;
    }

    private static string GetNodeLabel(Node node)
    {
      var nodeLabel = GetNodeTypeDescriptor(node);

      nodeLabel = GetNodeName(node) + "[label = \"" + nodeLabel + "\"];";

      return nodeLabel;
    }

    private static string GetNodeTypeDescriptor(Node node)
    {
      var str = "???????";

      if (node == null || node.NodeType == NodeType.Blank)
      {
        return str;
      }

      switch (node.NodeType)
      {
        case NodeType.CampSite:
          str = "Campsite";
          break;
        case NodeType.Fight:
          {
            switch (((Fight)node).FightType)
            {
              case FightType.Normal:
                str = "Normal";
                break;
              case FightType.Elite:
                str = "Elite";
                break;
              case FightType.Boss:
                str = "Boss";
                break;
            }

            break;
          }
      }

      if (node.IsMystery)
      {
        str += "_Mystery";
      }

      return str;
    }
  }
}