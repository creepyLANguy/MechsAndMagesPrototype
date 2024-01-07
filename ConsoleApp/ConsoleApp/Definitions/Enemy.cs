using System.Collections.Generic;
using System.Linq;

namespace MaM.Definitions;

public class Enemy : Player
{
  public Enemy()
  {
  }

  public Enemy(Player enemy) : base(enemy)
  {
  }

  public Enemy(
    string      name,
    int         maxHealth, 
    int         tradeRowSize,
    int         basicManna, 
    int         basicHandSize,
    int         initiative,
    List<Card>  deck
  ) 
    : base(
      true, 
      name, 
      -1,
      -1, 
      null,
      -1,
      null, 
      maxHealth, 
      maxHealth, 
      -1, 
      -1, 
      -1, 
      -1, 
      tradeRowSize,
      -1, 
      basicManna, 
      basicHandSize, 
      initiative,
      -1, 
      -1,
      deck.Select(card => card.id).ToList(), 
      deck, 
      null, 
      null, 
      null, 
      null,
      null
    )
  {
  }
}