using System;

namespace MaM.Helpers
{
 public static class UserInput
  {
    public static string Get()
    {
      return Console.ReadLine();
    }

    public static string Request(string message)
    {
      Console.WriteLine(message);
      return Get();
    }

    public static string GetPrintableCardName(string cardName)
    {
      const int printableLength = 14;
      const char spacer = ' ';
      const string ellipsis = "...";

      return cardName.Length <= printableLength
        ? cardName.PadRight(printableLength, spacer)
        : cardName.Substring(0, 1 + (printableLength - ellipsis.Length)) + ellipsis;
    }
  }
}
