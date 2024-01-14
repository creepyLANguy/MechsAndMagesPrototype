using System.Collections.Generic;

namespace MaM.Definitions;

public struct GameConfig
{
  public string                     cardsExcelFile;
  public string                     normalEnemiesExcelFile;
  public string                     eliteEnemiesExcelFile;
  public string                     bossesExcelFile;
  public List<MapConfig>            mapConfigs;
  public PlayerConfig               playerConfig;
  public List<InitialCardSelection> initialCardSelections;
  public int                        journeyLength;
  public int                        handSize;
  public int                        campsiteCardsOnOfferCount;
}