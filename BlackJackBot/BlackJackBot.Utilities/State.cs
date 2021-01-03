using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackBot.Utilities
{
    /// <summary>
    /// Single possible state of the game.
    /// Each state has its own reward and action quality to determine which should the player take.
    /// </summary>
    public class State
    {
        /// <summary>
        /// Sum of values of cards in the hand of the player.
        /// </summary>
        public int PlayerValue { get; private set; }

        /// <summary>
        /// Sum of values of revealed cards in the hand of the dealer.
        /// </summary>
        public int DealerValue { get; private set; }

        /// <summary>
        /// Amount of reward the player would get if this state was the final in the game.
        /// </summary>
        public double Reward { get; private set; }

        /// <summary>
        /// List of action quality values.
        /// 3 actions are available: STAND! HIT! DOUBLE!
        /// The value are listed in the same order.
        /// </summary>
        public List<double> ActionQuality { get; private set; }

        /// <summary>
        /// Sign if the player has an "ace" in his hand, which might affect his decision. 
        /// </summary>
        public bool IsSoft { get; private set; }

        /// <summary>
        /// Initialize a new game stat.
        /// The values of the hands determine whether the player won and thus the reward.
        /// </summary>
        /// <param name="playerValue"></param>
        /// <param name="dealerValue"></param>
        /// <param name="isSoft"></param>
        public State(int playerValue, int dealerValue, bool isSoft)
        {
            int blackJack = 21;

            if ((dealerValue > playerValue && dealerValue <= blackJack) || (playerValue > blackJack))
            {
                //lose
                this.Reward = -1.0;
            }
            else if ((dealerValue > blackJack && playerValue <= blackJack) || (dealerValue < playerValue && playerValue <= blackJack))
            {
                //win
                this.Reward = 2.0;
            }
            else if (dealerValue == playerValue && playerValue <= blackJack)
            {
                //tie
                this.Reward = 0.0;
            }
            else if (dealerValue != playerValue && playerValue == blackJack)
            {
                //player hit blackjack!
                this.Reward = 3.0;
            }

            this.DealerValue = dealerValue;
            this.PlayerValue = playerValue;
            this.IsSoft = isSoft;

            //three actions can be taken: stand / hit / double.
            this.ActionQuality = new List<double> { 0, 0, 0 };
        }

        /// <summary>
        /// Change the quality value of an action, as it was solved in the QualityTable.
        /// </summary>
        /// <param name="action"></param>
        /// <param name="newValue"></param>
        public void UpdateQuality(int action, double newValue)
        {
            this.ActionQuality[action] = newValue;
        }
    }
}
