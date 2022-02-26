using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Generators;

namespace MaM.GameLogic
{
  public static class Game
  {
    public static void Run(string gameConfigFilename, string saveFilename, string cryptoKey = null)
    {
      var gameContents = GameGenerator.Generate(gameConfigFilename, saveFilename, cryptoKey);

      //TODO - implement

      var player = gameContents.player; //TODO - make sure the default fields for player are set correctly. 

      for (var mapIndex = gameContents.journey.currentMapIndex; mapIndex < gameContents.journey.maps.Count; mapIndex++)
      {
        var map = gameContents.journey.maps[mapIndex];
        
        while (player.completedNodeLocations.Count < map.height)
        {
          var node = GetNextNode(ref player, ref map);
          while(VisitNode(ref player, ref node) == false) //ie while the player has failed to complete the node, repeat visiting the node.
          {
            //TODO - check for  death, etc.
          }
          
          player.completedNodeLocations.Add(new Tuple<int, int>(player.currentNodeX, player.currentNodeY));

          Console.WriteLine(
            "\nCompleted Node " + player.completedNodeLocations.Count + " of " + map.height +
            ", of Map " + (mapIndex+1) + " of " + gameContents.journey.maps.Count
            );
        }

        player.completedNodeLocations.Clear();
        player.currentNodeX = player.currentNodeY = -1;
        ++player.completedMapCount;
        ++gameContents.journey.currentMapIndex;

        Console.WriteLine("\nCompleted Map " + (mapIndex + 1));
      }

      Console.WriteLine("\nAweh, game completed. What a laarnie.");

      //TODO - offer save mechanism regardless of where we are in the game loop. 
      //var gameState = new GameState(DateTime.Now, gameContents.seed, player);
      //SaveFileHelper.PromptUserToSaveGame(ref gameState, cryptoKey);
      //
    }

    private static Node GetNextNode(ref Player player, ref Map map)
    {
      //TODO - test all cases.

      if (player.currentNodeY < 0 && player.currentNodeX < 0)
      {
        return PromptUserForStartingNode(ref player, ref map);
      }
      else
      {
        return PromptUserForNextNode(ref player, ref map);
      }
    }

    //TODO - refactor
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

      Console.WriteLine("\nPlease select your starting location:");
      var n = 0;
      foreach (var node in firstRow)
      {
        Console.WriteLine(++n + ")\t[" + node.x + ", " + node.y + "]\t" + node.nodeType + (node.isMystery ? "_Mystery" : ""));
      }

      var input = int.Parse(Console.ReadLine() ?? "1") - 1;

      var selectedNode = firstRow[input];

      player.currentNodeX = selectedNode.x;
      player.currentNodeY = selectedNode.y;

      return selectedNode;
    }

    //TODO - refactor
    private static Node PromptUserForNextNode(ref Player player, ref Map map)
    {
      var currentNode = map.nodes[player.currentNodeX, player.currentNodeY];
      Console.WriteLine("\nPlease select your next location:");
      var destList = new List<Node>();
      var n = 0;
      foreach (var (x, y) in currentNode.destinations)
      {
        var node = map.nodes[x, y];
        destList.Add(node);
        Console.WriteLine(++n + ")\t[" + node.x + ", " + node.y + "]\t" + node.nodeType + (node.isMystery ? "_Mystery" : ""));
      }

      var input = int.Parse(Console.ReadLine() ?? "1") - 1;
      var (item1, item2) = currentNode.destinations.First(dest => dest.Item1 == destList[input].x && dest.Item2 == destList[input].y);

      player.currentNodeX = item1;
      player.currentNodeY = item2;

      return map.nodes[item1, item2];
    }

    private static bool VisitNode(ref Player player, ref Node node)
    {        
      //TODO - implement properly

      switch (node.nodeType)
      {
        case NodeType.CampSite:
          player.health = player.maxHealth;
          break;
        case NodeType.Fight:
          break;
        case NodeType.Blank:
          break;
        default:
          return true;
      }

      return true;
    }

  }
}
