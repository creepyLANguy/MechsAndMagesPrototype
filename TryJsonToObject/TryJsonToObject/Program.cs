using System;

namespace TryJsonToObject
{
  public enum CardType
  {
    Unit,
    Base,
  }

  public enum Guild
  {
    Borg,
    Mech,
    Mage,
    Necro,
    Neutral
  }

  public class AbilitySet
  {
    public int Attack { get; set; }
    public int Trade { get; set; }
    public int Draw { get; set; }
    public int Scrap { get; set; }
    public int Consume { get; set; }
    public int OpponentDiscard { get; set; }
    public int Heal { get; set; }
  }

  public class JSON_Intermediate_Card
  {
    public string Name { get; set; }
    public CardType Type { get; set; }
    public Guild Guild { get; set; }
    public int Cost { get; set; }
    public int Defense { get; set; }
    public int Shield { get; set; }
    public string DefaultAbilities { get; set; }
    public string GuildBonuses { get; set; }
    public string AllyBonuses { get; set; }
    public string ScrapBonuses { get; set; }
    public int Id { get; set; }
  }

  public class Card 
  {
    public string Name { get; set; }
    public CardType Type { get; set; }
    public Guild Guild { get; set; }
    public int Cost { get; set; }
    public int Defense { get; set; }
    public int Shield { get; set; }
    public AbilitySet DefaultAbilities { get; set; }
    public AbilitySet GuildBonuses { get; set; }
    public AbilitySet AllyBonuses { get; set; }
    public AbilitySet ScrapBonuses { get; set; }
    public int Id { get; set; }
  }

  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
    }
  }
}
