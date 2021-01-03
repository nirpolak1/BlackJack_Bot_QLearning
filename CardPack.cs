using System;
using System.Collections.Generic;
using System.Text;

namespace BlackJackBot.Utilities
{
    /// <summary>
    /// Pack of number of standard card decks.
    /// </summary>
    public class CardPack
    {
        /// <summary>
        /// List of all card objects in the pack.
        /// </summary>
        public List<Card> Cards { get; private set; }

        /// <summary>
        /// Initialize a new card pack.
        /// The pack is filled with the given the number of standard deck.
        /// </summary>
        /// <param name="numberOfStandardPacks"></param>
        public CardPack(int numberOfStandardPacks)
        {
            //properties of standard deck.
            int cardRepetition = 4;
            int cardValues = 13;

            //fill the card pack with card objects.
            this.Cards = new List<Card>();

            for (int i = 0; i < numberOfStandardPacks; i++)
            {
                for (int j = 1; j <= cardValues; j++)
                {
                    for (int k = 0; k < cardRepetition; k++)
                    {
                        this.Cards.Add(new Card(j));
                    }
                }
            }
        }

        /// <summary>
        /// Randomize the order of the cards in the pack.
        /// </summary>
        /// <param name="randomGenerator"></param>
        public void Shuffle(Random randomGenerator)
        {
            int count = this.Cards.Count;

            //for every card, randomly choose another card to replace between them. 
            while (count > 1)
            {
                count--;
                int randomIndex = randomGenerator.Next(count + 1);
                
                Card swapCard = this.Cards[randomIndex];
                this.Cards[randomIndex] = this.Cards[count];
                this.Cards[count] = swapCard;
            }
        }

        /// <summary>
        /// Return the first card object from the pack.
        /// The card is discarded from the pack.
        /// </summary>
        /// <returns></returns>
        public Card DrawCard()
        {
            Card drawnCard = this.Cards[0];
            this.Cards.RemoveAt(0);

            return drawnCard;
        }


    }
}
