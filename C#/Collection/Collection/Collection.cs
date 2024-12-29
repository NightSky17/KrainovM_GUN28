using System;

namespace Collection
{
    struct Interval
    {
        public double Min;
        public double Max;
        private static Random random = new Random();

        public Interval(int minValue, int maxValue)
        {
            if (minValue > maxValue)
            {
                Console.WriteLine("Некорректные значения интервала. Меняем местами.");
                int temp = minValue;
                minValue = maxValue;
                maxValue = temp;
            }

            if (minValue < 0)
            {
                Console.WriteLine("Минимальное значение меньше 0. Устанавливаем 0.");
                minValue = 0;
            }

            if (maxValue < 0)
            {
                Console.WriteLine("Максимальное значение меньше 0. Устанавливаем 0.");
                maxValue = 0;
            }

            if (minValue == maxValue)
            {
                Console.WriteLine("Границы равны. Увеличиваем максимальное значение на 10.");
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
        public string Name;
        public Interval DamageRange;

        public Weapon(string name, int minDamage, int maxDamage)
        {
            Name = name;
            DamageRange = new Interval(minDamage, maxDamage);
        }
    }

    class Unit
    {
        public string Name;
        public Interval Damage;

        public Unit(string name, int minDamage, int maxDamage)
        {
            Name = name;
            Damage = new Interval(minDamage, maxDamage);
        }
    }

    class Room
    {
        public Unit Unit;
        public Weapon Weapon;

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
            rooms = new Room[3];
            rooms[0] = new Room(new Unit("Воин", 10, 20), new Weapon("Меч", 15, 25));
            rooms[1] = new Room(new Unit("Лучник", 5, 15), new Weapon("Лук", 10, 20));
            rooms[2] = new Room(new Unit("Маг", 8, 18), new Weapon("Посох", 12, 22));
        }

        public void ShowRooms()
        {
            for (int i = 0; i < rooms.Length; i++)
            {
                Console.WriteLine("Комната " + (i + 1) + ":");
                Console.WriteLine("Юнит: " + rooms[i].Unit.Name + ", Урон: " + rooms[i].Unit.Damage.Min + " - " + rooms[i].Unit.Damage.Max);
                Console.WriteLine("Оружие: " + rooms[i].Weapon.Name + ", Урон: " + rooms[i].Weapon.DamageRange.Min + " - " + rooms[i].Weapon.DamageRange.Max);
                Console.WriteLine("---");
            }
        }
    }

    class Collection
    {
        static void Main(string[] args)
        {
            Dungeon dungeon = new Dungeon();
            dungeon.ShowRooms();
        }
    }
}
