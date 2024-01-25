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
    Guild guild
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
    nodeType = NodeType.FIGHT;
    enemy = new Enemy();
    this.fightType = fightType;
    this.guild = guild;
  }
}