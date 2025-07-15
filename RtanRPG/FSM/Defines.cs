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

        protected int sGoal;
        protected int msGoal;

        protected int currTick;
        protected int startTick;
        protected Stat stat;
        public bool IsChangeAble { get { return sGoal <= currTick; } }
        public StateParent(IStateMachine stateMachine, Stat stat)
        {
            this.stateMachine = stateMachine;
            this.stat = stat;
        }

        public virtual void Enter()
        {
            startTick = Environment.TickCount;

        }

        public virtual void Execute()
        {
            currTick = Environment.TickCount - startTick;
            if (currTick >= msGoal) Exit();
        }

        public virtual void Exit()
        {
            throw new NotImplementedException();
        }
    }
    public enum StateType { Attack,Idle,Die}
    public class Stat
    {
        private int maxHP;
        private int currHP;
        public bool IsDie { get { return currHP <= 0; } }
        public int AttackDamage { get; private set; }
        public string Name { get; private set; }

        public Stat(int maxHP, int currHP, int attackDamage, string name)
        {
            this.maxHP = maxHP;
            this.currHP = currHP;
            AttackDamage = attackDamage;
            this.Name = name;
        }

        public void TakeDamage(int v)
        {
            currHP -= v;
            if (IsDie)
            {
                //YOON : FSM 사망으로 강제전이
            }
        }
        public void Heal(int v)
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
