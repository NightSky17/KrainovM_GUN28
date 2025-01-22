using GamePrototype.Items.EconomicItems;
using GamePrototype.Items.EquipItems;
using GamePrototype.Units;

namespace GamePrototype.Utils
{
    public class UnitFactoryDemo
    {
        public static Unit CreateHardModePlayer(string name)
        {
            var player = new Player(name, 30, 30, 6);
            player.AddItemToInventory(new Weapon(10, 15, "Sword"));
            player.AddItemToInventory(new Armour(10, 15, "Armour"));
            player.AddItemToInventory(new RangeWeapon(15, 25, "Bow"));
            player.AddItemToInventory(new Helmet(20, 35, "Helmet"));
            player.AddItemToInventory(new HealthPotion("Potion"));
            return player;
        }

        public static Unit CreateEasyModePlayer(string name)
        {
            var player = new Player(name, 50, 50, 8);
            player.AddItemToInventory(new Weapon(20, 30, "Enchanted Sword"));
            player.AddItemToInventory(new Armour(25, 40, "Steel Plate"));
            player.AddItemToInventory(new RangeWeapon(15, 35, "Magic Bow"));
            player.AddItemToInventory(new Helmet(30, 45, "Dragon Scale Armor"));
            player.AddItemToInventory(new HealthPotion("Greater Health Potion"));
            player.AddItemToInventory(new HealthPotion("Greater Health Potion"));
            return player;
        }

        public static Unit CreateGoblinEnemy() => new Goblin(GameConstants.Goblin, 18, 18, 2);
        public static Unit CreateEliteGoblinEnemy() => new Goblin(GameConstants.Goblin, 35, 35, 4);
    }
}
