using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MaM
{
  internal class JsonIntermediatePlayer
  {
    //TODO - implement
  }


  public static class PlayerReader
  {
    private static readonly string JoinedCardDelim = ",";

    private static Player PopulatePlayerFromJsonIntermediate(JsonIntermediatePlayer intermediatePlayer, ref List<Card> cards)
    {
      //TODO - implement

      //var cardIds = intermediatePlayer.Cards.Replace(" ", "").Split(JoinedCardDelim);

      //var playerCards = new List<Card>();

      //foreach (var cardId in cardIds)
      //{
      //  var card = cards.Find(it => it.Id == cardId);
      //  playerCards.Add(card);
      //}

      var player = new Player();

      return player;
    }

    public static Player GetPlayerFromExcel(string excelFile, ref List<Card> cards)
    {
      var json = FileIO.ExcelToJson(excelFile);

      var intermediatePlayer = JsonConvert.DeserializeObject<JsonIntermediatePlayer>(json);

      var player = PopulatePlayerFromJsonIntermediate(intermediatePlayer, ref cards);

      return player;
    }
  }
}
