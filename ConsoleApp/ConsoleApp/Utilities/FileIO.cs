using System;
using System.IO;

namespace MaM
{
  static class FileIO
  {
    public static void SaveFile(string filename, string content)
    {
      Console.WriteLine("Saving " + filename);
      File.WriteAllText(filename, content);
      Console.WriteLine("Saved");
    }

  }
}
