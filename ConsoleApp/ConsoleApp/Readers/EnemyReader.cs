using System;
using System.Collections.Generic;
using System.Text;
using Aspose.Cells;
using MaM.Definitions;

namespace MaM.Readers
{
  public static class EnemyReader
  {
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
      var i = guild.Value + 1;
      var descriptor = enemyNames.allLists[i][random.Next(0, enemyNames.allLists[i].Count)];

      var pre = GetPre(enemyNames.pre[random.Next(0, enemyNames.pre.Count)], descriptor[0]);

      var collective = enemyNames.collective[random.Next(0, enemyNames.collective.Count)];

      var post = enemyNames.post[random.Next(0, enemyNames.post.Count)];

      var place = enemyNames.place[random.Next(0, enemyNames.place.Count)];

      var fullEnemyName = BuildFullEnemyNameString(pre, descriptor, collective, post, place);
      
#if DEBUG
      //Console.WriteLine(fullEnemyName);
#endif

      return fullEnemyName;
    }

    private static string GetPre(string s, char c)
    {
      var l = s.Split(StringLiterals.Deliminator);

      if (StringLiterals.A.Contains(l[0]))
      {
        s = StringLiterals.Vowels.Contains(c) ? l[1] : l[0];
      }

      return s;
    }

    private static string BuildFullEnemyNameString(
      string pre,
      string descriptor,
      string collective,
      string post,
      string place
    ) => 
      new StringBuilder()
        .Append(pre.Trim())
        .Append(StringLiterals.Space)
        .Append(descriptor.Trim())
        .Append(StringLiterals.Space)
        .Append(collective.Trim())
        .Append(StringLiterals.Space)
        .Append(post.Trim())
        .Append(StringLiterals.Space)
        .Append(place.Trim())
        .Append(StringLiterals.Space)
        .ToString();
  }
}
