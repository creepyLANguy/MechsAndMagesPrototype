using System.Collections.Generic;
using MaM.Definitions;
using MaM.Helpers;
using Newtonsoft.Json;

namespace MaM.Readers;

public struct JsonIntermediateBoss
{
  public string   name          ;
  public int      health        ;
  public int      manna         ;
}

public static class BossReader
{
  private static List<Enemy> PopulateBossesFromJsonIntermediates(List<JsonIntermediateBoss> intermediateBosses, ref List<Card> cards)
  {
    var bosses = new List<Enemy>();

    foreach (var intermediateBoss in intermediateBosses)
    {
      var boss = new Enemy(
        intermediateBoss.name,
        intermediateBoss.health,
        intermediateBoss.manna
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