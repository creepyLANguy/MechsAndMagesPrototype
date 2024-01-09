using System;
using System.Collections.Generic;

namespace MaM.Definitions;

public struct GameContents
{
  public Player       player;
  public Journey      journey;
  public List<Card>   cards;
  public int handSize;
  public Random       random;
  public int          seed;

  public GameContents(Player player, Journey journey, List<Card> cards, int handSize, Random random, int seed)
  {
    this.player   = player;
    this.journey  = journey;
    this.cards    = cards;
    this.handSize = handSize;
    this.random   = random;
    this.seed     = seed;
  }
}