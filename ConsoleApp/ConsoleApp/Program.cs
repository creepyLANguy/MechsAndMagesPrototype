using System;

namespace MaM
{
  class Program
  {
    private static readonly string    ExcelFile         = "Cards.xlsx";
    
    private static readonly int       JourneyLength     = 3;
    
    private static readonly int       MapWidth          = 7;
    private static readonly int       MapHeight         = 15;
    private static readonly int       PathDensity       = 16;

    private static readonly double    CampsiteFrequency = 0.175;
    private static readonly double    EliteFrequency    = 0.2;
    private static readonly double    MysteryFrequency  = 0.2;

    private static readonly int       RandomSeed        = (int)DateTime.Now.Ticks;

    private static readonly MapConfig MapConfig         = new MapConfig(MapWidth, MapHeight, PathDensity, CampsiteFrequency, MysteryFrequency, EliteFrequency);

    static void Main(string[] args)
    {
      var cards = CardReader.GetCardsFromExcel(ExcelFile);

      var journey = JourneyGenerator.GenerateJourney(JourneyLength, MapConfig, RandomSeed);

      for (var i = 0; i < journey.Maps.Count; ++i)
      {
        var dotFileString = Utilities.GenerateDotFileString(journey.Maps[i], "Map_" + (i + 1));
        var dotFileName = "Map_" + (i + 1) + "_" + DateTime.Now.Ticks + ".dot";
        Utilities.SaveFile(dotFileName, dotFileString);
      }

      //create player
      //assign player's deck, etc
      //setup each node fully
      //...
    }
  }
}
