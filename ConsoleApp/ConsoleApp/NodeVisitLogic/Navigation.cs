using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Enums;
using MaM.Generators;
using MaM.Helpers;

namespace MaM.NodeVisitLogic;

public static class Navigation
{
  public static void Run(string gameConfigFilename, string saveFilename, string cryptoKey = null)
  {
    var gameContents = GameGenerator.Generate(gameConfigFilename, saveFilename, cryptoKey);

    var player = gameContents.player;

    AutoSave();

    for (var mapIndex = gameContents.journey.currentMapIndex; mapIndex < gameContents.journey.maps.Count; mapIndex++)
    {
      var map = gameContents.journey.maps[mapIndex];
        
      while (player.completedNodeLocations.Count < map.height)
      {
        var node = GetNextNode(ref player, ref map);

        AutoSave();

        if (VisitNode(ref node, ref gameContents) == FightResult.PLAYER_LOSE)
        {
          ConsoleMessages.Death(ref gameContents.journey);
          SaveGameHelper.ArchiveRun(saveFilename);
          return;
        }

        AutoSave();

        node.isComplete = true;
        player.completedNodeLocations.Add(new Tuple<int, int>(player.currentNodeX, player.currentNodeY));

        ConsoleMessages.CompletedNode(player.completedNodeLocations.Count, map.height, mapIndex, gameContents.journey.maps.Count);

        AutoSave();
      }

      ++player.completedMapCount;
      player.completedNodeLocations.Clear();
      player.currentNodeX = player.currentNodeY = -1;

      AutoSave();

      ++gameContents.journey.currentMapIndex;

      ConsoleMessages.CompletedMap(mapIndex);
    }

    ConsoleMessages.CompletedRun();

    SaveGameHelper.ArchiveRun(saveFilename);
    return;

    //Nested convenience function definition.
    void AutoSave() 
      => SaveGameHelper.Save(gameContents.seed, ref player, saveFilename, cryptoKey);
  }

  private static Node GetNextNode(ref Player player, ref Map map) 
    => (player.currentNodeY < 0 && player.currentNodeX < 0)
      ? PromptUserForStartingNode(ref player, ref map)
      : PromptUserForNextNode(ref player, ref map);

  private static Node PromptUserForStartingNode(ref Player player, ref Map map)
  {
    var firstRow = new List<Node>();
    for (var i = 0; i < map.width; ++i)
    {
      var node = map.nodes[i, 0];
      if (node != null)
      {
        firstRow.Add(node);
      }
    }

    ConsoleMessages.PromptForStartingNode(ref firstRow);

#if DEBUG
    const int input = 0;
#else 
    var input = UserInput.GetInt() - 1;
#endif

    var selectedNode = firstRow[input];

    player.currentNodeX = selectedNode.x;
    player.currentNodeY = selectedNode.y;

    return selectedNode;
  }

  private static Node PromptUserForNextNode(ref Player player, ref Map map)
  {
    var currentNode = map.nodes[player.currentNodeX, player.currentNodeY];

    ConsoleMessages.PromptForNextNode(ref currentNode, ref map);

#if DEBUG
    const int input = 0;
#else
    var input = UserInput.GetInt() - 1;
#endif

    var destList = new List<Node>();
    foreach (var (x, y) in currentNode.destinations)
    {
      var node = map.nodes[x, y];
      destList.Add(node);
    }

    var (item1, item2) 
      = currentNode.destinations.First(dest => dest.Item1 == destList[input].x && dest.Item2 == destList[input].y);

    player.currentNodeX = item1;
    player.currentNodeY = item2;

    return map.nodes[item1, item2];
  }

  private static FightResult VisitNode(ref Node node, ref GameContents gameContents)
  {
    switch (node.nodeType)
    {
      case NodeType.FIGHT:
        return Battle.Run((Fight)node, ref gameContents);
      case NodeType.CAMPSITE:
        return Rest.Run((Campsite)node, ref gameContents);
      default:
        return FightResult.NONE;
    }
  }
}