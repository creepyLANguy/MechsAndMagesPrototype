using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Helpers;

namespace MaM.GameLogic;

public static class Battle
{
  public static bool Run(ref Player humanPlayer, Fight node)
  {
    Console.WriteLine("\n[Start Battle]");

    var random = Algos.GenerateNewRandom();

    var players = new List<Player> { humanPlayer, node.enemy };
    players.ShuffleWhileAccountingForInitiatives(ref random);

    PrepareAllPlayersForBattle(ref players);

    //AL.
    //SetStartingHands(ref players);

    //AL.
    //SetStartingTraderows(ref players);

    var currentPlayerIndex = 0;
    while (true)
    {
      var currentPlayer = players[currentPlayerIndex % players.Count];

      Console.WriteLine("[Turn]\t\t" + currentPlayer.name);

      ExecuteTurn(ref currentPlayer);

      if (currentPlayer.health <= 0)
      {
        Console.WriteLine("[Death]\t\t" + currentPlayer.name);

        return currentPlayer.isComputer;
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
      player.toDiscard        = 0;
      player.tradePool        = player.deck.Where(it => it.guild != Guilds.Neutral).ToList();
      player.tradeRow         = new List<Card>();
      player.cardsInPlay      = new List<Card>();
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
    player.health = new Random((int)(DateTime.Now.Ticks)).Next(0, 5);
  }
    
  private static void ExecuteTurnForHuman(ref Player player)
  {
    //TODO
    player.health -= new Random((int)(DateTime.Now.Ticks)).Next(0, 5) == 0 ? 1 : 0;
  }
}