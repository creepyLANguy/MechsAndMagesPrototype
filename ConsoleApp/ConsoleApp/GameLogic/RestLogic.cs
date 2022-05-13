using MaM.Definitions;

namespace MaM.GameLogic;

public static class Rest
{
  public static bool Run(ref Player player, Campsite node)
  {
    //TODO - implement resting logic
    player.health += new System.Random((int)(System.DateTime.Now.Ticks)).Next(0, 2) == 0 ? 1 : 0;
    return true;
  }
}