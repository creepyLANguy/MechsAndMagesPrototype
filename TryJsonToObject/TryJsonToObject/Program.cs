using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TryJsonToObject
{
  public static class CardTypes
  {
    public const string Unit     = "Unit";
    public const string Base     = "Base";
    public const string Unknown  = "Unknown";

    public static readonly List<string> Set = new List<string>() { Unit, Base, Unknown };
  }

  public static class Guilds
  {
    public const string Borg    = "Borg";
    public const string Mech    = "Mech";
    public const string Mage    = "Mage";
    public const string Necro   = "Necro";
    public const string Neutral = "Neutral";

    public static readonly List<string> Set = new List<string>() { Borg, Mech, Mage, Necro, Neutral };
  }
  public static class Abilities
  {
    public const string Attack           = "A";
    public const string Trade            = "T";
    public const string Draw             = "D";
    public const string Scrap            = "S";
    public const string Consume          = "C";
    public const string OpponentDiscard  = "O";
    public const string Heal             = "H";

    public static List<string> Set = new List<string>() { Attack, Trade, Draw, Scrap, Consume, OpponentDiscard, Heal };
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

  public class JsonIntermediateCard
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
      string type, 
      string guild,
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
    public string Type { get; set; }
    public string Guild { get; set; }
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
    public static string Delim = ",";

    private static AbilitySet ConstructAbilitySet(string abs)
    {
      if (abs == null) return new AbilitySet();

      var set = new AbilitySet();

      var absList = abs.Split(Delim);

      foreach (var ab in absList)
      {
        int? val;

        val = GetAbilityValue(ab, Abilities.Attack);
        set.Attack = val ?? set.Attack;

        val = GetAbilityValue(ab, Abilities.Draw);
        set.Draw = val ?? set.Draw;

        val = GetAbilityValue(ab, Abilities.Scrap);
        set.Scrap = val ?? set.Scrap;

        val = GetAbilityValue(ab, Abilities.OpponentDiscard);
        set.OpponentDiscard = val ?? set.OpponentDiscard;

        val = GetAbilityValue(ab, Abilities.Consume);
        set.Consume = val ?? set.Consume;

        val = GetAbilityValue(ab, Abilities.Heal);
        set.Heal = val ?? set.Heal;

        val = GetAbilityValue(ab, Abilities.Trade);
        set.Draw = val ?? set.Draw;
      }

      return set;
    }

    private static int? GetAbilityValue(string str, string ability)
    {
      if (str.Contains(ability) == false) return null;

      //str = str.Replace(ability, "");
      var stripped = str.Substring(ability.Length);
      
      return int.Parse(stripped);
    }

    static void Main(string[] args)
    {
      var jsonFile = "Cards.json";
      Console.WriteLine("Reading in " + jsonFile);

      var json = System.IO.File.ReadAllText(jsonFile);
      var intermediateCards = JsonConvert.DeserializeObject<List<JsonIntermediateCard>>(json);

      var cards = new List<Card>();

      foreach (var ic in intermediateCards)
      {
        var card = new Card(
          ic.Name,
          ic.Type,
          ic.Guild,
          ic.Cost ?? 0,
          ic.Defense ?? 0,
          ic.Shield ?? 0,
          ConstructAbilitySet(ic.DefaultAbilities),
          ConstructAbilitySet(ic.GuildBonuses),
          ConstructAbilitySet(ic.AllyBonuses),
          ConstructAbilitySet(ic.ScrapBonuses),
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
