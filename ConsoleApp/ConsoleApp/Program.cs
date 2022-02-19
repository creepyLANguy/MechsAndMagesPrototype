using System;
using MaM.GameLogic;

namespace MaM
{
  internal static class Program
  {
    private const string gameConfigFilename   = "gameconfig.json";
    private const string saveFilename         = null; //TODO - test with full savefile containing complete player definition.

    private const string cryptoKey            = "嵵߬ꇄ寘汅浫䔜ꌰ"; //TODO - use crypto class when doing file io

    private static void Main()
    {
      var startTime = DateTime.Now;

      Game.Run(gameConfigFilename, saveFilename);

      Console.WriteLine("Duration: " + DateTime.Now.Subtract(startTime));
    }
  }
}
