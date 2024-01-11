namespace MaM.Definitions;

public class Enemy
{
  public Enemy()
  {
  }

  public Enemy(
    string name,
    int health,
    int baseManna,
    int marketSize
  )
  {
    this.name = name;
    this.health = health;
    this.baseManna = baseManna;
    this.marketSize = marketSize;
  }

  public string name;
  public int health;
  public int baseManna;
  public int marketSize;
}