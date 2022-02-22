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
  }
}
