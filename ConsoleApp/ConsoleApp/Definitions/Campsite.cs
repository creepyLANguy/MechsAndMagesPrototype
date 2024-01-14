using MaM.Enums;

namespace MaM.Definitions;

public class Campsite : Node
{
  public int countCardsOnOffer; //Make sure this is set during CompleteSetupOfAllNodes()

  public Campsite(Node baseNode)
    : base(
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