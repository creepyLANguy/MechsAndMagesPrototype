using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Helpers;
using Newtonsoft.Json;

namespace MaM.Readers;

public struct JsonIntermediateCard
{
  public int    quantity;
  public string name;
  public string guild;
  public int    powerCost;
  public int    mannaCost;
  public string defaultAbilities;
  public string id;
}

public static class CardReader
{
  private static List<Tuple<CardAttribute, int>> GetListOfCardAttributes(string attributes)
  {
    var list = new List<Tuple<CardAttribute, int>>();
    
    if (attributes == null)
    {
      return list;
    }
      
    var splits = attributes.Split(StringLiterals.ListDelim).ToList();
    foreach (var split in splits)
    {
      var marker = StringSplitters.GetAlphabeticPart(split);
      if (Enum.TryParse<CardAttribute>(marker, out var cardAttribute))
      {
        var numericValue = StringSplitters.GetNumericPart(split);
        list.Add(new Tuple<CardAttribute, int>(cardAttribute, numericValue));
      }
    }

    return list;
  }

  private static List<Card> PopulateCardsFromJsonIntermediates(List<JsonIntermediateCard> intermediateCards)
  {
    var cards = new List<Card>();

    foreach (var intermediateCard in intermediateCards)
    {
      if (Enum.TryParse(intermediateCard.guild, true, out Guild guild) == false)
      {
        guild = Guild.NEUTRAL;
      }

      for (var i = 0; i < intermediateCard.quantity; ++i)
      {
        var card = new Card(
          intermediateCard.id,
          intermediateCard.name,
          guild,
          intermediateCard.powerCost, 
          intermediateCard.mannaCost, 
          GetListOfCardAttributes(intermediateCard.defaultAbilities)
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