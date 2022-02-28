using System;
using MaM.GameLogic;

namespace MaM
{
 public static class Program
  {
    private const string GameConfigFilename   = "gameconfig.json";
    private const string SaveFilename         = "sv1"; //This will come from whichever save slot the user selects.

    private const string CryptoKey            = "嵵߬ꇄ寘汅浫䔜ꌰ";
    //private const string CryptoKey            = null;
    
    private static void Main()
    {
      var startTime = DateTime.Now;

      Game.Run(GameConfigFilename, SaveFilename, CryptoKey);

      Console.WriteLine("\nDuration: " + DateTime.Now.Subtract(startTime));
    }
  }
}
