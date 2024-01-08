using System.Collections.Generic;

namespace MaM.Definitions;

public class Journey
{
  public Journey()
  {
    maps = new List <Map>();
    currentMapIndex = 0;
  }

  public List<Map> maps;
  public int currentMapIndex;
}