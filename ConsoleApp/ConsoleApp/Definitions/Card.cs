namespace MaM.Definitions;

public struct Card
{
  public Card(
    string            id,
    string            name,
    CardType          type,
    Guild             guild,
    int               cost,
    int               defense,
    int               shield,
    ActionsValuesSet  defaultActionsValues,
    ActionsValuesSet  guildActionsValues,
    ActionsValuesSet  allyActionsValues,
    ActionsValuesSet  scrapActionsValues
  )
  {
    this.id           = id;
    this.name         = name;
    this.type         = type;
    this.guild        = guild;
    this.cost         = cost;
    this.defense      = defense;
    this.shield       = shield;
    defaultAbilities  = defaultActionsValues;
    guildBonuses      = guildActionsValues;
    allyBonuses       = allyActionsValues;
    scrapBonuses      = scrapActionsValues;
  }

  public string            name;
  public CardType          type;
  public Guild             guild;
  public int               cost;
  public int               defense;
  public int               shield;
  public ActionsValuesSet  defaultAbilities;
  public ActionsValuesSet  guildBonuses;
  public ActionsValuesSet  allyBonuses;
  public ActionsValuesSet  scrapBonuses;
  public string            id;
}