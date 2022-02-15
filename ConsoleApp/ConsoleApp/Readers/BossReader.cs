using System.Collections.Generic;
using System.Linq;
using MaM.Utilities;
using Newtonsoft.Json;

namespace MaM.Readers
{
  internal class JsonIntermediateBoss
  {
    public string   Id            ;
    public string   Name          ;
    public int      Health        ;
    public int      TradeRowSize  ;
    public int      Manna         ;
    public int      HandSize      ;
    public string   Cards         ;
  }


  public static class BossReader
  {
    private static readonly string JoinedCardDelim = ",";

    private static List<Player> PopulateBossesFromJsonIntermediates(List<JsonIntermediateBoss> intermediateBosses, ref List<Card> cards)
    {
      var bosses = new List<Player>();

      foreach (var intermediateBoss in intermediateBosses)
      {
        var cardIds = intermediateBoss.Cards.Replace(" ", "").Split(JoinedCardDelim).ToList();

        var bossCards = CardReader.GetCardsFromIds(cardIds, ref cards);

        var boss = new Player(
          true, 
          intermediateBoss.Name,
          -1,
          -1,
          null,
          -1,
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
          cardIds,
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
