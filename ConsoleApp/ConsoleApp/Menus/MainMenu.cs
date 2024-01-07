using MaM.GameLogic;
using MaM.Helpers;
using MaM.Definitions;

namespace MaM.Menus;

public static class MainMenu
{
  private enum MainMenuItems
  {
    PLAY = 1,
    EXIT = 2,
  }

  public static bool Show()
  {
    var keepRunning = true;

    var requestString =
      "\nMain Menu" +
      "\n" + MainMenuItems.PLAY.ToString("D") + ") " + MainMenuItems.PLAY +
      "\n" + MainMenuItems.EXIT.ToString("D") + ") " + MainMenuItems.EXIT;

    var choice = UserInput.GetInt(requestString);
    switch ((MainMenuItems)choice)
    {
      case MainMenuItems.PLAY:
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