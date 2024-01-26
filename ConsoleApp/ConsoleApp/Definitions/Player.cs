using Newtonsoft.Json;
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
    _deck                   = player._deck?.ToList();
    
    RefreshDeckCardIds();
  }

  public Player(
    string                name,
    int                   currentNodeX,
    int                   currentNodeY,
    List<Tuple<int, int>> completedNodeLocations,
    int                   completedMapCount,
    int                   health,
    List<Card>            deck
  )
  {
    this.name                   = name;
    this.currentNodeX           = currentNodeX;
    this.currentNodeY           = currentNodeY;
    this.completedNodeLocations = completedNodeLocations;
    this.completedMapCount      = completedMapCount;
    this.health                 = health;
    _deck                       = deck;
    
    RefreshDeckCardIds();
  }

  public string                 name;
  public int                    currentNodeX;
  public int                    currentNodeY;
  public List<Tuple<int, int>>  completedNodeLocations;
  public int                    completedMapCount;
  public int                    health;
  private List<Card>            _deck;
  [JsonProperty]
  private List<string> _deckCardIds;

  public List<string> GetDeckCardIds()
    => _deckCardIds;

  public List<Card> GetDeck() 
    => _deck;

  public void SetDeck(List<Card> deck)
  {
    _deck = deck;
    RefreshDeckCardIds();
  }

  public void AddToDeck(Card card)
  {
    _deck.Add(card);
    RefreshDeckCardIds();
  }

  public void RemoveFromDeck(Card card)
  {
    _deck.Remove(card);
    RefreshDeckCardIds();
  }

  private void RefreshDeckCardIds()
  {
    _deckCardIds = _deck?.Select(card => card.id).ToList();
  }
}