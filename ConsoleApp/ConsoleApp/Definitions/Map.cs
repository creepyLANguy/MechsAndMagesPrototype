namespace MaM.Definitions;

public struct Map
{
  public Map(int index, int width, int height)
  {
    this.index  = index;
    this.width  = width;
    this.height = height;
    nodes       = new Node[width, height];
  }

  public int      index;
  public Node[,]  nodes;
  public int      width;
  public int      height;
}