namespace MaM.Definitions;

public class Combatant 
{
  public int power = 0;
  public int manna = 0;
  public int health = 0;
  public bool isDefending = false;

  public Combatant(int health)
  {
    this.health = health;
  }
}

