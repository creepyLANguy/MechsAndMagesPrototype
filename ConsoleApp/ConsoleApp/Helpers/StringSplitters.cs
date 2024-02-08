using System.Text.RegularExpressions;

namespace MaM.Helpers;

class StringSplitters
{
  public static string GetAlphabeticPart(string input)
  {
    input = input.Trim();
    const string pattern = @"([a-zA-Z]+)";
    var match = Regex.Match(input, pattern);
    return match.Success ? match.Groups[1].Value : string.Empty;
  }

  public static int GetNumericPart(string input)
  {
    input = input.Trim();
    const string pattern = @"(\d+)";
    var match = Regex.Match(input, pattern);
    return match.Success ? int.Parse(match.Groups[1].Value) : 0;
  }
}