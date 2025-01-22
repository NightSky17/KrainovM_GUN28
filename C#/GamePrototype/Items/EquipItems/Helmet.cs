using GamePrototype.Utils;

namespace GamePrototype.Items.EquipItems
{
    public class Helmet : EquipItem
    {
        public Helmet(uint defence, uint durability, string name) : base(durability, name) => Defence = defence;

        public uint Defence { get; private set; }

        public void DecreaseDefence() => Defence -= 1;

        public override EquipSlot Slot => EquipSlot.Armour;
    }
}