using MaM.GameLogic;
using MaM.Helpers;
using MaM.Definitions;
using System;

namespace MaM.Menus;

public static class MainMenu
{
  private enum MainMenuItem
  {
    PLAY = 1,
    EXIT = 2,
  }

  public static bool Show()
  {
    var keepRunning = true;

    var requestString =
      "\nMain Menu" +
      "\n" + MainMenuItem.PLAY.ToString("D") + ") " + MainMenuItem.PLAY.ToString().ToSentenceCase() +
      "\n" + MainMenuItem.EXIT.ToString("D") + ") " + MainMenuItem.EXIT.ToString().ToSentenceCase();

#if DEBUG
    Console.WriteLine(requestString);
    var choice = MainMenuItem.PLAY;
#else 
    var choice = UserInput.GetInt(requestString);
#endif

    switch ((MainMenuItem)choice)
    {
      case MainMenuItem.PLAY:
        var saveFile = SaveGameHelper.PromptUserToSelectSaveSlot();
        Navigation.Run(SaveGame.GameConfigFilename, saveFile, SaveGame.CryptoKey);
        break;
      default:
        keepRunning = ExitMenu.Show();
        break;
    }

    return keepRunning;
  }
}