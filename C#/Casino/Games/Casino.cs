using Final_Task.Services;

namespace Final_Task.Games
{
    public class Casino : IGame
    {
        private PlayerProfile _playerProfile;
        private readonly List<CasinoGameBase> _games;
        private readonly ISaveLoadService<string> _saveLoadService;
        private const int MaxBankValue = 1000000; // Максимальное значение банка

        // Конструктор
        public Casino()
        {
            // Инициализация игр
            _games = new List<CasinoGameBase>
        {
            new Blackjack(52), // Блэкджек с 52 картами
            new DiceGame(3, 1, 6) // Игра в кости с 3 костями и значениями от 1 до 6
        };

            // Инициализация сервиса сохранения и загрузки
            _saveLoadService = new FileSystemSaveLoadService("Profiles");

            // Загрузка или создание профиля игрока
            LoadOrCreatePlayerProfile();
        }

        // Загрузка или создание профиля игрока
        private void LoadOrCreatePlayerProfile()
        {
            string name;
            while (true)
            {
                Console.WriteLine("Enter your name:");
                name = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(name))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Name cannot be empty. Please enter a name again.");
                }
            }

            try
            {
                string profileData = _saveLoadService.LoadData(name);
                _playerProfile = PlayerProfile.FromString(profileData);
                Console.WriteLine($"Welcome, {_playerProfile.Name}! Your current bank: {_playerProfile.Bank}");
            }
            catch (InvalidOperationException)
            {
                _playerProfile = new PlayerProfile(name, 1000);
                Console.WriteLine($"A new profile has been created. Welcome, {_playerProfile.Name}! Your starting bank: {_playerProfile.Bank}");
            }
        }

        // Реализация метода StartGame из интерфейса IGame
        public void StartGame()
        {
            while (true)
            {
                if (_playerProfile.Bank <= 0)
                {
                    Console.WriteLine("No money? Kicked!");
                    break;
                }

                Console.WriteLine("Choose a game:");
                Console.WriteLine("1 - Blackjack");
                Console.WriteLine("2 - Dice game");
                Console.WriteLine("0 - Exit");

                string choice = Console.ReadLine();

                if (choice == "0")
                {
                    Console.WriteLine("Thanks for playing! Goodbye!");
                    SavePlayerProfile();
                    break;
                }

                if (int.TryParse(choice, out int gameIndex) && gameIndex > 0 && gameIndex <= _games.Count)
                {
                    PlaySelectedGame(gameIndex - 1);
                }
                else
                {
                    Console.WriteLine("Invalid choice. Try again.");
                }
            }
        }

        // Запуск выбранной игры
        private void PlaySelectedGame(int gameIndex)
        {
            var selectedGame = _games[gameIndex];

            Console.WriteLine($"Your current bank: {_playerProfile.Bank}");
            Console.WriteLine("Place your bet:");
            int bet = GetValidBet();

            // Подписка на события игры
            selectedGame.OnWin += () =>
            {
                _playerProfile.AddWin();
                _playerProfile.Bank += bet;
                CheckBankLimit();
                Console.WriteLine($"You won! Your bank: {_playerProfile.Bank}");
            };
            selectedGame.OnLoose += () =>
            {
                _playerProfile.AddLoss();
                _playerProfile.Bank -= bet;
                Console.WriteLine($"You lost. Your bank: {_playerProfile.Bank}");
            };
            selectedGame.OnDraw += () =>
            {
                _playerProfile.AddDraw();
                Console.WriteLine($"It's a draw! Your bank: {_playerProfile.Bank}");
            };

            // Запуск игры
            selectedGame.PlayGame();

            // Сохранение профиля после игры
            SavePlayerProfile();
        }

        // Получение корректной ставки от игрока
        private int GetValidBet()
        {
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int bet) && bet > 0 && bet <= _playerProfile.Bank)
                {
                    return bet;
                }
                Console.WriteLine($"Invalid bet. Enter a number between 1 and {_playerProfile.Bank}.");
            }
        }

        // Проверка лимита банка
        private void CheckBankLimit()
        {
            if (_playerProfile.Bank > MaxBankValue)
            {
                int excess = _playerProfile.Bank - MaxBankValue;
                _playerProfile.Bank = MaxBankValue / 2;
                Console.WriteLine($"You wasted half of your bank money in casino’s bar. Your current bank: {_playerProfile.Bank}");
            }
        }

        // Сохранение профиля игрока
        private void SavePlayerProfile()
        {
            _saveLoadService.SaveData(_playerProfile.ToString(), _playerProfile.Name);
        }
    }
}
