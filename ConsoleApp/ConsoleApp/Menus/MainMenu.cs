using MaM.GameLogic;
using MaM.Helpers;
using MaM.Definitions;

namespace MaM.Menus;

public static class MainMenu
{
  private enum MainMenuItems
  {
    Play = 1,
    Exit = 2,
  }

  public static bool Show()
  {
    var keepRunning = true;

    var requestString =
      "\nMain Menu" +
      "\n" + MainMenuItems.Play.ToString("D") + ") " + MainMenuItems.Play +
      "\n" + MainMenuItems.Exit.ToString("D") + ") " + MainMenuItems.Exit;

    var choice = UserInput.GetInt(requestString);
    switch ((MainMenuItems)choice)
    {
      case MainMenuItems.Play:
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