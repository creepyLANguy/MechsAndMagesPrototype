using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;
using MaM.Helpers;
using Newtonsoft.Json;

namespace MaM.Readers;

public struct JsonIntermediateEnemy
{
  public string id;
  public string name;
  public int health;
  public int marketSize;
  public int quantity;
  public string turnActions;
}

public static class EnemyReader
{
  private static List<Tuple<EnemyTurnAction, int>> GetListOfEnemyTurnActions(string turnActions)
  {
    var list = new List<Tuple<EnemyTurnAction, int>>();

    if (turnActions == null)
    {
      return list;
    }

    var splits = turnActions.Split(StringLiterals.ListDelim).ToList();
    foreach (var split in splits)
    {
      var marker = StringSplitters.GetAlphabeticPart(split);
      if (Enum.TryParse<EnemyTurnAction>(marker, out var enemyTurnAction))
      {
        var numericValue = StringSplitters.GetNumericPart(split);
        list.Add(new Tuple<EnemyTurnAction, int>(enemyTurnAction, numericValue));
      }
    }

    return list;
  }

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
          intermediateEnemy.health,
          intermediateEnemy.marketSize,
          GetListOfEnemyTurnActions(intermediateEnemy.turnActions)
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