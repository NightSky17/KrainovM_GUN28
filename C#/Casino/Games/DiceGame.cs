using Final_Task.Dices;

namespace Final_Task.Games
{
    public class DiceGame : CasinoGameBase
    {
        private readonly int _numberOfDice;
        private readonly int _minValue;
        private readonly int _maxValue;
        private List<Dice> _diceCollection;

        // Конструктор, принимающий количество костей, минимальное и максимальное значения
        public DiceGame(int numberOfDice, int minValue, int maxValue)
        {
            if (numberOfDice <= 0)
                throw new ArgumentException("The number of dice must be greater than zero.", nameof(numberOfDice));

            _numberOfDice = numberOfDice;
            _minValue = minValue;
            _maxValue = maxValue;

            FactoryMethod(); // Вызываем FactoryMethod для инициализации костей
        }

        // Реализация FactoryMethod для создания костей
        protected override void FactoryMethod()
        {
            _diceCollection = new List<Dice>();
            for (int i = 0; i < _numberOfDice; i++)
            {
                _diceCollection.Add(new Dice(_minValue, _maxValue));
            }
        }

        // Основной метод игры
        public override void PlayGame()
        {
            Console.WriteLine("Starting the dice game!");

            int playerScore = RollDice();
            int computerScore = RollDice();

            Console.WriteLine($"Your score: {playerScore}");
            Console.WriteLine($"Computer's score: {computerScore}");

            // Логика определения победителя
            if (playerScore > computerScore)
            {
                Console.WriteLine("You win! You have more points.");
                OnWinInvoke();
            }
            else if (computerScore > playerScore)
            {
                Console.WriteLine("You lose! The computer has more points.");
                OnLooseInvoke();
            }
            else
            {
                Console.WriteLine("It's a draw! The scores are equal.");
                OnDrawInvoke();
            }
        }

        // Бросок костей
        private int RollDice()
        {
            int total = 0;
            foreach (var dice in _diceCollection)
            {
                total += dice.Number;
            }
            return total;
        }
    }
}
