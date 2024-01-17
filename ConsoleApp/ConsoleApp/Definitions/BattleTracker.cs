namespace MaM.Definitions;

//TODO - delete this and move player and enemy into BattlePack
public class BattleTracker
{
  public BattleResources player = new();
  public BattleResources enemy = new ();

  public BattleTracker(int playerHealth, int enemyHealth)
  {
    player.health = playerHealth;
    enemy.health = enemyHealth;
  }
}