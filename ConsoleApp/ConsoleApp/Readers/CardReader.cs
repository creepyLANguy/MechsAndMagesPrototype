using System.Collections.Generic;
using System.Linq;
using MaM.Utilities;
using Newtonsoft.Json;

namespace MaM.Readers
{
  internal class JsonIntermediateCard
  {
    public int?     Quantity          { get; set; }
    public string   Name              { get; set; }
    public string   Type              { get; set; }
    public string   Guild             { get; set; }
    public int?     Cost              { get; set; }
    public int?     Defense           { get; set; }
    public int?     Shield            { get; set; }
    public string   DefaultAbilities  { get; set; }
    public string   GuildBonuses      { get; set; }
    public string   AllyBonuses       { get; set; }
    public string   ScrapBonuses      { get; set; }
    public string   Id                { get; set; }
  }

  public static class CardReader
  {
    private static readonly string JoinedActionDelim = ",";

    private static int? GetCardActionValue(string str, Action action)
    {
      if (str.Contains(action.Key) == false) return null;

      //str = str.Replace(action, "");
      var stripped = str.Substring(action.Key.Length);

      return int.Parse(stripped);
    }

    private static ActionsValuesSet ConstructActionSet(string actionString)
    {
      if (actionString == null) return new ActionsValuesSet();

      var set = new ActionsValuesSet();

      var actions = actionString.Replace(" ", "").Split(JoinedActionDelim);

      foreach (var action in actions)
      {
        set.Attack = GetCardActionValue(action, Actions.Attack) ?? set.Attack;

        set.Draw = GetCardActionValue(action, Actions.Draw) ?? set.Draw;

        set.Scrap = GetCardActionValue(action, Actions.Scrap) ?? set.Scrap;

        set.OpponentDiscard = GetCardActionValue(action, Actions.OpponentDiscard) ?? set.OpponentDiscard;

        set.Consume = GetCardActionValue(action, Actions.Consume) ?? set.Consume;

        set.Heal = GetCardActionValue(action, Actions.Heal) ?? set.Heal;

        set.Trade = GetCardActionValue(action, Actions.Trade) ?? set.Trade;
      }

      return set;
    }

    private static List<Card> PopulateCardsFromJsonIntermediates(List<JsonIntermediateCard> intermediateCards)
    {
      var cards = new List<Card>();

      foreach (var ic in intermediateCards)
      {
        var cardType = CardTypes.All.SingleOrDefault(s => s.Key == ic.Type) ?? CardTypes.Unknown; 
        
        var guild = Guilds.All.SingleOrDefault(s => s.Key == ic.Guild) ?? Guilds.Neutral;

        var card = new Card(
          ic.Id,
          ic.Name,
          cardType,
          guild,
          ic.Cost ?? 0,
          ic.Defense ?? 0,
          ic.Shield ?? 0,
          ConstructActionSet(ic.DefaultAbilities),
          ConstructActionSet(ic.GuildBonuses),
          ConstructActionSet(ic.AllyBonuses),
          ConstructActionSet(ic.ScrapBonuses)
        );

        for (var i = 0; i < ic.Quantity; ++i)
        {
          cards.Add(card);
        }
      }

      return cards;
    }

    public static List<Card> GetCardsFromExcel(string excelFile)
    {
      var json = FileIO.ExcelToJson(excelFile);

      var intermediateCards = JsonConvert.DeserializeObject<List<JsonIntermediateCard>>(json);

      var cards = PopulateCardsFromJsonIntermediates(intermediateCards);

      return cards;
    }

    public static List<Card> GetCardsFromIds(List<string> cardIds, ref List<Card> cards)
    {
      if (cardIds == null || cards == null)
      {
        return null;
      }

      var deck = new List<Card>();

      foreach (var cardId in cardIds)
      {
        var card = cards.Find(it => it.Id == cardId);
        deck.Add(card);
      }

      return deck;
    }
  }
}
