using System;

namespace Classes
{
    struct Interval
    {
        public double Min { get; }
        public double Max { get; }
        private static Random random = new Random();

        public Interval(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                Console.WriteLine("Некорректные значения интервала. Поменяем местами.");
                (minValue, maxValue) = (maxValue, minValue);
            }

            if (minValue < 0)
            {
                Console.WriteLine("Минимальное значение интервала меньше 0. Установим 0.");
                minValue = 0;
            }

            if (maxValue < 0)
            {
                Console.WriteLine("Максимальное значение интервала меньше 0. Установим 0.");
                maxValue = 0;
            }

            if (minValue == maxValue)
            {
                Console.WriteLine("Границы интервала равны. Увеличим максимальное значение на 10.");
                maxValue += 10;
            }

            Min = minValue;
            Max = maxValue;
        }

        public double Get()
        {
            return random.NextDouble() * (Max - Min) + Min;
        }
    }

    class Weapon
    {
        public string Name { get; }
        public Interval DamageRange { get; private set; }

        public Weapon(string name, int minDamage, int maxDamage)
        {
            Name = name;
            DamageRange = new Interval(minDamage, maxDamage);
        }

        public void SetDamageRange(int minDamage, int maxDamage)
        {
            DamageRange = new Interval(minDamage, maxDamage);
        }
    }

    class Unit
    {
        public string Name { get; }
        public Interval Damage { get; private set; }

        public Unit(string name, int minDamage, int maxDamage)
        {
            Name = name;
            Damage = new Interval(minDamage, maxDamage);
        }
    }

    class Room
    {
        public Unit Unit { get; }
        public Weapon Weapon { get; }

        public Room(Unit unit, Weapon weapon)
        {
            Unit = unit;
            Weapon = weapon;
        }
    }

    class Dungeon
    {
        private Room[] rooms;

        public Dungeon()
        {
            rooms = new Room[]
            {
                new Room(new Unit("Воин", 10, 20), new Weapon("Меч", 15, 25)),
                new Room(new Unit("Лучник", 5, 15), new Weapon("Лук", 10, 20)),
                new Room(new Unit("Маг", 8, 18), new Weapon("Посох", 12, 22))
            };
        }

        public void ShowRooms()
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                Console.WriteLine("Комната " + (i + 1) + ":");
                Console.WriteLine("Юнит: " + rooms[i].Unit.Name + ", Урон: " + rooms[i].Unit.Damage.Min + " - " + rooms[i].Unit.Damage.Max);
                Console.WriteLine("Оружие: " + rooms[i].Weapon.Name + ", Урон: " + rooms[i].Weapon.DamageRange.Min + " - " + rooms[i].Weapon.DamageRange.Max);
                Console.WriteLine("—");
            }
        }
    }

    class Memory
    {
        static void Main(string[] args)
        {
            Dungeon dungeon = new Dungeon();
            dungeon.ShowRooms();
        }
    }
}
