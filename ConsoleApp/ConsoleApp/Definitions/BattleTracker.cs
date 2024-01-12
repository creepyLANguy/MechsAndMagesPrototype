namespace MaM.Definitions;

public class BattleTracker
{
    public int power = 0;
    public int manna = 0;
    public int threat = 0;
    public bool enemyIsDefending = false;
    public int enemyHealth = 0;
    public int playerHealth = 0;

    public BattleTracker(int playerHealth, int enemyHealth)
    {
      this.playerHealth = playerHealth;
      this.enemyHealth = enemyHealth;
    }
}