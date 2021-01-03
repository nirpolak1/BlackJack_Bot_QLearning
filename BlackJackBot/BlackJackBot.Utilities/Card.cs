using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackBot.Utilities
{
    /// <summary>
    /// Single card with a rank (name) and number.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Original standard value of the card.
        /// </summary>
        public int Value { get; private set; }

        /// <summary>
        /// The value of the card in the context of blackjack.
        /// </summary>
        public int BlackJackValue { get; private set; }

        /// <summary>
        /// Standard name of the card.
        /// </summary>
        public string Rank { get; private set; }

        /// <summary>
        /// Initialize a new card.
        /// Given its value, the card also gets its rank and value in the context of blackjack.
        /// </summary>
        /// <param name="value"></param>
        public Card(int value)
        {
            if (value == 1)
            {
                this.Rank = "Ace";
                this.BlackJackValue = 11;
            }
            else if (value > 1 && value <= 10)
            {
                this.Rank = value.ToString();
                this.BlackJackValue = value;
            }
            else if (value == 11)
            {
                this.Rank = "Jack";
                this.BlackJackValue = 10;
            }
            else if (value == 12)
            {
                this.Rank = "Queen";
                this.BlackJackValue = 10;
            }
            else if (value == 13)
            {
                this.Rank = "King";
                this.BlackJackValue = 10;
            }
        }
    }
}
