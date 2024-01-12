namespace MaM.Definitions;

public class Enemy
{
  public Enemy()
  {
  }

  public Enemy(
    string id,
    string name,
    int baseHealth,
    int baseManna,
    int marketSize
  )
  {
    this.id = id;
    this.name = name;
    this.health = baseHealth;
    this.manna = baseManna;
    this.marketSize = marketSize;
  }

  public string id;
  public string name;
  public int health;
  public int manna;
  public int marketSize;
}