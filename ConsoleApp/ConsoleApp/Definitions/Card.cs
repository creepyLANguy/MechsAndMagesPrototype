using System;
using System.Collections.Generic;

namespace MaM.Definitions;

public struct Card
{
  public Card(
    string id,
    string name,
    Guild guild,
    int powerCost,
    int mannaCost,
    List<Tuple<CardAttribute, int>> defaultActions
  )
  {
    this.id = id;
    this.name = name;
    this.guild = guild;
    this.powerCost = powerCost;
    this.mannaCost= mannaCost;
    this.defaultActions = defaultActions;
  }

  public string name;
  public Guild guild;
  public int powerCost;
  public int mannaCost;
  public List<Tuple<CardAttribute, int>> defaultActions;
  public string id;
}