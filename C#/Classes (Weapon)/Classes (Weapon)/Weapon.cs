using System;

namespace Classes
{
    class Weapon
    {
        public string Name { get; } // Имя оружия
        public int MinDamage { get; private set; } // Минимальный урон
        public int MaxDamage { get; private set; } // Максимальный урон
        public float Durability { get; } = 1f; // Прочность

        // Публичный конструктор
        public Weapon(string name)
        {
            Name = name;
            SetDamageParams(1, 10); // Значения урона по умолчанию
        }

        public Weapon(string name, int minDamage, int maxDamage) : this(name)
        {
            SetDamageParams(minDamage, maxDamage);
        }

        // Метод задания параметров урона
        public void SetDamageParams(int minDamage, int maxDamage)
        {
            if (minDamage > maxDamage)
            {
                Console.WriteLine("Некорректные значения урона для оружия " + Name + ". Поменяем местами.");
                (minDamage, maxDamage) = (maxDamage, minDamage);
            }

            if (minDamage < 1)
            {
                Console.WriteLine("Минимальный урон для оружия " + Name + " слишком мал. Установим значение 1.");
                minDamage = 1;
            }

            if (maxDamage <= 1)
            {
                Console.WriteLine("Максимальный урон для оружия " + Name + " слишком мал. Установим значение 10.");
                maxDamage = 10;
            }

            MinDamage = minDamage;
            MaxDamage = maxDamage;
        }

        // Метод получения среднего урона
        public int GetDamage()
        {
            return (MinDamage + MaxDamage) / 2;
        }
    }

    class Weapon2
    {
        static void Main(string[] args)
        {
            Weapon weapon1 = new Weapon("Меч");
            Console.WriteLine("Оружие: " + weapon1.Name + ", Прочность: " + weapon1.Durability);

            Weapon weapon2 = new Weapon("Лук", 5, 15);
            Console.WriteLine("Оружие: " + weapon2.Name + ", Минимальный урон: " + weapon2.MinDamage + ", Максимальный урон: " + weapon2.MaxDamage);

            weapon2.SetDamageParams(20, 10);
            Console.WriteLine("После корректировки: Минимальный урон: " + weapon2.MinDamage + ", Максимальный урон: " + weapon2.MaxDamage);

            int averageDamage = weapon2.GetDamage();
            Console.WriteLine("Средний урон: " + averageDamage);
        }
    }
}
