using RtanRPG.FSM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Utils
{
    public class PlayerCharactor
    {
        public Stat stat;
        
        public PlayerCharactor()
        {
            var data = DataManager.Instance.PlayerData;

            // Stat 생성자: (maxHP, currHP, attackDamage, name, stateMachine)
            stat = new Stat(
                data.HealthPoint,     // maxHP
                data.HealthPoint,     // currHP (처음엔 동일하게 설정)
                data.AttackPoint,
                data.Name
            );

            stat.stateMachine = null;
        }

        public void Execute()
        {
            stat.stateMachine?.GetCurrentState()?.Execute();
        }

        public void ChangeState(IStateMachine newState)
        {
            stat.stateMachine = newState;
        }

        // 전투 중 플레이어가 사용할 함수들

        public void TakeDamage(float amount)        //데미지
        {
            stat.TakeDamage(amount);
        }

        public void Heal(float amount)              //치료
        {
            stat.Heal(amount);
        }

        public void Attack(Stat stat)               //공격
        {
            stat.TakeDamage(this.stat.AttackDamage);
        }

        public bool Escape()                        //도망가기
        {
            return true;
        }
    }
}
