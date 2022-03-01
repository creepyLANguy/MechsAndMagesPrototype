using MaM.Helpers;

namespace MaM.Menus
{
  static class MainMenu
  {
    private enum MainMenuItems
    {
      Play = 1,
      Exit = 2,
    }

    public static void Load()
    {
      var requestString =
        "\nMake a choice:" +
        "\n" + MainMenuItems.Play.ToString("D") + ") " + MainMenuItems.Play +
        "\n" + MainMenuItems.Exit.ToString("D") + ") " + MainMenuItems.Exit;

      var choice = UserInput.RequestInt("\nMain Menu\n1) Play\n2) Exit");
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
