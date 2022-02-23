using System;
using System.Text;
using Aspose.Cells;
using MaM.Definitions;

namespace MaM.Readers
{
  public static class EnemyReader
  {
    private const string JoinedDelim = ",";

    //TODO - refactor this! 
    public static EnemyNames GetEnemyNames(string excelFile)
    {
      var names = new EnemyNames();

      var workbook = new Workbook(excelFile, new LoadOptions(LoadFormat.Auto));
      var cells = workbook.Worksheets[0].Cells;

      var col = 0;
      for (var row = 1; row <= cells.LastCell.Row && cells.GetCell(row, col) != null; ++row)
      {
        var cell = cells.GetCell(row, col);
        var s = cell.GetStringValue(CellValueFormatStrategy.None);
        names.pre.Add(s);
      }

      ++col;
      for (var row = 1; row <= cells.LastCell.Row && cells.GetCell(row, col) != null; ++row)
      {
        var cell = cells.GetCell(row, col);
        var s = cell.GetStringValue(CellValueFormatStrategy.None);
        names.neutralDescriptors.Add(s);
      }

      ++col;
      for (var row = 1; row <= cells.LastCell.Row && cells.GetCell(row, col) != null; ++row)
      {
        var cell = cells.GetCell(row, col);
        var s = cell.GetStringValue(CellValueFormatStrategy.None);
        names.borgDescriptors.Add(s);
      }

      ++col;
      for (var row = 1; row <= cells.LastCell.Row && cells.GetCell(row, col) != null; ++row)
      {
        var cell = cells.GetCell(row, col);
        var s = cell.GetStringValue(CellValueFormatStrategy.None);
        names.mechDescriptors.Add(s);
      }
      
      ++col;
      for (var row = 1; row <= cells.LastCell.Row && cells.GetCell(row, col) != null; ++row)
      {
        var cell = cells.GetCell(row, col);
        var s = cell.GetStringValue(CellValueFormatStrategy.None);
        names.mageDescriptors.Add(s);
      }
      
      ++col;
      for (var row = 1; row <= cells.LastCell.Row && cells.GetCell(row, col) != null; ++row)
      {
        var cell = cells.GetCell(row, col);
        var s = cell.GetStringValue(CellValueFormatStrategy.None);
        names.necroDescriptors.Add(s);
      }
      
      ++col;
      for (var row = 1; row <= cells.LastCell.Row && cells.GetCell(row, col) != null; ++row)
      {
        var cell = cells.GetCell(row, col);
        var s = cell.GetStringValue(CellValueFormatStrategy.None);
        names.collective.Add(s);
      }
      
      ++col;
      for (var row = 1; row <= cells.LastCell.Row && cells.GetCell(row, col) != null; ++row)
      {
        var cell = cells.GetCell(row, col);
        var s = cell.GetStringValue(CellValueFormatStrategy.None);
        names.post.Add(s);
      }

      ++col;
      for (var row = 1; row <= cells.LastCell.Row && cells.GetCell(row, col) != null; ++row)
      {
        var cell = cells.GetCell(row, col);
        var s = cell.GetStringValue(CellValueFormatStrategy.None);
        names.place.Add(s);
      }

      return names;
    }

    //TODO - refactor!
    public static string GetEnemyName(Guild guild, ref EnemyNames enemyNames, ref Random random)
    {
      var buff = new StringBuilder();

      var descriptor = "";

      if (guild == Guilds.Neutral)
      {
        descriptor += enemyNames.neutralDescriptors[random.Next(0, enemyNames.neutralDescriptors.Count)];
      }
      else if (guild == Guilds.Borg)
      {
        descriptor += enemyNames.borgDescriptors[random.Next(0, enemyNames.borgDescriptors.Count)];
      }
      else if (guild == Guilds.Mech)
      {
        descriptor += enemyNames.mechDescriptors[random.Next(0, enemyNames.mechDescriptors.Count)];
      }
      else if (guild == Guilds.Mage)
      {
        descriptor += enemyNames.mageDescriptors[random.Next(0, enemyNames.mageDescriptors.Count)];
      }
      else if (guild == Guilds.Necro)
      {
        descriptor += enemyNames.necroDescriptors[random.Next(0, enemyNames.necroDescriptors.Count)];
      }

      var pre = enemyNames.pre[random.Next(0, enemyNames.pre.Count)];
      var l = pre.Split(JoinedDelim);
      if (l[0].ToLower() == "a")
      {
        pre = IsVowel(descriptor[0]) ? l[1] : l[0];
      }

      var collective = enemyNames.collective[random.Next(0, enemyNames.collective.Count)];

      var post = enemyNames.post[random.Next(0, enemyNames.post.Count)];

      var place = enemyNames.place[random.Next(0, enemyNames.place.Count)];

      buff
        .Append(pre.Trim())
        .Append(' ')
        .Append(descriptor.Trim())
        .Append(' ')
        .Append(collective.Trim())
        .Append(' ')
        .Append(post.Trim())
        .Append(' ')
        .Append(place.Trim())
        .Append(' ')
        ;

      //AL.
#if DEBUG
      Console.WriteLine(buff);
#endif
      //

      return buff.ToString();
    }

    private static bool IsVowel(char c)
    {
      return "aeiouAEIOU".Contains(c);
    }
  }
}
