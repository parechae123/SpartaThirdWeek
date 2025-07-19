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
        private int maxHp;

        public int MaxHp
        {
            get => maxHp;
            set => maxHp = Math.Clamp(value, 0, 100);  // 0~100 사이로 제한
        }

        public string Name { get; set; }
        public int Level { get; set; }
        public string Class { get; set; }
        public int HealthPoint { get; set; }
        public int AttackPoint { get; set; }
        public int DefensePoint { get; set; }
        public int Gold { get; set; }

        public CharacterData()
        {
            MaxHp = 100;          // 기본 최대체력
            HealthPoint = MaxHp;  // 현재 체력은 최대체력으로 초기화
        }
    }
}
