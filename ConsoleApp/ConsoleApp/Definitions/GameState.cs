using System;

namespace MaM.Definitions;

public struct GameState
{
  public DateTime time;
  public int      randomSeed;
  public Player   player;

  public GameState(DateTime time, int randomSeed, Player player)
  {
    this.time       = time;
    this.randomSeed = randomSeed;
    this.player     = player;
  }
}