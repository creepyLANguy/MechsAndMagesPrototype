using MaM.Helpers;

namespace MaM.Menus;

public static class ExitMenu
{
  private enum ExitMenuItems
  {
    YES = 1,
    NO  = 2,
  }

  public static bool Show()
  {
    var keepRunning = true;

    var requestString =
      "\nAre you sure you want to exit?" +
      "\n" + ExitMenuItems.YES.ToString("D") + ") " + ExitMenuItems.YES + 
      "\n" + ExitMenuItems.NO.ToString("D") + ") " + ExitMenuItems.NO;

    var choice = UserInput.GetInt(requestString);
    switch ((ExitMenuItems)choice)
    {
      case (ExitMenuItems.YES):
        keepRunning = false;
        break;
    }

    return keepRunning;
  }
}