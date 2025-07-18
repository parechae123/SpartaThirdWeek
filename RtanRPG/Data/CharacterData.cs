using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Data
{
    public class CharacterData
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public string Class { get; set; }
        public int HealthPoint { get; set; }
        public int AttackPoint { get; set; }
        public int DefensePoint { get; set; }
        public int Gold { get; set; }
    }
}
