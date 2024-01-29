using MaM.Definitions;
using MaM.Generators;
using MaM.Helpers;
using MaM.GameplayLogic;
using MaM.Readers;

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