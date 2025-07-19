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
        private int healthPoint = 200; 
        public int HealthPoint
        {
            get => healthPoint;
            set => healthPoint = Math.Clamp(value, 0, MaxHp);
        } 

        public string Name { get; set; } = "Player";
        public int Level { get; set; } = 1;
        public string Class { get; set; } = "Musician";
        public int MaxHp {  get; set; }
        public int AttackPoint { get; set; } = 20;
        public int DefensePoint { get; set; } = 20;
        public int Gold { get; set; } = 0;

        public CharacterData()
        {
            MaxHp = 200;
            HealthPoint = MaxHp;
        }
    }
}
