using System;
using System.Collections.Generic;
using System.Linq;

namespace MaM.Definitions;

public class Player
{
  public Player()
  {
  }

  public Player(Player player)
  {
    isComputer              = player.isComputer;
    name                    = player.name;
    currentNodeX            = player.currentNodeX;
    currentNodeY            = player.currentNodeY;
    completedNodeLocations  = player.completedNodeLocations?.ToList();
    completedMapCount       = player.completedMapCount;
    activeGuild             = player.activeGuild;
    health                  = player.health;
    maxHealth               = player.maxHealth;
    coin                    = player.coin;
    vision                  = player.vision;
    awareness               = player.awareness;
    insight                 = player.insight;
    tradeRowSize            = player.tradeRowSize;
    manna                   = player.manna;
    basicManna              = player.basicManna;
    basicHandSize           = player.basicHandSize;
    initiative              = player.initiative;
    currentHandSize         = player.currentHandSize;
    toDiscard               = player.toDiscard;
    deckCardIds             = player.deckCardIds?.ToList();
    deck                    = player.deck?.ToList();
    drawPile                = player.drawPile?.ToList();
    discardPile             = player.discardPile?.ToList();
    tradeRow                = player.tradeRow?.ToList();
    tradePool               = player.tradePool?.ToList();
    cardsInPlay             = player.cardsInPlay?.ToList();
  }

  public Player(
    bool                  isComputer,
    string                name,
    int                   currentNodeX,
    int                   currentNodeY,
    List<Tuple<int, int>> completedNodeLocations,
    int                   completedMapCount,
    Guild                 activeGuild,
    int                   health,
    int                   maxHealth,
    int                   coin,
    int                   vision,
    int                   awareness,
    int                   insight,
    int                   tradeRowSize,
    int                   manna,
    int                   basicManna,
    int                   basicHandSize,
    int                   initiative,
    int                   currentHandSize,
    int                   toDiscard,
    List<string>          deckCardIds,
    List<Card>            deck,
    List<Card>            drawPile,
    List<Card>            discardPile,
    List<Card>            tradeRow,
    List<Card>            tradePool,
    List<Card>            cardsInPlay
  )
  {
    this.isComputer             = isComputer;
    this.name                   = name;
    this.currentNodeX           = currentNodeX;
    this.currentNodeY           = currentNodeY;
    this.completedNodeLocations = completedNodeLocations;
    this.completedMapCount      = completedMapCount;
    this.activeGuild            = activeGuild;
    this.health                 = health;
    this.maxHealth              = maxHealth;
    this.coin                   = coin;
    this.vision                 = vision;
    this.awareness              = awareness;
    this.insight                = insight;
    this.tradeRowSize           = tradeRowSize;
    this.basicManna             = basicManna;
    this.manna                  = manna;
    this.basicHandSize          = basicHandSize;
    this.initiative             = initiative;
    this.currentHandSize        = currentHandSize;
    this.toDiscard              = toDiscard;
    this.deckCardIds            = deckCardIds;
    this.deck                   = deck;
    this.drawPile               = drawPile;
    this.discardPile            = discardPile;
    this.tradeRow               = tradeRow;
    this.tradePool              = tradePool;
    this.cardsInPlay            = cardsInPlay;
  }

  public bool                   isComputer;
  public string                 name;
  public int                    currentNodeX;
  public int                    currentNodeY;
  public List<Tuple<int, int>>  completedNodeLocations;
  public int                    completedMapCount;
  public Guild                  activeGuild;
  public int                    health;
  public int                    maxHealth;
  public int                    coin;
  public int                    vision;       //How many nodes and paths ahead you can simply see
  public int                    awareness;    //How many of the visible nodes ahead have their types revealed
  public int                    insight;      //How many of the nodes ahead have their Guild distributions revealed
  public int                    tradeRowSize;
  public int                    basicManna;
  public int                    manna;
  public int                    toDiscard;
  public int                    basicHandSize;
  public int                    initiative;
  public int                    currentHandSize;
  public List<string>           deckCardIds;
  public List<Card>             deck;
  public List<Card>             drawPile;
  public List<Card>             discardPile;
  public List<Card>             tradeRow;
  public List<Card>             tradePool;
  public List<Card>             cardsInPlay;
}