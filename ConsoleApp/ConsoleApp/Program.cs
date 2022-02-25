using System;
using MaM.GameLogic;

namespace MaM
{
 public static class Program
  {
    private const string GameConfigFilename   = "gameconfig.json";
    private const string SaveFilename         = null;

    private const string CryptoKey            = "嵵߬ꇄ寘汅浫䔜ꌰ"; //= null //TODO - use
    //private const string cryptoKey            = null;
    
    private static void Main()
    {
      var startTime = DateTime.Now;

      Game.Run(GameConfigFilename, SaveFilename, CryptoKey);

      Console.WriteLine("Duration: " + DateTime.Now.Subtract(startTime));
    }
  }
}
