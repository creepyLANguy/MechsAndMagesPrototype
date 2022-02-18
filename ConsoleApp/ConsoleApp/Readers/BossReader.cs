using System.Collections.Generic;
using System.Linq;
using MaM.Helpers;
using Newtonsoft.Json;

namespace MaM.Readers
{
  internal struct JsonIntermediateBoss
  {
    public string   id            ;
    public string   name          ;
    public int      health        ;
    public int      tradeRowSize  ;
    public int      manna         ;
    public int      handSize      ;
    public string   cards         ;
  }

  public static class BossReader
  {
    private const string JoinedCardDelim = ",";

    private static List<Enemy> PopulateBossesFromJsonIntermediates(List<JsonIntermediateBoss> intermediateBosses, ref List<Card> cards)
    {
      var bosses = new List<Enemy>();

      foreach (var intermediateBoss in intermediateBosses)
      {
        var cardIds = intermediateBoss.cards.Replace(" ", string.Empty).Split(JoinedCardDelim).ToList();

        var bossCards = CardReader.GetCardsFromIds(cardIds, ref cards);

        var boss = new Enemy(
          intermediateBoss.name,
          intermediateBoss.health,
          intermediateBoss.tradeRowSize,
          intermediateBoss.manna,
          intermediateBoss.handSize,
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
