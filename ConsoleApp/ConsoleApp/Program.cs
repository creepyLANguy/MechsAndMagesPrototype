namespace MaM;

public static class Program
{
  private static void Main()
  { 
#if GENERATEDEBUGMAPS
    Generators.GameGenerator.GenerateDebugMaps();
    return;
#endif

    while (Menus.MainMenu.Show())
    {
    }
  }
}