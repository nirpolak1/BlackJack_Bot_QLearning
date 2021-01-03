using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackBot.Utilities
{
    /// <summary>
    /// Group of cards in the hand of either the dealer or the player.
    /// </summary>
    public class Hand
    {
        /// <summary>
        /// List of all cards in the hand.
        /// </summary>
        public List<Card> Cards { get; private set; }

        /// <summary>
        /// Sign if the hand contains an "ace" which allows to soften the hand (reducing 10 from hand value).
        /// </summary>
        public bool IsSoft { get; private set; }

        /// <summary>
        /// Sum of all cards' values in the hand.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// The value of the first card in the dealer hand, which is the only one visible for the player during the game.
        /// </summary>
        public int DealerValue { get; private set; }

        /// <summary>
        /// Initialize a new hand.
        /// Empty of cards, its initial value is zero and it starts in a "hard" state. 
        /// </summary>
        public Hand()
        {
            this.Cards = new List<Card>();
            this.IsSoft = false;
            this.Value = 0;
        }

        /// <summary>
        /// Draw a card from given pack and add it to the hand.
        /// </summary>
        /// <param name="drawnCard"></param>
        public void AddCard(Card drawnCard)
        {
            this.Cards.Add(drawnCard);
            this.Value += drawnCard.BlackJackValue;

            //dealer value is the value of the first card.
            if (this.Cards.Count == 1)
            {
                this.DealerValue = this.Value;
            }

            //when an "ace" is drawn, the hand becomes "soft".
            if (drawnCard.Rank == "Ace" && this.IsSoft == false)
            {
                this.IsSoft = true;
            }
        }

        /// <summary>
        /// Reduce 10 from the value of the hand.
        /// (Turning an ace from 11 to 1)
        /// </summary>
        public void SoftenValue()
        {
            this.Value -= 10;
        }

        /// <summary>
        /// Show the real value of the dealer hand when the player is done playing.
        /// </summary>
        public void DealerEndGameValue()
        {
            this.DealerValue = this.Value;
        }
    }
}
