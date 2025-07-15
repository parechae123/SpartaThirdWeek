using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM
{
    public interface IStateMachine
    {
        public void StateChange(StateType state);

    }
    interface IState
    {
        void Enter();
        void Execute();
        void Exit();
        bool IsChangeAble { get; }
    }
    public class StateParent : IState
    {
        protected IStateMachine stateMachine;
        protected string name;//파일명,애니메이션을 불러오기 위한 변수
        public StateParent(IStateMachine stateMachine, string name)
        {
            this.stateMachine = stateMachine;
            this.name = name;
        }

        public bool IsChangeAble => throw new NotImplementedException();

        public virtual void Enter()
        {

        }

        public virtual void Execute()
        {

        }

        public virtual void Exit()
        {
            throw new NotImplementedException();
        }
    }
    public enum StateType { Attack,Idle,Die}
    public class Stat
    {
        private float maxHP;
        private float currHP;
        public float GetHPPercent { get { return currHP / maxHP; } }
        public bool IsDie { get { return currHP <= 0; } }
        public int AttackDamage { get; private set; }
        public string Name { get; private set; }

        public Stat(float maxHP, float currHP, int attackDamage, string name)
        {
            this.maxHP = maxHP;
            this.currHP = currHP;
            AttackDamage = attackDamage;
            this.Name = name;
        }

        public void TakeDamage(float v)
        {
            currHP -= v;
            if (IsDie)
            {
                //YOON : FSM 사망으로 강제전이
            }
        }
        public void Heal(float v)
        {
            if(maxHP < currHP + v)
            {
                currHP = maxHP;
            }
            else
            {
                currHP += v;
            }
            
        }


    }
}
