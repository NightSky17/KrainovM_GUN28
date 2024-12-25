using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classes
{
    internal class Unit
    {
        public string Name { get; } // Имя юнита
        public float Health { get; private set; } // Здоровье
        public int Damage { get; } = 5; // Урон
        public float Armor { get; } = 0.6f; // Броня

        // Приватный конструктор
        private Unit(string name, float health)
        {
            Name = name;
            Health = health;
        }

        // Публичный конструктор только для чтения
        public Unit(string name) : this(name, 100f) // Здоровье по умолчанию
        {
        }

        public Unit() : this("Unknown Unit", 100f) // Конструктор по умолчанию
        {
        }

        public float GetRealHealth() // Реальное здоровье
        {
            float realHealth = Health * (1f + Armor);
            return realHealth;
        }

        public bool SetDamage(float value) // Получение урона
        {
            Health = Health - (value * Armor);
            if (Health <= 0f)
            {
                return true; // Юнит погиб
            }
            else
            {
                return false; // Юнит жив
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Unit unit = new Unit();
            Console.WriteLine("Имя юнита: " + unit.Name);
            Console.WriteLine("Здоровье: " + unit.Health);
            Console.WriteLine("Броня: " + unit.Armor);

            float realHealth = unit.GetRealHealth();
            Console.WriteLine("Фактическое здоровье: " + realHealth);

            bool isDead = unit.SetDamage(20f);
            Console.WriteLine("После получения урона: " + unit.Health);

            if (isDead)
            {
                Console.WriteLine("Юнит погиб");
            }
            else
            {
                Console.WriteLine("Юнит жив");
            }
        }
    }
}
