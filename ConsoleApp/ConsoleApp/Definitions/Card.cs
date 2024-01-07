using System;
using System.Collections.Generic;

namespace MaM.Definitions;

public struct Card
{
  public Card(
    string id,
    string name,
    CardType type,
    Guild guild,
    int cost,
    int defense,
    int shield,
    List<Tuple<CardAttribute, int>> defaultActions,
    List<Tuple<CardAttribute, int>> guildActions,
    List<Tuple<CardAttribute, int>> scrapActions
  )
  {
    this.id = id;
    this.name = name;
    this.type = type;
    this.guild = guild;
    this.cost = cost;
    this.defense = defense;
    this.shield = shield;
    this.defaultActions = defaultActions;
    this.guildActions = guildActions;
    this.scrapActions = scrapActions;
  }

  public string name;
  public CardType type;
  public Guild guild;
  public int cost;
  public int defense;
  public int shield;
  public List<Tuple<CardAttribute, int>> defaultActions;
  public List<Tuple<CardAttribute, int>> guildActions;
  public List<Tuple<CardAttribute, int>> scrapActions;
  public string id;
}