namespace MaM.Definitions;

public class Combatant 
{
  public int power;
  public int manna;
  public int health;
  public bool isDefending;

  public Combatant(int health, int power = 0, int manna = 0, bool isDefending = false)
  {
    this.health = health;
    this.power = power;
    this.manna = manna;
    this.isDefending = isDefending;
  }
}

