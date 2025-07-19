using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Data
{
    public enum EquipType
    {
        Weapon,
        Armor,
        Accessory
    }

    public class EquipData:ItemData
    {
        public string Name { get; set; }
        public EquipType Type { get; set; }
        public int AttackBonus { get; set; }
        public int DefenseBonus {  get; set; }
        public EquipData()
        {
            Category = ItemCategory.Equip;
        }
    }
}
