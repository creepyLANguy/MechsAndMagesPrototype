using MaM.Definitions;
using System;
using System.Collections.Generic;

namespace MaM.Helpers
{
  class ConsoleMessages
  {
    public static void PrintBattleState(ref Player player, ref Enemy enemy, ref int power)
    {
      Console.WriteLine();

      Console.WriteLine("Your Life:\t" + player.health);

      Console.WriteLine("Your Power:\t" + power);

      Console.WriteLine("Enemy Life:\t" + enemy.health);

      Console.WriteLine("Enemy Manna:\t" + enemy.manna);
    }
    
    public static void PrintCards(List<Card> cards)
    {
      const char tab = '\t';
      const char pipe = '|';

      foreach (var card in cards)
      {
        Console.WriteLine(
          UserInput.GetPrintableCardName(card.name) +
          tab + tab + pipe + tab +
          "Manna Cost :" + card.cost +
          tab + pipe + tab +
          "Type:" + card.type +
          tab + pipe + tab +
          "Guild:" + card.guild
        );
      }

      Console.WriteLine();
    }
  }
}
