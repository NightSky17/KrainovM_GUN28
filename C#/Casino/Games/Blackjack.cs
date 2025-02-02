using Final_Task.Cards;

namespace Final_Task.Games
{
    public class Blackjack : CasinoGameBase
    {
        private readonly int _numberOfCards;
        private Queue<Card> _deck;

        // Конструктор, принимающий количество карт
        public Blackjack(int numberOfCards)
        {
            if (numberOfCards <= 0)
                throw new ArgumentException("Number of cards must be greater than zero.", nameof(numberOfCards));

            _numberOfCards = numberOfCards;
            FactoryMethod(); // Вызываем FactoryMethod для инициализации колоды
        }

        // Реализация FactoryMethod для создания колоды карт
        protected override void FactoryMethod()
        {
            var cards = new List<Card>();
            foreach (Suit suit in Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    cards.Add(new Card(suit, rank));
                }
            }

            // Если количество карт больше, чем в стандартной колоде, добавляем дополнительные карты
            while (cards.Count < _numberOfCards)
            {
                foreach (Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    foreach (Rank rank in Enum.GetValues(typeof(Rank)))
                    {
                        if (cards.Count >= _numberOfCards) break;
                        cards.Add(new Card(suit, rank));
                    }
                    if (cards.Count >= _numberOfCards) break;
                }
            }

            _deck = new Queue<Card>(cards);
            Shuffle(); // Перетасовываем колоду
        }

        // Перетасовка карт
        private void Shuffle()
        {
            var rnd = new Random();
            _deck = new Queue<Card>(_deck.OrderBy(x => rnd.Next()));
        }

        // Основной метод игры
        public override void PlayGame()
        {
            Console.WriteLine("Starting Blackjack game!");

            var playerCards = new List<Card> { DrawCard(), DrawCard() };
            var computerCards = new List<Card> { DrawCard(), DrawCard() };

            Console.WriteLine($"Your cards: {string.Join(", ", playerCards)}");
            Console.WriteLine($"Computer's cards: {string.Join(", ", computerCards)}");

            int playerScore = CalculateScore(playerCards);
            int computerScore = CalculateScore(computerCards);

            Console.WriteLine($"Your score: {playerScore}");
            Console.WriteLine($"Computer's score: {computerScore}");

            // Логика определения победителя
            if (playerScore == computerScore)
            {
                Console.WriteLine("It's a tie! Drawing one more card for each player.");
                playerCards.Add(DrawCard());
                computerCards.Add(DrawCard());

                playerScore = CalculateScore(playerCards);
                computerScore = CalculateScore(computerCards);

                Console.WriteLine($"Your new cards: {string.Join(", ", playerCards)}");
                Console.WriteLine($"Computer's new cards: {string.Join(", ", computerCards)}");
                Console.WriteLine($"Your score: {playerScore}");
                Console.WriteLine($"Computer's score: {computerScore}");
            }

            if (playerScore > 21 && computerScore > 21)
            {
                Console.WriteLine("Both players lost! It's a tie.");
                OnDrawInvoke();
            }
            else if (playerScore > 21)
            {
                Console.WriteLine("You lost! Your score is over 21.");
                OnLooseInvoke();
            }
            else if (computerScore > 21)
            {
                Console.WriteLine("You won! Computer's score is over 21.");
                OnWinInvoke();
            }
            else if (playerScore > computerScore)
            {
                Console.WriteLine("You won! Your score is higher.");
                OnWinInvoke();
            }
            else if (computerScore > playerScore)
            {
                Console.WriteLine("You lost! Computer's score is higher.");
                OnLooseInvoke();
            }
            else
            {
                Console.WriteLine("It's a tie! Scores are equal.");
                OnDrawInvoke();
            }
        }

        // Взятие карты из колоды
        private Card DrawCard()
        {
            if (_deck.Count == 0)
                throw new InvalidOperationException("The deck is empty!");

            return _deck.Dequeue();
        }

        // Подсчёт очков
        private int CalculateScore(List<Card> cards)
        {
            int score = cards.Sum(card => GetCardValue(card.Rank));
            int aceCount = cards.Count(card => card.Rank == Rank.Ace);

            // Корректировка очков для тузов
            while (score > 21 && aceCount > 0)
            {
                score -= 10;
                aceCount--;
            }

            return score;
        }

        // Получение значения карты
        private int GetCardValue(Rank rank)
        {
            return rank switch
            {
                Rank.Six => 6,
                Rank.Seven => 7,
                Rank.Eight => 8,
                Rank.Nine => 9,
                Rank.Ten => 10,
                Rank.Jack => 10,
                Rank.Queen => 10,
                Rank.King => 10,
                Rank.Ace => 11,
                _ => throw new ArgumentException("Unknown card rank.")
            };
        }
    }
}
