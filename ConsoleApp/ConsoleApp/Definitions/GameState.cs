using System;
using System.Collections.Generic;

namespace MaM.Definitions;

public struct GameState
{
  public DateTime time;
  public Player   player;
  public int randomSeed;
  public UbiRandomCallHistory randomCallHistory;

  public GameState(DateTime time, Player player, int randomSeed, UbiRandomCallHistory randomCallHistory)
  {
    this.time = time;
    this.player = player;
    this.randomSeed = randomSeed;
    this.randomCallHistory = randomCallHistory;
  }
}