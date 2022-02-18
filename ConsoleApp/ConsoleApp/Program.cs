using MaM.Generators;
using MaM.Helpers;

namespace MaM
{
  internal static class Program
  {
    private const string gameConfigFile   = "gameconfig.json";
    private const string saveFilename     = null; //TODO - test with full savefile containing complete player definition.

    private const string cryptoKey       = "嵵߬ꇄ寘汅浫䔜ꌰ"; //TODO - use crypto class when doing file io
    //var gch = GCHandle.Alloc(key, GCHandleType.Pinned);
    //...
    //ZeroMemory(gch.AddrOfPinnedObject(), key.Length * 2);
    //gch.Free();

    private static void Main()
    {
      var gameConfig = FileHelper.GetGameConfigFromFile(gameConfigFile);

      var gameContents = GameGenerator.GenerateGame(saveFilename, gameConfig);

      GameLogic.Run(ref gameContents);
    }


    //AL.
    private static void TryCrypto(string standardFileName, string encryptedFileName)
    {
      var s = "";
      for (var i = 0; i < 100; ++i)
      {
        var d = Crypto.GenerateKey();
        s += d + "\n";
      }
      FileHelper.WriteFileToDrive("FISHES.txt", s);
      Crypto.EncryptFile(standardFileName, encryptedFileName, cryptoKey);
      Crypto.DecryptFile(encryptedFileName, standardFileName, cryptoKey);
    }
    //


  }
}
