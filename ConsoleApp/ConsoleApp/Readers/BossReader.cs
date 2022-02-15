using System.Collections.Generic;
using System.Linq;
using MaM.Helpers;
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
    private const string JoinedCardDelim = ",";

    private static List<Enemy> PopulateBossesFromJsonIntermediates(List<JsonIntermediateBoss> intermediateBosses, ref List<Card> cards)
    {
      var bosses = new List<Enemy>();

      foreach (var intermediateBoss in intermediateBosses)
      {
        var cardIds = intermediateBoss.Cards.Replace(" ", "").Split(JoinedCardDelim).ToList();

        var bossCards = CardReader.GetCardsFromIds(cardIds, ref cards);

        var boss = new Enemy(
          intermediateBoss.Name,
          intermediateBoss.Health,
          intermediateBoss.TradeRowSize,
          intermediateBoss.Manna,
          intermediateBoss.HandSize,
          bossCards
        );

        bosses.Add(boss);
      }

      return bosses;
    }

    public static List<Enemy> GetBossesFromExcel(string excelFile, ref List<Card> cards)
    {
      var json = FileHelper.ExcelToJson(excelFile);

      var intermediateBosses = JsonConvert.DeserializeObject<List<JsonIntermediateBoss>>(json);

      var bosses = PopulateBossesFromJsonIntermediates(intermediateBosses, ref cards);

      return bosses;
    }
  }
}
