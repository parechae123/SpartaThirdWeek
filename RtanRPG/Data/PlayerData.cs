using RtanRPG.Utils;
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


        //MaxHP 역할을 할 변수가 보이지 않아 일단 깡 덧셈 뺄샘으로 작업해두었습니다.
        public void TakeDamage(int amount)
        {
            HealthPoint -= amount;
            // 사망 처리 FSM 연결 가능
        }

        public void Heal(int amount)
        {
            HealthPoint += amount; // 자동으로 MaxHp 초과 안 됨
        }

    }
}
