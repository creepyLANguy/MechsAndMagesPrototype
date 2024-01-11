using MaM.GameLogic;
using MaM.Helpers;
using MaM.Definitions;

namespace MaM.Menus;

public static class MainMenu
{
  public static bool Show()
  {
    var keepRunning = true;

    ConsoleMessages.ShowMainMenu();

#if DEBUG
    var choice = MainMenuItem.PLAY;
#else 
    var choice = UserInput.GetInt();
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