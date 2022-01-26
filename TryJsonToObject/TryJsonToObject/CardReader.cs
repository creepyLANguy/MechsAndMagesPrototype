using System.Collections.Generic;
using Aspose.Cells;
using Aspose.Cells.Utility;
using Newtonsoft.Json;

namespace TryJsonToObject
{
  internal class JsonIntermediateCard
  {
    public int? Quantity { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public string Guild { get; set; }
    public int? Cost { get; set; }
    public int? Defense { get; set; }
    public int? Shield { get; set; }
    public string DefaultAbilities { get; set; }
    public string GuildBonuses { get; set; }
    public string AllyBonuses { get; set; }
    public string ScrapBonuses { get; set; }
    public int? Id { get; set; }
  }

  public static class CardReader
  {
    private static readonly string JoinedActionDelim = ",";

    private static int? GetActionValue(string str, string action)
    {
      if (str.Contains(action) == false) return null;

      //str = str.Replace(action, "");
      var stripped = str.Substring(action.Length);

      return int.Parse(stripped);
    }

    private static ActionSet ConstructActionSet(string actionString)
    {
      if (actionString == null) return new ActionSet();

      var set = new ActionSet();

      var actions = actionString.Split(JoinedActionDelim);

      foreach (var action in actions)
      {
        int? val;

        val = GetActionValue(action, Actions.Attack);
        set.Attack = val ?? set.Attack;

        val = GetActionValue(action, Actions.Draw);
        set.Draw = val ?? set.Draw;

        val = GetActionValue(action, Actions.Scrap);
        set.Scrap = val ?? set.Scrap;

        val = GetActionValue(action, Actions.OpponentDiscard);
        set.OpponentDiscard = val ?? set.OpponentDiscard;

        val = GetActionValue(action, Actions.Consume);
        set.Consume = val ?? set.Consume;

        val = GetActionValue(action, Actions.Heal);
        set.Heal = val ?? set.Heal;

        val = GetActionValue(action, Actions.Trade);
        set.Trade = val ?? set.Trade;
      }

      return set;
    }

    private static void PopulateCardsFromJsonIntermediates(ref List<JsonIntermediateCard> intermediateCards, ref List<Card> cards)
    {
      foreach (var ic in intermediateCards)
      {
        var card = new Card(
          ic.Id ?? 0,
          ic.Name,
          ic.Type,
          ic.Guild,
          ic.Cost ?? 0,
          ic.Defense ?? 0,
          ic.Shield ?? 0,
          ConstructActionSet(ic.DefaultAbilities),
          ConstructActionSet(ic.GuildBonuses),
          ConstructActionSet(ic.AllyBonuses),
          ConstructActionSet(ic.ScrapBonuses)
        );

        for (var i = 0; i < ic.Quantity; ++i)
        {
          cards.Add(card);
        }
      }
    }

    private static List<JsonIntermediateCard> GetIntermediateCardsFromJson(ref string json)
    {
      return JsonConvert.DeserializeObject<List<JsonIntermediateCard>>(json);
    }

    private static string ExcelToJson(string excelFile)
    {
      var workbook = new Workbook(excelFile, new LoadOptions(LoadFormat.Auto));

      var cells = workbook.Worksheets[0].Cells;
      
      var range = cells.CreateRange(0, 0, cells.LastCell.Row + 1, cells.LastCell.Column + 1);
      
      var exportOptions = new ExportRangeToJsonOptions();
      
      var json = JsonUtility.ExportRangeToJson(range, exportOptions);
      
      //System.IO.File.WriteAllText("output.json", jsonData);
      
      return json;
    }

    public static void GetCardsFromExcel(string excelFile, ref List<Card> cards)
    {
      var json = ExcelToJson(excelFile);

      var intermediateCards = GetIntermediateCardsFromJson(ref json);

      PopulateCardsFromJsonIntermediates(ref intermediateCards, ref cards);
    }
  }
}
