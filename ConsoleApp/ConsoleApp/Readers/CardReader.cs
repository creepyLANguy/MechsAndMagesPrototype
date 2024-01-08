using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MaM.Definitions;
using MaM.Helpers;
using Newtonsoft.Json;

namespace MaM.Readers;

public struct JsonIntermediateCard
{
  public int?   quantity;
  public string name;
  public string type;
  public string guild;
  public int?   cost;
  public int?   defense;
  public int?   shield;
  public string defaultAbilities;
  public string allyBonuses;
  public string scrapBonuses;
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
      var marker = GetAlphabeticPart(split);
      if (Enum.TryParse<CardAttribute>(marker, out var cardAttribute))
      {
        var numericValue = GetNumericPart(split);
        list.Add(new Tuple<CardAttribute, int>(cardAttribute, numericValue));
      }
    }

    return list;
  }

  private static List<Card> PopulateCardsFromJsonIntermediates(List<JsonIntermediateCard> intermediateCards)
  {
    var cards = new List<Card>();

    foreach (var ic in intermediateCards)
    {
      if (Enum.TryParse(ic.type, true, out CardType cardType) == false)
      {
        cardType = CardType.UNKNOWN;
      }

      if (Enum.TryParse(ic.guild, true, out Guild guild) == false)
      {
        guild = Guild.NEUTRAL;
      }

      var card = new Card(
        ic.id,
        ic.name,
        cardType,
        guild,
        ic.cost ?? 0,
        ic.defense ?? 0,
        ic.shield ?? 0,
        GetListOfCardAttributes(ic.defaultAbilities),
        GetListOfCardAttributes(ic.allyBonuses),
        GetListOfCardAttributes(ic.scrapBonuses)
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

  static string GetAlphabeticPart(string input)
  {
    input = input.Trim();
    const string pattern = @"([a-zA-Z]+)";
    var match = Regex.Match(input, pattern);
    return match.Success ? match.Groups[1].Value : string.Empty;
  }

  static int GetNumericPart(string input)
  {
    input = input.Trim();
    const string pattern = @"(\d+)";
    var match = Regex.Match(input, pattern);
    return match.Success ? int.Parse(match.Groups[1].Value) : 0;
  }

}