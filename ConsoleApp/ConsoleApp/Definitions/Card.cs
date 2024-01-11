using System;
using System.Collections.Generic;

namespace MaM.Definitions;

public struct Card
{
  public Card(
    string id,
    string name,
    Guild guild,
    int cost,
    List<Tuple<CardAttribute, int>> defaultActions
  )
  {
    this.id = id;
    this.name = name;
    this.guild = guild;
    this.cost = cost;
    this.defaultActions = defaultActions;
  }

  public string name;
  public Guild guild;
  public int cost;
  public List<Tuple<CardAttribute, int>> defaultActions;
  public string id;
}