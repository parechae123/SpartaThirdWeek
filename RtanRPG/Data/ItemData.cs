using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Data
{
    public enum ItemCategory
    {
        Equip,
        Consumable,
        Quest
    }
    public class ItemData
    {
        public string Name { get; set; }
        public ItemCategory Category { get; set; }
        public string Description { get; set; }
    }
    public class ConsumableItemData : ItemData  //소모품 아이템
    {
        public int HealAmount { get; set; }
        public ConsumableItemData()
        {
            Category = ItemCategory.Consumable;
        }
        public void Use(PlayerData player)
        {
            player.HealthPoint += HealAmount;
            Console.WriteLine($"{Name} used. Healed {HealAmount} HP!");
            if (player.HealthPoint > player.MaxHealthPoint )
                player.HealthPoint = player.MaxHealthPoint;
        }
    }
}
