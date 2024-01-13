using System;
using System.Collections.Generic;
using MaM.Definitions;
using MaM.Helpers;
using Newtonsoft.Json;

namespace MaM.Readers;

public struct JsonIntermediateCard
{
  public int    quantity;
  public string id;
  public string name;
  public string guild;
  public int    powerCost;
  public int    mannaCost;
  public int    power;
  public string ability;
  public int?    abilityCount;
}

public static class CardReader
{
  private static List<Card> PopulateCardsFromJsonIntermediates(List<JsonIntermediateCard> intermediateCards)
  {
    var cards = new List<Card>();

    foreach (var intermediateCard in intermediateCards)
    {
      if (Enum.TryParse(intermediateCard.guild, true, out Guild guild) == false)
      {
        guild = Guild.NEUTRAL;
      }
      
      if (Enum.TryParse(intermediateCard.ability, true, out CardAbility ability) == false)
      {
        ability = CardAbility.NONE;
      }

      for (var i = 0; i < intermediateCard.quantity; ++i)
      {
        var card = new Card(
          intermediateCard.id,
          intermediateCard.name,
          guild,
          intermediateCard.powerCost, 
          intermediateCard.mannaCost,
          intermediateCard.power,
          ability,
          intermediateCard.abilityCount ?? 0
        );

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