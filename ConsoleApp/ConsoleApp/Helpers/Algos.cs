using System;
using System.Collections.Generic;
using System.Linq;
using MaM.Definitions;

namespace MaM.Helpers
{ 
  public static class Algos
  {
    public static void Shuffle<T>(this IList<T> list, ref Random random)
    {
      for (var i = list.Count - 1; i > 0; --i)
      {
        var randomIndex = random.Next(i + 1);
        (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
      }
    }

    public static int GenerateRandomSeed() => 
      Math.Abs((int)DateTime.Now.Ticks);

    public static Random GenerateNewRandom(int? randomSeed = null)
      => new Random(randomSeed ?? GenerateRandomSeed());


    private static Player SelectRandomPlayerWhileAccountingForInitiatives(this List<Player> players, ref Random random)
    {
      var sumInitiatives = players.Sum(player => player.initiative);

      var randomNumber = random.Next(0, sumInitiatives);

      Player selectedPlayer = null;
      foreach (var player in players)
      {
        if (randomNumber < player.initiative)
        {
          selectedPlayer = player;
          break;
        }

        randomNumber -= player.initiative;
      }

      return selectedPlayer;
    }

    public static void ShuffleWhileAccountingForInitiatives(this List<Player> players, ref Random random)
    {
      var newList = new List<Player>();

      var totalPlayerCount = players.Count;
      while (newList.Count < totalPlayerCount)
      {
        var selectedPlayer = players.SelectRandomPlayerWhileAccountingForInitiatives(ref random);
        newList.Add(selectedPlayer);
        players.Remove(selectedPlayer);
      }

      players.AddRange(newList);
    }

    public static List<Player> DeepCopyPlayerList(ref List<Player> list) 
      => list.Select(player => new Player(player)).ToList();
  }
}
