using MaM.Helpers;

namespace MaM.Menus
{
  public static class ExitMenu
  {
    private enum ExitMenuItems
    {
      Yes = 1,
      No  = 2,
    }

    public static bool Show()
    {
      var keepRunning = true;

      var requestString =
        "\nAre you sure you want to exit?" +
        "\n" + ExitMenuItems.Yes.ToString("D") + ") " + ExitMenuItems.Yes + 
        "\n" + ExitMenuItems.No.ToString("D") + ") " + ExitMenuItems.No;

      var choice = UserInput.GetInt(requestString);
      switch ((ExitMenuItems)choice)
      {
        case (ExitMenuItems.Yes):
          keepRunning = false;
          break;
      }

      return keepRunning;
    }
  }
}
