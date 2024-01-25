using System;
using System.IO;
using System.Text;
using MaM.Definitions;
using MaM.Enums;

namespace MaM.Helpers;

public static class GraphVis
{
  private const string DefaultGraphDirection = "rankdir=\"BT\"";

  private const string DefaultNodeColour = "white";
  private const string DefaultNodeShape = "circle";
  private const string DefaultNodeAttributes = "fixedsize=true width=2.5 height=2.5 penwidth=10";
  private const string DefaultEdgeAttributes = "penwidth=2";

  //private const string NodeColourCamp = "tan1";
  //private const string NodeColourGreen = "darkolivegreen1";
  //private const string NodeColourRed = "pink";
  //private const string NodeColourBlue = "lightblue";
  //private const string NodeColourBlack = "grey";
  private const string NodeColourCamp = "orange";
  private const string NodeColourGreen = "green3";
  private const string NodeColourRed = "red";
  private const string NodeColourBlue = "blue";
  private const string NodeColourBlack = "black";

  private const string NodeShapeCamp = "triangle";
  private const string NodeShapeElite = "pentagon";
  private const string NodeShapeBoss = "star";

  public static void SaveMapsAsDotFiles(ref Journey journey, bool verbose)
  {
    var folderName = "DEBUG_MAPS" + Path.DirectorySeparatorChar;

    for (var i = 0; i < journey.maps.Count; ++i)
    {
      var dotFilename = "Map_" + (i + 1) + "_" + DateTime.Now.Ticks + ".dot";
      var dotFileString = GenerateDotFileContents(journey.maps[i], "Map_" + (i + 1), verbose);
      FileHelper.WriteFileToDrive(dotFilename, dotFileString, folderName);
    }
  }

  private static string GenerateDotFileContents(Map map, string mapName, bool verbose)
  {
    var mainBuffer = new StringBuilder(); 
      
    mainBuffer.Append("digraph " + mapName + " {" + "\n");
    mainBuffer.Append(DefaultGraphDirection + "\n");
    mainBuffer.Append("node[shape=" + DefaultNodeShape + " " + DefaultNodeAttributes + "]\n");
    mainBuffer.Append("edge[" + DefaultEdgeAttributes + "]\n");

    AddRelationshipsSection(ref mainBuffer, map);

    if (verbose == false)
    {
      AddNodeLabelsSection(ref mainBuffer, map);
    }

    mainBuffer.Append('}');

    return mainBuffer.ToString();
  }

  private static void AddRelationshipsSection(ref StringBuilder mainBuffer, Map map)
  {
    mainBuffer.Append("\n//Relationships : \n");

    for (var y = 0; y < map.height; ++y)
    {
      for (var x = 0; x < map.width; ++x)
      {
        var node = map.nodes[x, y];

        if (node == null) continue;

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
  }

  private static void AddNodeLabelsSection(ref StringBuilder mainBuffer, Map map)
  {
    mainBuffer.Append("\n//Labels : \n");

    for (var y = 0; y < map.height; ++y)
    {
      for (var x = 0; x < map.width; ++x)
      {
        var node = map.nodes[x, y];

        if (node == null) continue;

        mainBuffer.Append(GetNodeLabel(node) + "\n");
      }
    }
  }

  private static string GetNodeName(Node node)
  {
    var nodeName = "nodeColourBlack" + node.x + "y" + node.y + "_";
    return nodeName + GetNodeTypeDescriptor(node);
  }

  private static string GetNodeLabel(Node node)
  {
    var nodeName = GetNodeName(node);
    var descriptor = GetNodeTypeDescriptor(node).Replace("_", "\\n");
    var decorations = GetNodeDecorations(node);
    return nodeName + "[label=\"" + descriptor + "\"" + decorations + "];";
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
        str = NodeType.CAMPSITE.ToString();
        break;
      case NodeType.FIGHT:
        str = NodeType.FIGHT + "_" + ((Fight)node).fightType + "_" + ((Fight) node).guild;
        break;
    }

    if (node.isMystery)
    {
      str += "_MYSTERY";
    }

    return str;
  }

  private static string GetNodeDecorations(Node node)
  {
    if (node == null || node.nodeType == NodeType.BLANK)
    {
      return "";
    }

    var fillStyle = node.isMystery ? "none" : "filled";
    var colour = GetColourForNode(node);
    var fontColour = (node.isMystery ? colour : "white");
    var shape = GetShapeForNode(node);
    return " style=" + fillStyle + " color=\"" + colour + "\" fontcolor =\"" + fontColour + "\" shape=\"" + shape + "\"";
  }

  private static string GetColourForNode(Node node)
  {
    var colour = DefaultNodeColour;

    switch (node.nodeType)
    {
      case NodeType.CAMPSITE:
        colour = NodeColourCamp;
        break;
      case NodeType.FIGHT:
      {
        switch (((Fight)node).guild)
        {
          case Guild.GREEN:
            colour = NodeColourGreen;
            break;
          case Guild.RED:
            colour = NodeColourRed;
            break;
          case Guild.BLUE:
            colour = NodeColourBlue;
            break;
          case Guild.BLACK:
            colour = NodeColourBlack;
            break;
          case Guild.NEUTRAL:
          default:
            break;
          }
        break;
      }
    }

    return colour;
  }

  private static string GetShapeForNode(Node node)
  {
    var shape = DefaultNodeShape;
    if (node.GetType() == typeof(Campsite))
    {
      shape = NodeShapeCamp;
    }
    else if (node.GetType() == typeof(Fight))
    {
      if (((Fight)node).fightType == FightType.ELITE)
      {
        shape = NodeShapeElite;
      }
      else if (((Fight) node).fightType == FightType.BOSS)
      {
        shape = NodeShapeBoss;
      }
    }

    return shape;
  }
}