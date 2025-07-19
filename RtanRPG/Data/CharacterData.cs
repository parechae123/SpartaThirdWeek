using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Data
{
    public class CharacterData
    {
        private int healthPoint;
        public int HealthPoint
        {
            get => healthPoint;
            set => healthPoint = Math.Clamp(value, 0, MaxHp);
        }

        public string Name { get; set; }
        public int Level { get; set; }
        public string Class { get; set; }
        public int MaxHp {  get; set; }
        public int AttackPoint { get; set; }
        public int DefensePoint { get; set; }
        public int Gold { get; set; }

        public CharacterData()
        {
            MaxHp = 100;
            HealthPoint = MaxHp;
        }
    }
}
