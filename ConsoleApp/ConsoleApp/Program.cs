using MaM.Generators;

namespace MaM;

public static class Program
{
  private static void Main()
  {
#if GENERATEDEBUGMAPS
    GameGenerator.GenerateDebugMaps();
    return;
#endif

    while (Menus.MainMenu.Show());
  }
}