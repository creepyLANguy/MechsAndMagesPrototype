using System;
using MaM.Enums;
using MaM.Helpers;

namespace MaM.Definitions;

public class Fight : Node
{
  public FightType fightType;
  public Enemy enemy;
  public Guild guild;
  
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

    guild = GetRandomNonNeutralGuild();
  }

  private static Guild GetRandomNonNeutralGuild()
  {
    var chosen = Guild.NEUTRAL;
    var values = Enum.GetValues(typeof(Guild));
    while (chosen == Guild.NEUTRAL)
    {
      chosen = (Guild) values.GetValue(UbiRandom.Next(values.Length));
    }

    return chosen;
  }
  
}