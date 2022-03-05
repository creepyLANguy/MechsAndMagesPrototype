using MaM.Helpers;

namespace MaM.Menus
{
  public static class MainMenu
  {
    private enum MainMenuItems
    {
      Play = 1,
      Exit = 2,
    }

    public static void Load()
    {
      var requestString =
        "\nMain Menu" +
        "\n" + MainMenuItems.Play.ToString("D") + ") " + MainMenuItems.Play +
        "\n" + MainMenuItems.Exit.ToString("D") + ") " + MainMenuItems.Exit;

      var choice = UserInput.RequestInt(requestString);
      switch ((MainMenuItems)choice)
      {
        case MainMenuItems.Play:
          SaveGameHelper.PromptUserToSelectSaveSlot();
          break;
        default:
          ExitMenu.Load();
          break;
      }
    }
  }
}
