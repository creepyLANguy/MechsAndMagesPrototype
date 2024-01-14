using MaM.Enums;

namespace MaM.Definitions;

public class Campsite : Node
{
  public int countCardsOnOffer;

  //TODO - make countCardsOnOffer configurable
  public Campsite(Node baseNode, int countCardsOnOffer = 3)
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
    this.countCardsOnOffer = countCardsOnOffer;
  }
}