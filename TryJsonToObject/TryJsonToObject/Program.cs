using System;
using System.Collections.Generic;

namespace TryJsonToObject
{
  class Program
  {
    private static readonly string  ExcelFile       = "Cards.xlsx";
    private static readonly int     JourneyLength   = 3;
    private static readonly int     MapWidth        = 7;
    private static readonly int     MapHeight       = 15;
    private static readonly int     PathDensity     = 6;

    private static readonly Random Random = new Random((int)DateTime.Now.Ticks);

    static void Main(string[] args)
    {
      //var jsonFile = "Cards.json";
      //var json = System.IO.File.ReadAllText(jsonFile);

      var cards = CardReader.GetCardsFromExcel(ExcelFile);

      var journey = JourneyGenerator.GenerateJourney(JourneyLength, MapWidth, MapHeight, PathDensity, Random);

      //create player
      //assign player's deck, etc
      //...
    }
  }
}
