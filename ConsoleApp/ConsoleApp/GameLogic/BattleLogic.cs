using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Helpers;

namespace MaM.GameLogic
{
  public static class Battle
  {
    public static bool Run(ref Player human, Fight node)
    {
      //TODO

#if DEBUG
      Console.WriteLine("[Start Battle]");
#endif

      var humanPlayer = new Player(human);
      var enemies = Algos.DeepCopyPlayerList(ref node.enemies);

      var random = Algos.GenerateNewRandom();

      var tempPlayerList = GetAllPlayers(ref humanPlayer, ref enemies);
      var allPlayers = GetPlayerOrder(ref tempPlayerList, ref random);

      PrepareAllPlayersForBattle(ref allPlayers);

      var currentPlayerIndex = 0;
      while (true)
      {
        var currentPlayer = allPlayers[currentPlayerIndex % allPlayers.Count];

#if DEBUG
        Console.WriteLine("[Turn]\t\t" + currentPlayer.name);
#endif

        ExecuteTurn(ref currentPlayer);

        if (currentPlayer.health <= 0)
        {
#if DEBUG
          Console.WriteLine("[Death]\t\t" + currentPlayer.name);
#endif
          if (currentPlayer.isComputer == false)
          {
            return false; //Player has died.
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

    private static List<Player> GetAllPlayers(ref Player player, ref List<Player> enemies)
    {
      var allPlayers = new List<Player>();
      allPlayers.Add(player);
      allPlayers.AddRange(enemies);
      return allPlayers;
    }

    private static List<Player> GetPlayerOrder(ref List<Player> players, ref Random random)
    {
      var allPlayers = new List<Player>();
      allPlayers.AddRange(players);

      allPlayers.ShuffleWhileAccountingForInitiatives(ref random);

      return allPlayers;
    }

    private static void PrepareAllPlayersForBattle(ref List<Player> allPlayers)
    {
      foreach (var player in allPlayers)
      {
        player.activeGuild      = Guilds.Neutral;
        player.currentHandSize  = 0;
        player.discardPile      = new List<Card>();
        player.drawPile         = player.deck.Where(it => it.guild == Guilds.Neutral).ToList();
        player.manna            = 0;
        player.shield           = 0;
        player.toDiscard        = 0;
        player.tradePool        = player.deck.Where(it => it.guild != Guilds.Neutral).ToList();
        player.tradeRow         = new List<Card>();
      }
    }

    private static void ExecuteTurn(ref Player player)
    {
      if (player.isComputer)
      {
        ExecuteTurnForComputer(ref player);
      }
      else
      {
        ExecuteTurnForHuman(ref player);
      }
    }

    private static void ExecuteTurnForComputer(ref Player player)
    {
      //TODO
      player.health = 0;
    }
    
    private static void ExecuteTurnForHuman(ref Player player)
    {
      //TODO
      player.health = 0;
    }
  }
}
