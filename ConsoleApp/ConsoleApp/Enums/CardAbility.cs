namespace MaM.Enums;

public enum CardAbility
{ 
  NONE,
  DRAW,//Draw a single card from deck into hand
  HEAL,//Increase player health by +1
  STOMP,//Exile a random card from field or hand (no choice)
  CYCLE,//Cycle the trade row
  SHUN//Exile a card from your hand (player chooses which) and draw a card
}