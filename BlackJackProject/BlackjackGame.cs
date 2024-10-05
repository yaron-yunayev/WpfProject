using System;
using System.Collections.Generic;

namespace BlackjackGame
{
    public class BlackjackGame
    {
        public List<Card> Deck { get; set; }
        public List<Card> PlayerHand { get; set; }
        public List<Card> DealerHand { get; set; }

        public BlackjackGame()
        {
            Deck = new List<Card>();
            PlayerHand = new List<Card>();
            DealerHand = new List<Card>();
            InitializeDeck();
        }

        private void InitializeDeck()
        {
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King", "Ace" };

            foreach (string suit in suits)
            {
                foreach (string rank in ranks)
                {
                    int value = rank switch
                    {
                        "Ace" => 11,
                        "King" or "Queen" or "Jack" => 10,
                        _ => int.Parse(GetRankValue(rank))
                    };

                    string imageName = $"{rank}{GetSuitPrefix(suit)}";
                    Deck.Add(new Card(suit, rank, value, imageName)); // לא צריך לשנות כאן

                }
            }
        }


        private string GetRankValue(string rank)
        {
            return rank switch
            {
                "Two" => "2",
                "Three" => "3",
                "Four" => "4",
                "Five" => "5",
                "Six" => "6",
                "Seven" => "7",
                "Eight" => "8",
                "Nine" => "9",
                "Ten" => "10",
                _ => throw new ArgumentException("Invalid rank")
            };
        }

        private string GetSuitPrefix(string suit)
        {
            return suit switch
            {
                "Hearts" => "Red",
                "Diamonds" => "Diamond",
                "Clubs" => "Black",
                "Spades" => "Spad",
                _ => throw new ArgumentException("Invalid suit")
            };
        }


        public void DealCard(bool toPlayer)
        {
            var random = new Random();
            int index = random.Next(Deck.Count);
            Card card = Deck[index];
            Deck.RemoveAt(index);

            if (toPlayer)
            {
                PlayerHand.Add(card);
            }
            else
            {
                DealerHand.Add(card);
            }
        }

        public int CalculateScore(List<Card> hand)
        {
            int score = 0;
            int aceCount = 0;

            foreach (Card card in hand)
            {
                score += card.Value;
                if (card.Rank == "Ace")
                {
                    aceCount++;
                }
            }

            while (score > 21 && aceCount > 0)
            {
                score -= 10;
                aceCount--;
            }

            return score;
        }

        public void PlayerHit()
        {
            DealCard(true);
        }

        public void PlayerStand()
        {
            while (CalculateScore(DealerHand) < 17)
            {
                DealCard(false);
            }
        }
    }
}
