using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace TryJsonToObject
{
  public enum CardType
  {
    Unit,
    Base,
    Unknown
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
    public int? Quantity { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Guild { get; set; }
    public int? Cost { get; set; }
    public int? Defense { get; set; }
    public int? Shield { get; set; }
    public string DefaultAbilities { get; set; }
    public string GuildBonuses { get; set; }
    public string AllyBonuses { get; set; }
    public string ScrapBonuses { get; set; }
    public int? Id { get; set; }
  }

  public class Card
  {
    public Card() { }

    public Card(
      string name, 
      CardType type, 
      Guild guild,
      int cost,
      int defense,
      int shield,
      AbilitySet defaultAbilities,
      AbilitySet guildBonuses,
      AbilitySet allyBonuses,
      AbilitySet scrapBonuses,
      int id
      )
    {
      Name = name;
      Type = type;
      Guild = guild;
      Cost = cost;
      Defense = defense;
      Shield = shield;
      DefaultAbilities = defaultAbilities;
      GuildBonuses = guildBonuses;
      AllyBonuses = allyBonuses;
      ScrapBonuses = scrapBonuses;
      Id = id;
    }

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
    private static CardType DetermineType(string type)
    {
      return type switch
      {
        "Base" => CardType.Base,
        "Unit" => CardType.Unit,
        _ => CardType.Unknown
      };
    }

    private static Guild DetermineGuild(string guild)
    {
      return guild switch
      {
        "Borg" => Guild.Borg,
        "Mech" => Guild.Mech,
        "Mage" => Guild.Mage,
        "Necro" => Guild.Necro,
        _ => Guild.Neutral
      };
    }

    private static AbilitySet DetermineAbilitySet(string abs)
    {
      if (abs == null) return new AbilitySet();

      var a = "A".ToLower();
      var t = "T".ToLower();
      var d = "D".ToLower();
      var s = "S".ToLower();
      var o = "O".ToLower();
      var c = "C".ToLower();
      var h = "H".ToLower();

      var list = abs.Split(',');

      var set = new AbilitySet();

      //AL.
      //TODO
      //foreach string, check what kinda value it is, strip the markup away and convert the value and assign to correct field.

      return set;
    }

    static void Main(string[] args)
    {
      var jsonFile = "Cards.json";
      Console.WriteLine("Reading in " + jsonFile);

      string json = System.IO.File.ReadAllText(jsonFile);
      var intermediateCards = JsonConvert.DeserializeObject<List<JSON_Intermediate_Card>>(json);

      var cards = new List<Card>();

      foreach (var ic in intermediateCards)
      {
        var card = new Card(
          ic.Name,
          DetermineType(ic.Type),
          DetermineGuild(ic.Guild),
          ic.Cost ?? 0,
          ic.Defense ?? 0,
          ic.Shield ?? 0,
          DetermineAbilitySet(ic.DefaultAbilities),
          DetermineAbilitySet(ic.GuildBonuses),
          DetermineAbilitySet(ic.AllyBonuses),
          DetermineAbilitySet(ic.ScrapBonuses),
          ic.Id ?? 0
        );

        for (var i = 0; i < ic.Quantity; ++i)
        {
          cards.Add(card);
        }
      }

    }

  }
}
