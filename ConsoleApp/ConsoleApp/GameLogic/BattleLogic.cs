using System;
using System.Collections.Generic;
using MaM.Definitions;
using MaM.Helpers;

namespace MaM.GameLogic
{
  public static class Battle
  {
    public static bool Run(ref Player player, Fight node)
    {
      //TODO

      var random = Algos.GenerateNewRandom();

      var allPlayers = GetPlayerOrder(ref player, ref node.enemies, ref random);

      var currentPlayerIndex = 0;
      while (true)
      {
        var currentPlayer = allPlayers[currentPlayerIndex % allPlayers.Count];

        ExecutePlayerTurn(ref currentPlayer);

        if (currentPlayer.health <= 0)
        {
          if (currentPlayer.isComputer == false)
          {
            return true; //Player has died.
          }

          allPlayers.RemoveAt(currentPlayerIndex);
          if (allPlayers.Count == 1)
          {
            return true; //All enemies have been defeated, and player is still alive. 
          }
        }

        ++currentPlayerIndex;
      }
    }

    private static List<Player> GetPlayerOrder(ref Player player, ref List<Player> enemies, ref Random random)
    {
      //TODO - take initiatives into account.

      var allPlayers = new List<Player>();
      allPlayers.Add(player);
      allPlayers.AddRange(enemies);
      allPlayers.Shuffle(ref random);
      return allPlayers;
    }

    private static void ExecutePlayerTurn(ref Player player)
    {
      if (player.isComputer)
      {
        ExecuteComputerPlayerTurn(ref player);
      }
      else
      {
        ExecuteHumanPlayerTurn(ref player);
      }
    }

    private static void ExecuteComputerPlayerTurn(ref Player player)
    {
      //TODO
    }
    
    private static void ExecuteHumanPlayerTurn(ref Player player)
    {
      //TODO
    }
  }
}
