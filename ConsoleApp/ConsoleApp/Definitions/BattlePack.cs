using System.Collections.Generic;
using System.Linq;
using MaM.Enums;
using MaM.Helpers;

namespace MaM.Definitions;

public class BattlePack
{
  public Combatant player;
  public Combatant enemy;

  public Market market;

  public Stack<Card> deck;

  public Hand hand;

  public List<Card> field;

  public List<Card> graveyard;

  public List<Card> scrapheap;

  private Fight _node;
  private GameContents _gameContents;

  public BattlePack(Fight node, ref GameContents gameContents)
  { 
    _node = node;
    _gameContents = gameContents;

    player = new Combatant(gameContents.player.health);
    enemy = new Combatant(node.enemy.health);

    SetupMarket(node.fightType);

    SetupDeck();

    SetupHand();
    
    SetupField();

    SetupGraveyard();

    SetupScrapheap();
  }

  private void SetupMarket(FightType fightType)
  {
    var playerCardsContribution = _gameContents.player.GetDeck().Where(card => card.guild != Guild.NEUTRAL).ToList();

    var enemyCardsContribution =
      fightType == FightType.BOSS 
        ? _gameContents.cards
        : _gameContents.cards.Where(card => card.guild == _node.guild).ToList();
    
    market = new Market(_node.enemy.marketSize, playerCardsContribution, enemyCardsContribution);

    market.Fill();
  }

  private void SetupDeck()
  {
    var startingCards = _gameContents.player.GetDeck().Where(card => card.guild == Guild.NEUTRAL).ToList();
    startingCards.Shuffle();

    deck = new Stack<Card>(startingCards);
  }

  private void SetupHand()
  {
    hand = new Hand(_gameContents.handSize);
    hand.Draw_Full();
  }
  private void SetupField()
  {
    field = new List<Card>();
  }

  private void SetupGraveyard()
  {
    graveyard = new List<Card>();
  }

  private void SetupScrapheap()
  {
    scrapheap = new List<Card>();
  }

  public void Mulligan()
  {
    var cardsInHand = hand.GetAllCardsInHand();
    foreach (var card in cardsInHand)
    {
      deck.Push(card);
    }

    var shuffledDeck = deck.ToList();
    shuffledDeck.Shuffle();

    hand.Clear();
    hand.Draw_Full();

    market.Cycle();
  }

  public void MoveHandToGraveyard()
  {
    graveyard.AddRange(hand.GetAllCardsInHand());
    hand.Clear();
  }  
  
  public void MoveFieldToGraveyard()
  {
    graveyard.AddRange(field);
    field.Clear();
  }
}