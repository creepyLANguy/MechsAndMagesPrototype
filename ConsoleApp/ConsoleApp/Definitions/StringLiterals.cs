using System.Text.RegularExpressions;

namespace MaM.Definitions;

public static class StringLiterals
{
  public static readonly string ListDelim = ",";
  public static readonly string Vowels    = "aeiouAEIOU";

  public static string ToSentenceCase(this string str)
  {
    var lowerCase = str.ToLower(); 
    var r = new Regex(@"(^[a-z])|\.\s+(.)", RegexOptions.ExplicitCapture);
    return r.Replace(lowerCase, s => s.Value.ToUpper());
  }
}