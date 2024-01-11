using System;

namespace MaM.Definitions;

public class Fight : Node
{
  public FightType fightType;
  public Enemy enemy;
  public Guild guild;
  
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

    guild = GetRandomNonNeutralGuild(ref random);
  }

  private Guild GetRandomNonNeutralGuild(ref Random random)
  {
    var chosen = Guild.NEUTRAL;
    var values = Enum.GetValues(typeof(Guild));
    while (chosen == Guild.NEUTRAL)
    {
      chosen = (Guild)values.GetValue(random.Next(values.Length));
    }

    return chosen;
  }
  
}