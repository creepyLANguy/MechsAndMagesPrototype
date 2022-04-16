using System;

namespace MaM.Helpers
{
 public static class UserInput
  {
    public static string Get()
    {
      return Console.ReadLine();
    }

    public static string GetString(string message = null)
    {
      if (string.IsNullOrEmpty(message) == false)
      {
        Console.WriteLine(message);
      }
      
      return Get();
    }

    public static int GetInt(string message = null)
    {
      var input = GetString(message);

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
  }
}
