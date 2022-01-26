using System;
using System.Collections.Generic;

namespace TryJsonToObject
{
  class Program
  {
    static void Main(string[] args)
    {
      //var jsonFile = "Cards.json";
      //var json = System.IO.File.ReadAllText(jsonFile);

      var cards = new List<Card>();
      CardReader.GetCardsFromExcel("Cards.xlsx", ref cards);
    }
  }
}
