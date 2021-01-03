using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackBot.Utilities
{
    /// <summary>
    /// The entity that represents that house the player bets against.
    /// The dealer's actions are predetermined. The dealer hit on soft 17.
    /// </summary>
    public class Dealer
    {
        /// <summary>
        /// The group of cards in the player hand.
        /// </summary>
        public Hand DealerHand { get; private set; }

        /// <summary>
        /// Initialze a new dealer, with his own hand object.
        /// </summary>
        public Dealer()
        {
            this.DealerHand = new Hand();
        }

        /// <summary>
        /// Predetermined actions the dealer must take when the player is done.
        /// </summary>
        /// <param name="pack"></param>
        public void Play(CardPack pack)
        {
            //draw card until reaching 17 or over.
            while (this.DealerHand.Value < 17)
            {
                this.DealerHand.AddCard(pack.DrawCard());
            }
            //hit if at soft 17, soften hand if hand is soft and over blackjack (21).
            if (this.DealerHand.IsSoft == true)
            {
                if (this.DealerHand.Value == 17)
                {
                    this.DealerHand.AddCard(pack.DrawCard());
                }
                if (this.DealerHand.Value > 21)
                {
                    this.DealerHand.SoftenValue();
                }
            }

            //reveal true hand value in order to determine game result.
            this.DealerHand.DealerEndGameValue();
        }


    }
}
