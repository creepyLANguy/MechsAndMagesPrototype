using System.IO;
using MaM.Definitions;
using Newtonsoft.Json;

namespace MaM.Readers
{
  public static class GameConfigReader 
  {
    public static GameConfig GetGameConfigFromFile(string filename)
    {
      var content = File.ReadAllText(filename);

      var gameConfig = JsonConvert.DeserializeObject<GameConfig>(content);

      return gameConfig;
    }
  }
}
