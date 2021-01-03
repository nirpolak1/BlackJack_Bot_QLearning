using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace BlackJackBot.Utilities
{
    /// <summary>
    /// Container of game states, and methods to update action quality values through Q-Learning.
    /// </summary>
    public class QualityTable
    {
        /// <summary>
        /// List of all game states.
        /// Each state contains the rewards and the quality of actions to be taken in this state.
        /// </summary>
        public List<State> StateList { get; private set; }

        /// <summary>
        /// The propability in which the player would take a random action instead of relying on the qualities of the actions.
        /// </summary>
        public double Epsilon { get; private set; }

        /// <summary>
        /// Learning rate.
        /// (The weight of the new quality value when it's averaged with the current value)
        /// </summary>
        public double Alpha { get; private set; }

        /// <summary>
        /// Discount factor.
        /// (The weight of future rewards, how much the player "looks forward")
        /// </summary>
        public double Gamma { get; private set; }

        /// <summary>
        /// Initialize a new quality table which contains all game stats.
        /// </summary>
        public QualityTable()
        {
            this.Epsilon = 0.2; 
            this.Alpha = 0.1; 
            this.Gamma = 0.1;

            this.StateList = new List<State>();

            //fill state list for hard hands
            for (int playerValue = 4; playerValue <= 22; playerValue++)
            {
                for (int dealerValue = 2; dealerValue <= 22; dealerValue++)
                {
                    StateList.Add(new State(playerValue, dealerValue, false));
                }
            }

            //fill state list for soft hands
            for (int playerValue = 12; playerValue <= 22; playerValue++)
            {
                for (int dealerValue = 2; dealerValue <= 22; dealerValue++)
                {
                    StateList.Add(new State(playerValue, dealerValue, true));
                }
            }
        }

        /// <summary>
        /// Given a previous state, a new state, the action taken, and the final reward given - update the quality value of the taken action in the previous state.
        /// </summary>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <param name="action"></param>
        /// <param name="reward"></param>
        public void UpdateQuality(State oldState, State newState, int action, double reward)
        {
            //max future expected reward.
            double maxActionQuality = newState.ActionQuality.Max();

            //q learning formula to calculate new quality value.
            double newValue = (1 - Alpha) * oldState.ActionQuality[action] + (reward + (Gamma * maxActionQuality)) * Alpha;

            //update the previous value in the state.
            oldState.UpdateQuality(action, newValue);
        }

        /// <summary>
        /// Return the state of the game, based on the revealed hands.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="dealer"></param>
        /// <returns></returns>
        public State GetState(Player player, Dealer dealer)
        {
            return this.StateList.Where(o => o.PlayerValue == Math.Min(22, player.PlayerHand.Value) &&
            o.DealerValue == Math.Min(22, dealer.DealerHand.DealerValue) &&
            o.IsSoft == player.PlayerHand.IsSoft).First();
        }

        /// <summary>
        /// Print a strategy chart for the main states, according to the quality of actions in each state.
        /// </summary>
        public void PrintResultTable()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine("-----------------------");
            Console.WriteLine("Strategy for hard hand:");
            Console.WriteLine("-----------------------");
            Console.WriteLine("Player Totals /                    Dealer Totals:");
            Console.Write("_____________/  ");

            for (int dealerValue = 2; dealerValue <= 11; dealerValue++)
            {
                Console.Write($"{dealerValue,4}", dealerValue);
            }

            for (int playerValue = 4; playerValue <= 21; playerValue++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(System.Environment.NewLine);
                Console.Write($"{playerValue,16}", playerValue);
                for (int dealerValue = 2; dealerValue <= 11; dealerValue++)
                {
                    State thisState = this.StateList.Where(o => o.PlayerValue == playerValue &&
                    o.DealerValue == dealerValue &&
                    o.IsSoft == false).First();

                    int strategy = thisState.ActionQuality.IndexOf(thisState.ActionQuality.Max());

                    if (strategy == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write($"{"S",4}");
                    }
                    else if (strategy == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write($"{"H",4}");
                    }
                    else if (strategy == 2)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.Write($"{"D",4}");
                    }
                }
            }

            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(System.Environment.NewLine);
            Console.WriteLine("-----------------------");
            Console.WriteLine("Strategy for soft hand:");
            Console.WriteLine("-----------------------");
            Console.WriteLine("Player Totals /                    Dealer Totals:");
            Console.Write("_____________/  ");

            for (int dealerValue = 2; dealerValue <= 11; dealerValue++)
            {
                Console.Write($"{dealerValue,4}", dealerValue);
            }

            for (int playerValue = 13; playerValue <= 21; playerValue++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Write(System.Environment.NewLine);
                Console.Write($"{playerValue,16}", playerValue);
                for (int dealerValue = 2; dealerValue <= 11; dealerValue++)
                {
                    State thisState = this.StateList.Where(o => o.PlayerValue == playerValue &&
                    o.DealerValue == dealerValue &&
                    o.IsSoft == true).First();

                    int strategy = thisState.ActionQuality.IndexOf(thisState.ActionQuality.Max());

                    if (strategy == 0)
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                        Console.Write($"{"S",4}");
                    }
                    else if (strategy == 1)
                    {
                        Console.BackgroundColor = ConsoleColor.Green;
                        Console.Write($"{"H",4}");
                    }
                    else if (strategy == 2)
                    {
                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.Write($"{"D",4}");
                    }
                }
            }

            Console.Write(System.Environment.NewLine);
        }
    }

}
