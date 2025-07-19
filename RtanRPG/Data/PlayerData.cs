using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Data
{
    public class PlayerData : CharacterData   //인벤토리, 장착장비, 골드 상태 저장
    {
        public List<EquipData> Inventory { get; set; } = new List<EquipData>();
        public Dictionary<EquipType, EquipData> EquippedItems { get; set; } = new();
        public int MaxHealthPoint { get; set; } = 100;
        public List<ItemData> Inventory1 { get; set; } = new();  //장비랑 동시에 관리 못함?
        public Dictionary<EquipType, EquipData> EquippedItem { get; set; } = new();
    }
    public class Equip2Data : ItemData
    {
        public EquipType Type { get; set; }
        public int AttackBonus { get; set; }
        public int DefenseBonus { get; set; }
    }
}
