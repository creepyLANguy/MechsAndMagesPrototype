using System.Collections.Generic;

namespace MaM.Definitions;

public class Campsite : Node
{
  public Campsite(
    Node        baseNode,
    List<Card>  recruits
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
    this.recruits   = recruits;
    nodeType        = NodeType.CampSite;
  }

  public List<Card> recruits;

  //TODO - implement these types
  //public List<Potion> Potions;
  //public List<Relic> Relics;
  //public List<HealingKit> HealingKits;
  //public List<VisionUpgrade> VisionUpgrades;
  //public List<MaxHealthUpgrade> HealthUpgrades;
  //public Scrapper Scrapper;
}