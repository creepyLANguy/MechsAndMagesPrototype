using MaM.Helpers;
using MaM.Definitions;
using MaM.NodeVisitLogic;

namespace MaM.Menus;

public static class MainMenu
{
  public static bool Show()
  {
    var keepRunning = true;

    Terminal.ShowMainMenu();

#if DEBUG
    var choice = MainMenuItem.PLAY;
#else 
    var choice = (MainMenuItem)UserInput.GetInt();
#endif
    //TODO - Create mode where scenario is loaded for quick playtesting. 
    switch (choice)
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