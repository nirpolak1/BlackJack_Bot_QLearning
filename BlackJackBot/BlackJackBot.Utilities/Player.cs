using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackBot.Utilities
{
    /// <summary>
    /// The entity that bets against the dealer.
    /// The strategy table will be updated using the player's actions 
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The group of cards in the player hand.
        /// </summary>
        public Hand PlayerHand { get; private set; }

        /// <summary>
        /// Initialze a new player, with his own hand object.
        /// </summary>
        public Player()
        {
            this.PlayerHand = new Hand();
        }

        /// <summary>
        /// HIT! action - draw a card and add it to the player hand.
        /// When the player chooses to DOUBLE!, he hit.
        /// </summary>
        /// <param name="pack"></param>
        public void Hit(CardPack pack)
        {
            this.PlayerHand.AddCard(pack.DrawCard());
        }

        /// <summary>
        /// STAND! action - don't draw any more cards.
        /// If the player's hand is soft and he reached over blackjack (21), the hand is soften. 
        /// </summary>
        public void Stand()
        {
            if (this.PlayerHand.IsSoft == true && this.PlayerHand.Value > 21)
            {
                this.PlayerHand.SoftenValue();
            }
        }

    }
}
