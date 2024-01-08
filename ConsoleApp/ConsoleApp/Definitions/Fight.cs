using System;

namespace MaM.Definitions;

public class Fight : Node
{
  public Fight(
    ref Random random,
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

    var values = Enum.GetValues(typeof(Guild));
    guild = (Guild)values.GetValue(random.Next(values.Length));
  }

  public FightType fightType;
  public Enemy enemy;
  public Guild guild;
}