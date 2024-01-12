using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using MaM.Definitions;
using MaM.Helpers;
using Newtonsoft.Json;

namespace MaM.Readers;

public struct JsonIntermediateEnemy
{
  public string id;
  public string name;
  public int baseHealth;
  public int baseManna;
  public int marketSize;
  public int quantity;
}

public static class EnemyReader
{
  private static List<Enemy> PopulateEnemiesFromJsonIntermediates(List<JsonIntermediateEnemy> intermediateEnemies)
  {
    var enemies = new List<Enemy>();

    foreach (var intermediateEnemy in intermediateEnemies)
    {
      for (var i = 0; i < intermediateEnemy.quantity; ++i)
      {
        var enemy = new Enemy(
          intermediateEnemy.id,
          intermediateEnemy.name,
          intermediateEnemy.baseHealth,
          intermediateEnemy.baseManna,
          intermediateEnemy.marketSize
        );

        enemies.Add(enemy);
      }
    }

    return enemies;
  }

  public static List<Enemy> GetEnemiesFromExcel(string excelFile)
  {
    var json = FileHelper.ExcelToJson(excelFile);

    var intermediateEnemies = JsonConvert.DeserializeObject<List<JsonIntermediateEnemy>>(json);

    var enemies = PopulateEnemiesFromJsonIntermediates(intermediateEnemies);

    return enemies;
  }
}