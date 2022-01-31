using System;
using System.Collections.Generic;

namespace TryJsonToObject
{
  class Program
  {
    static readonly string ExcelFile = "Cards.xlsx";
    static readonly int JourneyLength = 3;
    static readonly int MapWidth = 7;
    static readonly int MapHeight = 15;

    static readonly Random Random = new Random((int)DateTime.Now.Ticks);

    static void Main(string[] args)
    {
      //var jsonFile = "Cards.json";
      //var json = System.IO.File.ReadAllText(jsonFile);

      var cards = new List<Card>();
      CardReader.GetCardsFromExcel(ExcelFile, ref cards);

      var journey = new Journey();
      JourneyGenerator.GenerateJourney(JourneyLength, MapWidth, MapHeight, Random, ref journey);

      //create player
      //assign player's deck, etc
      //...
    }
  }
}
