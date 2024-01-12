using System;
using System.Collections.Generic;

namespace MaM.Definitions;

public class Enemy
{
  public Enemy()
  {
  }

  public Enemy(
    string id,
    string name,
    int health,
    int marketSize,
    List<Tuple<EnemyTurnAction, int>> turnActions
  )
  {
    this.id = id;
    this.name = name;
    this.health = health;
    this.marketSize = marketSize;
    this.turnActions = turnActions;
  }

  public string id;
  public string name;
  public int health;
  public int marketSize;
  public List<Tuple<EnemyTurnAction, int>> turnActions;
}