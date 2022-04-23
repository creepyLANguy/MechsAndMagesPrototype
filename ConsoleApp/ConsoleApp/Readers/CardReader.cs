using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Helpers;
using Newtonsoft.Json;
using Action = MaM.Definitions.Action;

namespace MaM.Readers
{
 public struct JsonIntermediateCard
  {
    public int?     quantity          ;
    public string   name              ;
    public string   type              ;
    public string   guild             ;
    public int?     cost              ;
    public int?     defense           ;
    public int?     shield            ;
    public string   defaultAbilities  ;
    public string   guildBonuses      ;
    public string   allyBonuses       ;
    public string   scrapBonuses      ;
    public string   id                ;
  }

  public static class CardReader
  {
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

      var actions = actionString
        .Replace(" ", string.Empty)
        .Split(StringLiterals.Deliminator);

      foreach (var action in actions)
      {
        set.attack = GetCardActionValue(action, Actions.Attack) ?? set.attack;

        set.draw = GetCardActionValue(action, Actions.Draw) ?? set.draw;

        set.scrap = GetCardActionValue(action, Actions.Scrap) ?? set.scrap;

        set.opponentDiscard = GetCardActionValue(action, Actions.OpponentDiscard) ?? set.opponentDiscard;

        set.consume = GetCardActionValue(action, Actions.Consume) ?? set.consume;

        set.heal = GetCardActionValue(action, Actions.Heal) ?? set.heal;

        set.trade = GetCardActionValue(action, Actions.Trade) ?? set.trade;
      }

      return set;
    }

    private static List<Card> PopulateCardsFromJsonIntermediates(List<JsonIntermediateCard> intermediateCards)
    {
      var cards = new List<Card>();

      foreach (var ic in intermediateCards)
      {
        var cardType = CardTypes.All.SingleOrDefault(s => s.Key == ic.type) ?? CardTypes.Unknown; 
        
        var guild = Guilds.All.SingleOrDefault(s => s.Key == ic.guild) ?? Guilds.Neutral;

        var card = new Card(
          ic.id,
          ic.name,
          cardType,
          guild,
          ic.cost ?? 0,
          ic.defense ?? 0,
          ic.shield ?? 0,
          ConstructActionSet(ic.defaultAbilities),
          ConstructActionSet(ic.guildBonuses),
          ConstructActionSet(ic.allyBonuses),
          ConstructActionSet(ic.scrapBonuses)
        );

        for (var i = 0; i < ic.quantity; ++i)
        {
          cards.Add(card);
        }
      }

      return cards;
    }

    public static List<Card> GetCardsFromExcel(string excelFile)
    {
      var json = FileHelper.ExcelToJson(excelFile);

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
        var card = GetCardFromId(cardId, ref cards);
        deck.Add(card);
      }

      return deck;
    }

    public static Card GetCardFromId(string cardId, ref List<Card> cards)
      => cards.Find(it => it.id == cardId);
  }
}
