using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Data
{
    public class PlayerData : CharacterData
    {
        public int[] Inventories { get; set; }
        public int[] Equipments { get; set; }
    }
}
