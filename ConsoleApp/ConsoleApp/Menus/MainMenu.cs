using MaM.Helpers;
using MaM.Definitions;
using MaM.NodeVisitLogic;
using static MaM.Menus.MainMenuItem;

namespace MaM.Menus;

public static class MainMenu
{
  public static bool Show()
  {
    var keepRunning = true;

    Terminal.ShowMainMenu();

    var choice = (MainMenuItem) UserInput.GetInt((int?) PLAY);
    switch (choice)
    {
      case PLAY:
        var saveFile = SaveGameHelper.PromptUserToSelectSaveSlot();
        Navigation.Run(SaveGame.GameConfigFilename, saveFile, SaveGame.CryptoKey);
        break;
      case EXIT:
      default:
        keepRunning = ExitMenu.Show();
        break;
    }

    return keepRunning;
  }
}