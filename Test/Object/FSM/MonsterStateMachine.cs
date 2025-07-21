using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.FSM.MonsterStates;
namespace Test.FSM
{
    class MonsterStateMachine : IStateMachine
    {
        Dictionary<StateType, IState> states;
        IState currState;
        Stat stat;
        public Stat GetStatInfo { get { return stat; } }

        public string GetName => stat.Name;

        public int GetPhase => phase;

        private int phase = 0;
        //일단은 3페이즈까지 있다는 가정 하에 작성
        public MonsterStateMachine(Stat stat)
        {
            this.stat = stat;
            phase = 0;
            states = new Dictionary<StateType, IState>(3);
            states.Add(StateType.Idle, new MonsterStates.IdleState(this));

            currState = states[StateType.Idle];

            states.Add(StateType.Die, new MonsterStates.DieState(this));
            states.Add(StateType.Attack, new MonsterStates.AttackState(this));
        }

        public void StateMachineUpdate()
        {
            currState.Execute();
        }

        public void StateChange(StateType state)
        {
            if (state != StateType.Die && stat.GetHPPercent <= 0) { StateChange(StateType.Die); return; }
            if (states[state] == currState || currState == states[StateType.Die]) return;
            currState.Exit();

            float currHP = stat.GetHPPercent;

            phase = currHP > 0.7f ? 0 : currHP > 0.3f ? 1 : 2;//0.7이상일시 0페, 이하 1페, 0.3보다 작으면 2페

            currState = states[state];

            currState.Enter();
        }

        public IState GetCurrentState()
        {
            return currState;
        }
    }


}
