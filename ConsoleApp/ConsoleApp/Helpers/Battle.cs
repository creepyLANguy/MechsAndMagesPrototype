using System;
using System.Collections.Generic;
using MaM.Definitions;

namespace MaM.Helpers;

public static class Battle
{ 
  [ThreadStatic]
  private static BattlePack _battlePack;

  public static Combatant Player => _battlePack.player;

  public static Combatant Enemy => _battlePack.enemy;

  public static Market Market => _battlePack.market;

  public static Stack<Card> Deck => _battlePack.deck;

  public static Hand Hand => _battlePack.hand;

  public static List<Card> Field => _battlePack.field;

  public static List<Card> Graveyard => _battlePack.graveyard;

  public static List<Card> Scrapheap => _battlePack.scrapheap;

  public static void Mulligan() => _battlePack.Mulligan();

  public static void MoveHandToGraveyard() => _battlePack.MoveHandToGraveyard();

  public static void MoveFieldToGraveyard() => _battlePack.MoveFieldToGraveyard();

  public static void Start(Fight node, ref GameContents gameContents)
  {
    _battlePack = new BattlePack(node, ref gameContents);
  }
  
  public static void End()
  {
    _battlePack = null;
  }
}