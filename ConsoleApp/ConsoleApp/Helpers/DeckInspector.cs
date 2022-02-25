using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using MaM.Definitions;

namespace MaM.Helpers
{
 public static class DeckInspector
  {
    /*
    public static List<Tuple<Guild, int>> GetGuildsTallyList(ref List<Card> deck)
    {
      var tallyList = new List<int>(new int[Guilds.All.Count]);

      foreach (var card in deck)
      {
        var index = Guilds.All.IndexOf(card.guild);
        ++tallyList[index];
      }

      var fullTally = new List<Tuple<Guild, int>>();

      for (var index = 0; index < Guilds.All.Count; index++)
      {
        fullTally.Add(new Tuple<Guild, int>(Guilds.All[index], tallyList[index]));
      }

      return fullTally;
    }
    */

    public static Guild GetDominantGuild(ref List<Card> deck)
    {
      var deckSize = deck.Count;
      var tallyList = GetAllGuildDistributions(ref deck).Select(item => item.Item2 * deckSize).ToImmutableList();
      var maxVal = tallyList.Max();

      var isMaxGuildCountIsShared = tallyList.Count(value => value.Equals(maxVal)) > 1;

      return isMaxGuildCountIsShared
        ? Guilds.Neutral
        : Guilds.All[tallyList.IndexOf(maxVal)];
    }

    public static List<Tuple<Guild, double>> GetAllGuildDistributions(ref List<Card> deck)
    {
      var allDistributions = new List<Tuple<Guild, double>>();

      foreach (var guild in Guilds.All)
      {
        var currentDistribution = GetGuildDistribution(ref deck, guild);
        allDistributions.Add(new Tuple<Guild, double>(guild, currentDistribution));
      }

      return allDistributions;
    }

    public static double GetGuildDistribution(ref List<Card> deck, Guild guild)
    {
      var count = deck.Count(card => card.guild == guild);

      return (double)count / deck.Count;
    }
  }
}
