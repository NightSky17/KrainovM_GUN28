using GamePrototype.Items.EconomicItems;
using GamePrototype.Items.EquipItems;
using GamePrototype.Utils;
using System.Text;

namespace GamePrototype.Units
{
    public sealed class Player : Unit
    {
        private readonly Dictionary<EquipSlot, EquipItem> _equipment = new();
        public Inventory GetInventory() => Inventory;
        public Dictionary<EquipSlot, EquipItem> GetEquipment() => _equipment;

        public Player(string name, uint health, uint maxHealth, uint baseDamage) : base(name, health, maxHealth, baseDamage)
        {            
        }


        public override uint GetUnitDamage()
        {
            if (_equipment.TryGetValue(EquipSlot.Weapon, out var item) && item is Weapon weapon) 
            {
                return BaseDamage + weapon.Damage;
            }
            return BaseDamage;
        }

        public override void HandleCombatComplete()
        {
            var items = Inventory.Items;
            for (int i = 0; i < items.Count; i++) 
            {
                if (items[i] is EconomicItem economicItem) 
                {
                    UseEconomicItem(economicItem);
                    Inventory.TryRemove(items[i]);
                }
            }
        }

        public override void AddItemToInventory(Item item)
        {
            if (item is EquipItem equipItem && _equipment.TryAdd(equipItem.Slot, equipItem)) 
            {
                // Item was equipped
                return;
            }
            base.AddItemToInventory(item);
        }

        private void UseEconomicItem(EconomicItem economicItem)
        {
            if (economicItem is HealthPotion healthPotion) 
            {
                Health += healthPotion.HealthRestore;
            }
            else if (economicItem is Grindstone grindstone)
            {
                if (_equipment.TryGetValue(EquipSlot.Weapon, out var item)) 
                {
                    item.Repair(grindstone.DurabilityRestore);
                }
            }
        }

        protected override uint CalculateAppliedDamage(uint damage)
        {
            if (_equipment.TryGetValue(EquipSlot.Armour, out var item) && item is Armour armour) 
            {
                damage -= (uint)(damage * (armour.Defence / 100f));
                armour.DecreaseDefence();
            }
            return damage;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.AppendLine(Name);
            builder.AppendLine($"Health {Health}/{MaxHealth}");
            builder.AppendLine("Loot:");
            var items = Inventory.Items;
            for (int i = 0; i < items.Count; i++) 
            {
                builder.AppendLine($"[{items[i].Name}] : {items[i].Amount}");
            }
            return builder.ToString();
        }

        public string[] ChangeAmmunition()
        {
            var messages = new List<string>();
            
            if (_equipment.TryGetValue(EquipSlot.Weapon, out var weapon))
            {
                if (weapon.Durability >= weapon.MaxDurability)
                {
                    var newWeapon = Inventory.Items.FirstOrDefault(i => i is EquipItem equip && equip.Slot == EquipSlot.Weapon);
                    if (newWeapon != null)
                    {
                        _equipment[EquipSlot.Weapon] = (EquipItem)newWeapon;
                        Inventory.TryRemove(newWeapon);
                        Inventory.TryAdd(weapon);
                        messages.Add($"Weapon {weapon.Name} replaced with {newWeapon.Name}");
                    }
                }
            }

            if (_equipment.TryGetValue(EquipSlot.Armour, out var armour))
            {
                var equippedArmour = (Armour)armour;
                if (equippedArmour.Durability >= equippedArmour.MaxDurability || equippedArmour.Defence == 0)
                {
                    var newArmour = Inventory.Items.FirstOrDefault(i => i is EquipItem equip && equip.Slot == EquipSlot.Armour);
                    if (newArmour != null)
                    {
                        _equipment[EquipSlot.Armour] = (EquipItem)newArmour;
                        Inventory.TryRemove(newArmour);
                        Inventory.TryAdd(armour);
                        messages.Add($"Armour {armour.Name} replaced with {newArmour.Name}");
                    }
                }
            }

            return messages.ToArray();
        }
    }
}
