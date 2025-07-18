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
        public void TakeDamage(int v)
        {
            HealthPoint -= v;
            /*            if (IsDie)
                        {
                            //YOON : FSM 사망으로 강제전이

                        }*/
        }
        public void Heal(int v)
        {
            HealthPoint += v;
/*            if (maxHP < currHP + v)
            {
                currHP = maxHP;
            }
            else
            {
                currHP += v;
            }*/

        }
    }
}
