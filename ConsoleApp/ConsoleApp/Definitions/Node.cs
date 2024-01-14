using System;
using System.Collections.Generic;
using MaM.Enums;

namespace MaM.Definitions;

public class Node
{
  public Node(Node node)
  {
    nodeType      = node.nodeType;
    isMystery     = node.isMystery;
    x             = node.x;            
    y             = node.y;            
    isComplete    = node.isComplete;
    isDestination = node.isDestination;
    destinations  = node.destinations;
  }

  public Node(
    NodeType                  nodeType,
    bool                      isMystery,
    int                       x,
    int                       y,
    bool                      isComplete,
    bool                      isDestination,
    HashSet<Tuple<int, int>>  destinations
  )
  {
    this.nodeType      = nodeType;
    this.isMystery     = isMystery;
    this.x             = x;
    this.y             = y;
    this.isComplete    = isComplete;
    this.isDestination = isDestination;
    this.destinations  = destinations;
  }

  public NodeType                 nodeType;
  public bool                     isMystery;
  public int                      x;
  public int                      y;
  public bool                     isComplete;    
  public bool                     isDestination;
  public HashSet<Tuple<int, int>> destinations;
}