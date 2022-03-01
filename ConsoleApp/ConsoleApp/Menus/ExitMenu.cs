using System;
using MaM.GameLogic;
using MaM.Helpers;

namespace MaM.Menus
{
  static class ExitMenu
  {
    private enum ExitMenuItems
    {
      Yes = 1,
      No  = 2,
    }

    public static void Load()
    {
      var requestString =
        "\nAre you sure you want to exit?" +
        "\n" + ExitMenuItems.Yes.ToString("D") + ") " + ExitMenuItems.Yes + 
        "\n" + ExitMenuItems.No.ToString("D") + ") " + ExitMenuItems.No;

      var choice = UserInput.RequestInt(requestString);
      switch ((ExitMenuItems)choice)
      {
        case (ExitMenuItems.Yes):
          return;
        default:
          MainMenu.Load();
          break;
      }
    }
  }
}
