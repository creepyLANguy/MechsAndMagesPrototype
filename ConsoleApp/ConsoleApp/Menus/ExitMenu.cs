using MaM.Enums;
using MaM.Helpers;

namespace MaM.Menus;

public static class ExitMenu
{
  public static bool Show()
  {
    var keepRunning = true;

    ConsoleMessages.ShowExitMenu();

    var choice = UserInput.GetInt();
    switch ((YesNoChoice)choice)
    {
      case (YesNoChoice.YES):
        keepRunning = false;
        break;
    }

    return keepRunning;
  }
}