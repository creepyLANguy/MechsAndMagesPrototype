namespace MaM.Definitions;

public class Enemy
{
  public Enemy()
  {
  }

  public Enemy(
    string id,
    string name,
    int health,
    int marketSize
  )
  {
    this.id = id;
    this.name = name;
    this.health = health;
    this.marketSize = marketSize;
  }

  public string id;
  public string name;
  public int health;
  public int marketSize;
}