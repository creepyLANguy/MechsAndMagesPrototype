using System.Collections.Generic;

namespace TryJsonToObject
{
  public static class Guilds
  {
    public const string Borg = "Borg";
    public const string Mech = "Mech";
    public const string Mage = "Mage";
    public const string Necro = "Necro";
    public const string Neutral = "Neutral";

    public static readonly List<string> All = new List<string>() { Borg, Mech, Mage, Necro, Neutral };
  }

  public static class Actions
  {
    public const string Attack = "A";
    public const string Trade = "T";
    public const string Draw = "D";
    public const string Scrap = "S";
    public const string Consume = "C";
    public const string OpponentDiscard = "O";
    public const string Heal = "H";

    public static List<string> All = new List<string>() { Attack, Trade, Draw, Scrap, Consume, OpponentDiscard, Heal };
  }

  public class ActionSet
  {
    public int Attack { get; set; }
    public int Trade { get; set; }
    public int Draw { get; set; }
    public int Scrap { get; set; }
    public int Consume { get; set; }
    public int OpponentDiscard { get; set; }
    public int Heal { get; set; }
  }
  public static class CardTypes
  {
    public const string Unit = "Unit";
    public const string Base = "Base";
    public const string Unknown = "Unknown";

    public static readonly List<string> All = new List<string>() { Unit, Base, Unknown };
  }

  public class Card
  {
    public Card() {}

    public Card(
      int id,
      string name,
      string type,
      string guild,
      int cost,
      int defense,
      int shield,
      ActionSet defaultActions,
      ActionSet guildActions,
      ActionSet allyActions,
      ActionSet scrapActions
    )
    {
      Id = id;
      Name = name;
      Type = type;
      Guild = guild;
      Cost = cost;
      Defense = defense;
      Shield = shield;
      DefaultAbilities = defaultActions;
      GuildBonuses = guildActions;
      AllyBonuses = allyActions;
      ScrapBonuses = scrapActions;
    }

    public string Name { get; set; }
    public string Type { get; set; }
    public string Guild { get; set; }
    public int Cost { get; set; }
    public int Defense { get; set; }
    public int Shield { get; set; }
    public ActionSet DefaultAbilities { get; set; }
    public ActionSet GuildBonuses { get; set; }
    public ActionSet AllyBonuses { get; set; }
    public ActionSet ScrapBonuses { get; set; }
    public int Id { get; set; }
  }

}
