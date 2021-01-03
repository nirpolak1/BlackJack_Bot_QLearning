using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BlackJackBot.Utilities
{
    /// <summary>
    /// Single game of blackjack with the following steps:
    /// 1. 2 cards are drawn to both the player and the dealer. dealer shows only first card.
    /// 2. The player takes an actions. he can hit and draw as many cards as he likes until he stands. 
    ///     The player can only double on inital hand then draw one card.
    /// 3. The dealer makes his predetermined actions and reveals his full hand.
    /// 4. A winner is determined based on hands' values.
    /// </summary>
    public class Game
    {
        /// <summary>
        /// Dealer object. Single in each game.
        /// </summary>
        public Dealer DealerBot { get; private set; }

        /// <summary>
        /// Player object. Single in each game.
        /// </summary>
        public Player PlayerBot { get; private set; }

        /// <summary>
        /// Full pack of cards to serve the game.
        /// </summary>
        public CardPack Deck { get; private set; }

        /// <summary>
        /// Initialize a new game.
        /// Create a 6 deck pack and shuffle it.
        /// Draw 2 cards to both the dealer and the player.
        /// </summary>
        /// <param name="randomGenerator"></param>
        public Game(Random randomGenerator) 
        {
            this.Deck = new CardPack(6);
            for (int shuffles = 0; shuffles < 20; shuffles++)
            {
                this.Deck.Shuffle(randomGenerator);
            }

            this.DealerBot = new Dealer();
            this.PlayerBot = new Player();

            for (int initialDraw = 0; initialDraw < 2; initialDraw++)
            {
                this.DealerBot.DealerHand.AddCard(this.Deck.DrawCard());
                this.PlayerBot.PlayerHand.AddCard(this.Deck.DrawCard());
            }
        }

        /// <summary>
        /// Continue the game: have the player take his action.
        /// When the player stands or doubles, the dealer plays and the winner is determined.
        /// In each state of the game update the quality value of the action taken to reach this state.
        /// When "print" is true, the steps are written in the console.
        /// </summary>
        /// <param name="qualityTable"></param>
        /// <param name="randomGenerator"></param>
        /// <param name="print"></param>
        public void Start(QualityTable qualityTable, Random randomGenerator, bool print)
        {
            bool gameStatus = true; //is the player done?
            bool doublePossible = true; //is the player already hit?
            double rewardDouble = 1.0; //doubles when the player choose to DOUBLE!
            
            //sample the current state of the game.
            State currentState = qualityTable.GetState(this.PlayerBot, this.DealerBot);
            State newState;

            if (print)
            {
                Console.WriteLine("Game Started");
                Console.WriteLine("Dealer card: " + DealerBot.DealerHand.Cards.First().Rank);
                Console.WriteLine("Player hand: " + PlayerBot.PlayerHand.Cards.First().Rank + " and " + PlayerBot.PlayerHand.Cards.Last().Rank);
            }

            //have the player play until he stands or doubles.
            while (gameStatus)
            {
                //player decide what action to take
                int action = -1;
                Random explorationPath = randomGenerator;
                if (explorationPath.NextDouble() < qualityTable.Epsilon)
                {
                    action = explorationPath.Next(3);
                }
                else
                {
                    action = currentState.ActionQuality.IndexOf(currentState.ActionQuality.Max());
                }

                //sample current state
                currentState = qualityTable.GetState(this.PlayerBot, this.DealerBot);

                //perform action
                //stand!
                if (action == 0)
                {
                    if (print)
                    {
                        Console.WriteLine("Player stands!");
                    }

                    PlayerBot.Stand();
                    DealerBot.Play(this.Deck);
                    gameStatus = false;
                }
                //hit!
                else if (action == 1)
                {
                    if (print)
                    {
                        Console.WriteLine("Player hit!");
                    }

                    doublePossible = false;
                    PlayerBot.Hit(this.Deck);
                }
                //double!
                else if (action == 2 && doublePossible)
                {
                    if (print)
                    {
                        Console.WriteLine("Player doubles!");
                    }

                    PlayerBot.Hit(this.Deck);
                    rewardDouble = 2.0;
                    DealerBot.Play(this.Deck);
                    gameStatus = false;
                }

                double stageReward;
                //update new state and take reward.
                newState = qualityTable.GetState(this.PlayerBot, this.DealerBot);

                //punish the player if he tries to double when not allowed.
                if (action == 2 && !doublePossible)
                {
                    gameStatus = false;
                    stageReward = -10;
                    if (print)
                    {
                        Console.WriteLine("Player made an illegal move");
                    }
                }
                //multiply the reward when game is finished.
                else
                {
                    
                    if (!gameStatus)
                    {
                        rewardDouble *= 10.0;
                    }
                    stageReward = newState.Reward * rewardDouble;
                }
                

                //update q table
                qualityTable.UpdateQuality(currentState, newState, action, stageReward);

                currentState = newState;

                if (print && !gameStatus)
                {
                    Console.WriteLine("Final Hands:");
                    Console.Write("Dealer: ");
                    DealerBot.DealerHand.Cards.ForEach(o => Console.Write(o.Rank + " "));
                    Console.Write(System.Environment.NewLine + "Player: ");
                    PlayerBot.PlayerHand.Cards.ForEach(o => Console.Write(o.Rank + " "));
                    Console.WriteLine(" ");

                    if (currentState.Reward > 0)
                    {
                        Console.WriteLine("Player Wins!");
                    }
                    if (currentState.Reward == 0)
                    {
                        Console.WriteLine("Tie!");
                    }
                    if (currentState.Reward < 0)
                    {
                        Console.WriteLine("Player Loses!");
                    }
                }
            }

            
        }
    }
}
