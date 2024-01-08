namespace MaM.Definitions;

public class Campsite : Node
{
  public Campsite(
    Node        baseNode
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
    nodeType = NodeType.CAMPSITE;
  }
}