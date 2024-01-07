namespace MaM.Definitions;

public class Fight : Node
{
  public Fight(
    Node baseNode,
    FightType fightType,
    Enemy enemy = null
  ) : base(
    baseNode.nodeType,
    baseNode.isMystery,
    baseNode.x,
    baseNode.y,
    baseNode.isComplete,
    baseNode.isDestination,
    baseNode.destinations
  )
  {
    this.fightType = fightType;
    this.enemy = enemy ?? new Enemy();
    nodeType = NodeType.FIGHT;
  }

  public FightType fightType;
  public Enemy enemy;
}