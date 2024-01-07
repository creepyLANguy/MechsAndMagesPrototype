using System;
using System.Text;
using MaM.Definitions;

namespace MaM.Helpers;

public static class GraphVis
{
  public static void SaveMapsAsDotFiles(ref Journey journey, bool verbose)
  {
    var folderName = "DEBUG_MAPS" + FileSystem.directorySeparator;

    for (var i = 0; i < journey.maps.Count; ++i)
    {
      var dotFileString = GenerateDotFileContents(journey.maps[i], "Map_" + (i + 1), verbose);

      var dotFilename = "Map_" + (i + 1) + "_" + DateTime.Now.Ticks + ".dot";

      FileHelper.WriteFileToDrive(dotFilename, dotFileString, folderName);
    }
  }

  private static string GenerateDotFileContents(Map map, string mapName, bool verbose)
  {
    var mainBuffer = new StringBuilder(); 
      
    mainBuffer.Append("digraph " + mapName + " {" + "\n");

    mainBuffer.Append("\n//Relationships : \n");
    for (var y = 0; y < map.height; ++y)
    {
      for (var x = 0; x < map.width; ++x)
      {
        var node = map.nodes[x, y];

        if (node == null)
        {
          continue;
        }

        var nodeName = GetNodeName(node);

        if (node.destinations == null || node.destinations.Count == 0)
        {
          mainBuffer.Append(nodeName + ";" + "\n");
          continue;
        }

        foreach (var (destX, destY) in node.destinations)
        {
          var destinationName = GetNodeName(map.nodes[destX, destY]);
          mainBuffer.Append(nodeName + "->" + destinationName + ";" + "\n");
        }
      }
    }

    if (verbose == false)
    {
      AddNodeLabelsSection(ref mainBuffer, ref map);
    }

    mainBuffer.Append('}');

    return mainBuffer.ToString();
  }

  private static void AddNodeLabelsSection(ref StringBuilder mainBuffer, ref Map map)
  {
    mainBuffer.Append("\n//Labels : \n");
    for (var y = 0; y < map.height; ++y)
    {
      for (var x = 0; x < map.width; ++x)
      {
        var node = map.nodes[x, y];

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
    var nodeName = "x" + node.x + "y" + node.y + "_";

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
    var str = "Error: UNKNOWN OR BLANK NODE TYPE";

    if (node == null || node.nodeType == NodeType.BLANK)
    {
      return str;
    }

    switch (node.nodeType)
    {
      case NodeType.CAMPSITE:
        str = "Campsite";
        break;
      case NodeType.FIGHT:
      {
        switch (((Fight)node).fightType)
        {
          case FightType.NORMAL:
            str = "NORMAL";
            break;
          case FightType.ELITE:
            str = "ELITE";
            break;
          case FightType.BOSS:
            str = "BOSS";
            break;
        }

        break;
      }
    }

    if (node.isMystery)
    {
      str += "_Mystery";
    }

    return str;
  }
}