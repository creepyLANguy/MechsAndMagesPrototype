namespace MaM.Definitions;

public class Enemy
{
  public Enemy()
  {
  }

  public Enemy(
    string name,
    int health,
    int manna,
    int tradeRowSize
  )
  {
    this.name = name;
    this.health = health;
    this.manna = manna;
    this.tradeRowSize = tradeRowSize;
  }

  public string name;
  public int health;
  public int manna;
  public int tradeRowSize;
}