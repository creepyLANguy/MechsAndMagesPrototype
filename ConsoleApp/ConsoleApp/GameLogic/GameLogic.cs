using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Generators;
using MaM.Helpers;

namespace MaM.GameLogic
{
  public static class Game
  {
    public static void Run(string gameConfigFilename, string saveFilename, string cryptoKey = null)
    {
      var gameContents = GameGenerator.Generate(gameConfigFilename, saveFilename, cryptoKey);

      var player = gameContents.player; //TODO - make sure the default fields for player are set correctly. 

      SaveGameHelper.Save(gameContents.seed, ref player, saveFilename, cryptoKey);

      for (var mapIndex = gameContents.journey.currentMapIndex; mapIndex < gameContents.journey.maps.Count; mapIndex++)
      {
        var map = gameContents.journey.maps[mapIndex];
        
        while (player.completedNodeLocations.Count < map.height)
        {
          var node = GetNextNode(ref player, ref map);

          SaveGameHelper.Save(gameContents.seed, ref player, saveFilename, cryptoKey);

          while (VisitNode(ref player, ref node) == false) //ie while the player has failed to complete the node, repeat visiting the node.
          {
            //TODO - check for  death, etc.
          }
          
          player.completedNodeLocations.Add(new Tuple<int, int>(player.currentNodeX, player.currentNodeY));

          Console.WriteLine(
            "\nCompleted Node " + player.completedNodeLocations.Count + " of " + map.height +
            ", of Map " + (mapIndex+1) + " of " + gameContents.journey.maps.Count
            );

          SaveGameHelper.Save(gameContents.seed, ref player, saveFilename, cryptoKey);
        }

        ++player.completedMapCount;
        player.completedNodeLocations.Clear();
        player.currentNodeX = player.currentNodeY = -1;

        SaveGameHelper.Save(gameContents.seed, ref player, saveFilename, cryptoKey);

        ++gameContents.journey.currentMapIndex;

        Console.WriteLine("\nCompleted Map " + (mapIndex + 1));
      }

      Console.WriteLine("\nAweh, game completed. What a laarnie.");

      SaveGameHelper.Delete(saveFilename);

      Menus.MainMenu.Load();
    }

    private static Node GetNextNode(ref Player player, ref Map map)
    {
      if (player.currentNodeY < 0 && player.currentNodeX < 0)
      {
        return PromptUserForStartingNode(ref player, ref map);
      }
      else
      {
        return PromptUserForNextNode(ref player, ref map);
      }
    }

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

#if DEBUG
      const int input = 0;
#else 
      var input = UserInput.RequestInt();
#endif

      var selectedNode = firstRow[input];

      player.currentNodeX = selectedNode.x;
      player.currentNodeY = selectedNode.y;

      return selectedNode;
    }

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

#if DEBUG
      const int input = 0;
#else 
      var input = UserInput.RequestInt();
#endif

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
