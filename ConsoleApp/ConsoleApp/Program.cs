﻿using System;

namespace MaM
{
  class Program
  {
    private static readonly int               RandomSeed              = (int)DateTime.Now.Ticks;

    private static readonly string            ExcelFile               = "Cards.xlsx";
                                                                      
    private static readonly int               JourneyLength           = 3;
                                                                      
    private static readonly int               MapWidth                = 7;
    private static readonly int               MapHeight               = 15;
    private static readonly int               PathDensity             = 16;
                                                                      
    private static readonly double            CampsiteFrequency       = 0.175;
    private static readonly double            EliteFrequency          = 0.2;
    private static readonly double            MysteryFrequency        = 0.2;

    private static readonly MapConfig         MapConfig               
      = new MapConfig(
        MapWidth, 
        MapHeight, 
        PathDensity, 
        CampsiteFrequency, 
        MysteryFrequency, 
        EliteFrequency
        );

    private static readonly int               NormalEnemyHealth       = 50;
    private static readonly int               NormalEnemyManna        = 0;
    private static readonly int               NormalEnemyDeckSize     = 20;
    private static readonly int               NormalTradeRowSize      = 3;
    private static readonly int               NormalEnemyHandSize     = 5;
    private static readonly int               NormalEnemyMinCardCost  = 0;
    private static readonly int               NormalEnemyMaxCardCost  = 7;

    private static readonly EnemyConfig       NormalEnemyConfig       
      = new EnemyConfig(
        NormalEnemyHealth, 
        NormalEnemyManna, 
        NormalEnemyDeckSize, 
        NormalTradeRowSize, 
        NormalEnemyHandSize, 
        NormalEnemyMinCardCost, 
        NormalEnemyMaxCardCost
        );
    

    private static readonly int               EliteEnemyHealth       = 75;
    private static readonly int               EliteEnemyManna        = 0;
    private static readonly int               EliteEnemyDeckSize     = 17;
    private static readonly int               EliteTradeRowSize      = 4;
    private static readonly int               EliteEnemyHandSize     = 5;
    private static readonly int               EliteEnemyMinCardCost  = 4;
    private static readonly int               EliteEnemyMaxCardCost  = 8;

    private static readonly EnemyConfig       EliteEnemyConfig
      = new EnemyConfig(
        EliteEnemyHealth,
        EliteEnemyManna,
        EliteEnemyDeckSize,
        EliteTradeRowSize,
        EliteEnemyHandSize,
        EliteEnemyMinCardCost,
        EliteEnemyMaxCardCost
      );


    static void Main(string[] args)
    {
      Console.WriteLine("Seed: " + RandomSeed);

      var random = new Random(RandomSeed);

      var cards = CardReader.GetCardsFromExcel(ExcelFile);

      var journey = JourneyGenerator.GenerateJourney(JourneyLength, MapConfig, NormalEnemyConfig, EliteEnemyConfig, cards, ref random);

#if DEBUG
      GraphVis.SaveMapsAsDotFiles(ref journey);
#endif

      //create player

      //run game
    }
  }
}
