using System.Collections.Generic;

namespace MaM.Definitions;

public class EnemyNames
{
  public EnemyNames(List<List<string>> blob)
  {
    var i = -1;

    pre = blob[++i];

    neutralDescriptors = blob[++i];
    borgDescriptors    = blob[++i];
    mechDescriptors    = blob[++i];
    mageDescriptors    = blob[++i];
    necroDescriptors   = blob[++i];

    collective         = blob[++i];

    post               = blob[++i];

    place              = blob[++i];

    allLists = new List<List<string>>
    {
      pre, 
      neutralDescriptors, 
      borgDescriptors, 
      mechDescriptors, 
      mageDescriptors, 
      necroDescriptors, 
      collective, 
      post,
      place
    };
  }

  public readonly List<string> pre;

  public readonly List<string> neutralDescriptors;
  public readonly List<string> borgDescriptors;
  public readonly List<string> mechDescriptors;
  public readonly List<string> mageDescriptors;
  public readonly List<string> necroDescriptors;

  public readonly List<string> collective;

  public readonly List<string> post;

  public readonly List<string> place;

  public readonly List<List<string>> allLists;
}