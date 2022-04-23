using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Helpers;

namespace MaM.GameLogic
{
  public static class Battle
  {
    public static bool Run(ref Player humanPlayer, Fight node)
    {
      //TODO

#if DEBUG
      Console.WriteLine("[Start Battle]");
#endif

      var random = Algos.GenerateNewRandom();

      var copyOfHumanPlayer = new Player(humanPlayer);
      var players = new List<Player> { copyOfHumanPlayer, node.enemy };
      players.ShuffleWhileAccountingForInitiatives(ref random);
      PrepareAllPlayersForBattle(ref players);

      var currentPlayerIndex = 0;
      while (true)
      {
        var currentPlayer = players[currentPlayerIndex % players.Count];

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
            //Human player has died.
            //Note, make sure you do NOT persist their death state in this scenario unless it's a hardcore permadeath type game mode.
            return false; 
          }

          players.RemoveAt(currentPlayerIndex);
          if (players.Count == 1)
          {
            //All enemies have been defeated, and human player is still alive.
            humanPlayer = players.First(); //save the human player's victory state.
            return true;  
          }
        }

        ++currentPlayerIndex;
      }
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
