using MaM.Enums;

namespace MaM.Definitions;

public struct Card
{
  public Card(
    string id,
    string name,
    Guild guild,
    int powerCost,
    int mannaCost,
    int power,
    CardAbility ability,
    int abilityCount
  )
  {
    this.id           = id;
    this.name         = name;
    this.guild        = guild;
    this.powerCost    = powerCost;
    this.mannaCost    = mannaCost;
    this.power        = power;
    this.ability      = ability;
    this.abilityCount = abilityCount;
  }

  public string id;
  public string name;
  public Guild guild;
  public int powerCost;
  public int mannaCost;
  public int power;
  public CardAbility ability;
  public int abilityCount;
}