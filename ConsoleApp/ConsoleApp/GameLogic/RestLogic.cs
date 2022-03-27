using MaM.Definitions;

namespace MaM.GameLogic
{
  public static class Rest
  {
    public static bool Run(ref Player player, Campsite node)
    {
      //TODO
      player.health = player.maxHealth;
      return true;
    }
  }
}
