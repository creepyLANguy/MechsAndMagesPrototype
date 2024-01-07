namespace MaM.Definitions;

public class Enemy
{
  public Enemy()
  {
  }

  public Enemy(
    string name,
    int health,
    int manna
  )
  {
    this.name = name;
    this.health = health;
    this.manna = manna;
  }

  public string name;
  public int health;
  public int manna;
}