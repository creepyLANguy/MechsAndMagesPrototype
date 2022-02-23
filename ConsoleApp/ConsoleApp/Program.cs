using System;
using MaM.GameLogic;

namespace MaM
{
  internal static class Program
  {
    private const string gameConfigFilename   = "gameconfig.json";
    private const string saveFilename         = null;

    private const string cryptoKey            = "嵵߬ꇄ寘汅浫䔜ꌰ"; //= null //TODO - use
    //private const string cryptoKey            = null;
    
    private static void Main()
    {
      var startTime = DateTime.Now;

      Game.Run(gameConfigFilename, saveFilename, cryptoKey);

      Console.WriteLine("Duration: " + DateTime.Now.Subtract(startTime));
    }
  }
}
