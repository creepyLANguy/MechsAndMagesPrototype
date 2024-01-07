using System.Collections.Generic;

namespace MaM.Definitions;

public struct GameConfig
{
  public string                     cardsExcelFile;
  public string                     bossesExcelFile;
  public string                     enemyNamesExcelFile;
  public List<MapConfig>            mapConfigs;
  public EnemyConfig                normalEnemyConfig;
  public EnemyConfig                eliteEnemyConfig;
  public PlayerConfig               playerConfig;
  public List<InitialCardSelection> initialCardSelections;
  public int                        journeyLength;
}