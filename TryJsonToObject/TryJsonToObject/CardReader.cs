using System.Collections.Generic;
using System.Linq;
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

    private static int? GetCardActionValue(string str, Action action)
    {
      if (str.Contains(action.Key) == false) return null;

      //str = str.Replace(action, "");
      var stripped = str.Substring(action.Key.Length);

      return int.Parse(stripped);
    }

    private static ActionsValuesSet ConstructActionSet(string actionString)
    {
      if (actionString == null) return new ActionsValuesSet();

      var set = new ActionsValuesSet();

      var actions = actionString.Split(JoinedActionDelim);

      foreach (var action in actions)
      {
        int? val;

        val = GetCardActionValue(action, Actions.Attack);
        set.Attack = val ?? set.Attack;

        val = GetCardActionValue(action, Actions.Draw);
        set.Draw = val ?? set.Draw;

        val = GetCardActionValue(action, Actions.Scrap);
        set.Scrap = val ?? set.Scrap;

        val = GetCardActionValue(action, Actions.OpponentDiscard);
        set.OpponentDiscard = val ?? set.OpponentDiscard;

        val = GetCardActionValue(action, Actions.Consume);
        set.Consume = val ?? set.Consume;

        val = GetCardActionValue(action, Actions.Heal);
        set.Heal = val ?? set.Heal;

        val = GetCardActionValue(action, Actions.Trade);
        set.Trade = val ?? set.Trade;
      }

      return set;
    }

    private static void PopulateCardsFromJsonIntermediates(ref List<JsonIntermediateCard> intermediateCards, ref List<Card> cards)
    {
      foreach (var ic in intermediateCards)
      {
        var cardType = CardTypes.All.SingleOrDefault(s => s.Key == ic.Type) ?? CardTypes.Unknown; 
        
        var guild = Guilds.All.SingleOrDefault(s => s.Key == ic.Guild) ?? Guilds.Neutral;

        var card = new Card(
          ic.Id ?? 0,
          ic.Name,
          cardType,
          guild,
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
