using MaM.Definitions;
using MaM.Helpers;

namespace MaM.Menus;

public static class ExitMenu
{
  public static bool Show()
  {
    var keepRunning = true;

    ConsoleMessages.ShowExitMenu();

    var choice = UserInput.GetInt();
    switch ((ExitMenuItem)choice)
    {
      case (ExitMenuItem.YES):
        keepRunning = false;
        break;
    }

    return keepRunning;
  }
}