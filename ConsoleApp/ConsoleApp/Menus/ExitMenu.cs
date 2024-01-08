using MaM.Definitions;
using MaM.Helpers;

namespace MaM.Menus;

public static class ExitMenu
{
  private enum ExitMenuItem
  {
    YES = 1,
    NO  = 2,
  }

  public static bool Show()
  {
    var keepRunning = true;

    var requestString =
      "\nAre you sure you want to exit?" +
      "\n" + ExitMenuItem.YES.ToString("D") + ") " + ExitMenuItem.YES.ToString().ToSentenceCase() + 
      "\n" + ExitMenuItem.NO.ToString("D") + ") " + ExitMenuItem.NO.ToString().ToSentenceCase();

    var choice = UserInput.GetInt(requestString);
    switch ((ExitMenuItem)choice)
    {
      case (ExitMenuItem.YES):
        keepRunning = false;
        break;
    }

    return keepRunning;
  }
}