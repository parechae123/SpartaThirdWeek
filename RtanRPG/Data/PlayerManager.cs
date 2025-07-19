using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Data
{
    // 만들것: 장착가능여부검사, 기존장비해제후교체, 스탯변경적용
    internal class PlayerManager  //장착/해제 및 아이템 사용
    {
        private PlayerData player;

        public PlayerManager(PlayerData player)
        {
            this.player = player;
        }

        public void EquipItem(EquipData item)
        {
            if (item == null)
            {
                Console.WriteLine("There's no items to equip.");
                return;
            }
            if(!player.Inventory.Contains(item))
            {
                Console.WriteLine("these not in your inventory.");
                return;
            }
            if (player.EquippedItems.ContainsKey(item.Type))
            {
                UnequipItem(item.Type);
            }
            player.EquippedItems[item.Type] = item;
            Console.WriteLine($"{item.Name} Equipped.");
        }
        
        public void UnequipItem(EquipType type)
        {
            if(player.EquippedItems.TryGetValue(type,out var equipped))
            {
                Console.WriteLine($"{equipped.Name} Cleared");
                player.EquippedItems.Remove(type);
            }
            else
            {
                Console.WriteLine($"{type} The equipment is not equipped.");
            }
        }
        public void UseItem(ItemData item)
        {
            if (item == null)
            {
                Console.WriteLine("There's no items to use.");
                return;
            }
            if (!player.Inventory.Contains(item))
            {
                Console.WriteLine("these not in your inventory.");
                return;
            }
            if (item is ConsumableItemData consumable)
            {
                consumable.Use(player);
                player.Inventory.Remove((EquipData)item);
            }
            else
            {
                Console.WriteLine($"{item.Name}is not available.");
            }
        }
    }
}
