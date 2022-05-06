using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Helpers;
using Newtonsoft.Json;

namespace MaM.Readers;

public struct JsonIntermediateBoss
{
  public string   id            ;
  public string   name          ;
  public int      health        ;
  public int      tradeRowSize  ;
  public int      manna         ;
  public int      handSize      ;
  public int      initiative    ;
  public string   cards         ;
}

public static class BossReader
{
  private static List<Enemy> PopulateBossesFromJsonIntermediates(List<JsonIntermediateBoss> intermediateBosses, ref List<Card> cards)
  {
    var bosses = new List<Enemy>();

    foreach (var intermediateBoss in intermediateBosses)
    {
      var cardIds = intermediateBoss.cards
        .Replace(" ", string.Empty)
        .Split(StringLiterals.Deliminator)
        .ToList();

      var bossCards = CardReader.GetCardsFromIds(cardIds, ref cards);

      var boss = new Enemy(
        intermediateBoss.name,
        intermediateBoss.health,
        intermediateBoss.tradeRowSize,
        intermediateBoss.manna,
        intermediateBoss.handSize,
        intermediateBoss.initiative,
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