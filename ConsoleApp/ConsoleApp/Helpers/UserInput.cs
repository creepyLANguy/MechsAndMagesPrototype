using System;
using MaM.Definitions;

namespace MaM.Helpers;

public static class UserInput
{
  public static string Get()
    => Console.ReadLine();

  public static string GetString()
  {   
    return Get();
  }

  public static int GetInt()
  {
    var input = GetString();

    while (string.IsNullOrEmpty(input))
    {
      input = Get();
    }

    return int.Parse(input);
  }

  public static string GetPrintableCardName(string cardName)
  {
    const int printableCardNameLength = 14;
    const char spacer = ' ';
    const string ellipsis = "...";

    return cardName.Length <= printableCardNameLength
      ? cardName.PadRight(printableCardNameLength, spacer)
      : cardName.Substring(0, 1 + (printableCardNameLength - ellipsis.Length)) + ellipsis;
  }

  public static string GetPrintableCardAbilities(Card card)
  {
    var buffer = "";
    foreach (var cardDefaultAction in card.defaultActions)
    {
      buffer += cardDefaultAction.Item1;
      buffer += cardDefaultAction.Item2;
      buffer += ",";
    }
    return buffer;
  }
}