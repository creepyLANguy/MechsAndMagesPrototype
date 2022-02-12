using System.Collections.Generic;
using MaM.Utilities;
using Newtonsoft.Json;

namespace MaM.Readers
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

    public static Player GetPlayerFromJsonFile(string jsonFile, ref List<Card> cards)
    {
      //TODO = implement
      var json = "";//FileIO.ExcelToJson(excelFile);

      var intermediatePlayer = JsonConvert.DeserializeObject<JsonIntermediatePlayer>(json);

      var player = PopulatePlayerFromJsonIntermediate(intermediatePlayer, ref cards);

      return player;
    }
  }
}
