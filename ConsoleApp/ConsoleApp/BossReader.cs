using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MaM
{
    internal class JsonIntermediateBoss
  {
    public string   Id            { get; set; }
    public string   Name          { get; set; }
    public int      Health        { get; set; }
    public int      TradeRowSize  { get; set; }
    public int      Manna         { get; set; }
    public int      HandSize      { get; set; }
    public string   Cards         { get; set; }
  }


  public static class BossReader
  {
    private static readonly string JoinedCardDelim = ",";

    private static List<Player> PopulateBossesFromJsonIntermediates(List<JsonIntermediateBoss> intermediateBosses, ref List<Card> cards)
    {
      var bosses = new List<Player>();

      foreach (var intermediateBoss in intermediateBosses)
      {
        var cardIds = intermediateBoss.Cards.Replace(" ", "").Split(JoinedCardDelim);

        var bossCards = new List<Card>();

        foreach (var cardId in cardIds)
        {
          var card = cards.Find(it => it.Id == cardId);
          bossCards.Add(card);
        }

        var boss = new Player(
          true, 
          intermediateBoss.Name,
          null,
          intermediateBoss.Health,
          intermediateBoss.Health,
          0,
          0,
          0,
          0,
          intermediateBoss.TradeRowSize, 
          0,
          0,
          intermediateBoss.Manna,
          intermediateBoss.HandSize,
          intermediateBoss.HandSize,
          0,
          bossCards,
          null,
          null,
          null,
          null,
          null
        );

        bosses.Add(boss);
      }

      return bosses;
    }

    public static List<Player> GetBossesFromExcel(string excelFile, ref List<Card> cards)
    {
      var json = FileIO.ExcelToJson(excelFile);

      var intermediateBosses = JsonConvert.DeserializeObject<List<JsonIntermediateBoss>>(json);

      var bosses = PopulateBossesFromJsonIntermediates(intermediateBosses, ref cards);

      return bosses;
    }
  }
}
