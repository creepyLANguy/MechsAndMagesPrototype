using System;
using System.Collections.Generic;
using System.Linq;

namespace MaM.Definitions;

public class Player
{
  public Player()
  {
  }

  public Player(Player player)
  {
    name                    = player.name;
    currentNodeX            = player.currentNodeX;
    currentNodeY            = player.currentNodeY;
    completedNodeLocations  = player.completedNodeLocations?.ToList();
    completedMapCount       = player.completedMapCount;
    health                  = player.health;
    deckCardIds             = player.deckCardIds?.ToList();
    deck                    = player.deck?.ToList();
  }

  public Player(
    string                name,
    int                   currentNodeX,
    int                   currentNodeY,
    List<Tuple<int, int>> completedNodeLocations,
    int                   completedMapCount,
    int                   health,
    List<string>          deckCardIds,
    List<Card>            deck
  )
  {
    this.name                   = name;
    this.currentNodeX           = currentNodeX;
    this.currentNodeY           = currentNodeY;
    this.completedNodeLocations = completedNodeLocations;
    this.completedMapCount      = completedMapCount;
    this.health                 = health;
    this.deckCardIds            = deckCardIds;
    this.deck                   = deck;
  }

  public string                 name;
  public int                    currentNodeX;
  public int                    currentNodeY;
  public List<Tuple<int, int>>  completedNodeLocations;
  public int                    completedMapCount;
  public int                    health;
  public List<string>           deckCardIds;
  public List<Card>             deck;
}