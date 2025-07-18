using RtanRPG.FSM.MonsterStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM
{
    class PlayerStateMachine : IStateMachine
    {
        Dictionary<StateType, IState> states;
        IState currState;
        Stat stat;
        public Stat GetStatInfo { get { return stat; } }

        public int GetPhase => 0;
        public string GetName => stat.Name;
        public PlayerStateMachine(Stat stat)
        {
            this.stat = stat;
            states = new Dictionary<StateType, IState>(3);
            states.Add(StateType.Idle, new PlayerStates.IdleState(this));

            currState = states[StateType.Idle];

            states.Add(StateType.Die, new PlayerStates.DieState(this));
            states.Add(StateType.Attack, new PlayerStates.AttackState(this));
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


            currState = states[state];

            currState.Enter();
        }

        public IState GetCurrentState()
        {
            return currState;
        }
    }
}
