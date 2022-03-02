using System;
using System.Collections.Generic;
using System.Text;
using Aspose.Cells;
using MaM.Definitions;

namespace MaM.Readers
{
  public static class EnemyReader
  {
    private const string  JoinedDelim = ",";
    private const char    Space       = ' ';
    private const string  Vowels      = "aeiouAEIOU";

    public static EnemyNames GetEnemyNames(string excelFile)
    {
      var workbook = new Workbook(excelFile, new LoadOptions(LoadFormat.Auto));
      var cells = workbook.Worksheets[0].Cells;

      var listOfLists = new List<List<string>>();
      for (var col = 0; col < cells.Columns.Count; ++col)
      {
        var list = new List<string>();
        for (var row = 1; row <= cells.LastCell.Row && cells.GetCell(row, col) != null; ++row)
        {
          var cell = cells.GetCell(row, col);
          var s = cell.GetStringValue(CellValueFormatStrategy.None);
          list.Add(s);
        }

        listOfLists.Add(list);
      }

      return new EnemyNames(listOfLists);
    }

    public static string GetEnemyName(Guild guild, ref EnemyNames enemyNames, ref Random random)
    {
      var buff = new StringBuilder();

      var i = 1 + guild.Value;
      var descriptor = enemyNames.allLists[i][random.Next(0, enemyNames.allLists[i].Count)];

      var pre = enemyNames.pre[random.Next(0, enemyNames.pre.Count)];
      var l = pre.Split(JoinedDelim);
      if (l[0].ToLower() == "a")
      {
        pre = Vowels.Contains(descriptor[0]) ? l[1] : l[0];
      }

      var collective = enemyNames.collective[random.Next(0, enemyNames.collective.Count)];

      var post = enemyNames.post[random.Next(0, enemyNames.post.Count)];

      var place = enemyNames.place[random.Next(0, enemyNames.place.Count)];

      buff
        .Append(pre.Trim())
        .Append(Space)
        .Append(descriptor.Trim())
        .Append(Space)
        .Append(collective.Trim())
        .Append(Space)
        .Append(post.Trim())
        .Append(Space)
        .Append(place.Trim())
        .Append(Space)
        ;

#if DEBUG
      //Console.WriteLine(buff);
#endif

      return buff.ToString();
    }
  }
}
