using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackJackBot.Utilities;

namespace BlackJackBot
{
    class Program
    {
        static void Main(string[] args)
        {
            //create new quality table to represent the strategy chart.
            QualityTable qualityTable = new QualityTable();

            //create new randomize generator to sample random values. 
            Random randomGenerator = new Random();

            //number of games to play in order to train the quality table.
            int epochNumber = 100000000;

            //play the number of games.
            //print the resulted strategy chart from time to time.
            for (int epoch = 0; epoch < epochNumber; epoch++)
            {
                Game GameBJ = new Game(randomGenerator);
                GameBJ.Start(qualityTable, randomGenerator, false);

                if (epoch % 10000 == 0)
                {
                    qualityTable.PrintResultTable();
                }
            }

            qualityTable.PrintResultTable();

            Console.ReadLine();
        }
    }
}
