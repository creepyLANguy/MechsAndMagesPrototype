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
    nodeType = NodeType.CampSite;
  }

  //TODO - implement these types
  //public List<Potion> Potions;
  //public List<Relic> Relics;
  //public List<HealingKit> HealingKits;
  //public List<VisionUpgrade> VisionUpgrades;
  //public List<MaxHealthUpgrade> HealthUpgrades;
  //public Scrapper Scrapper;
}