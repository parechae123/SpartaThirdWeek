using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtanRPG.FSM.States;
namespace RtanRPG.FSM
{
    class MonsterStateMachine : IStateMachine
    {
        Dictionary<StateType, IState[]> stateMachine;
        IState currState;
        Stat stat;
        public int phase = 0;
        //일단은 3페이즈까지 있다는 가정 하에 작성
        public MonsterStateMachine(Stat stat)
        {
            this.stat = stat;
            phase = 0;
            stateMachine = new Dictionary<StateType, IState[]>(3);

            currState = stateMachine[StateType.Idle][0];
            stateMachine.Add(StateType.Attack, new IState[3]
            {   new PhaseOneAttackState(this,stat.Name),
                new PhaseTwoAttackState(this, stat.Name),
                new PhaseThreeAttackState(this, stat.Name) });

            stateMachine.Add
                (StateType.Idle, new IState[3] 
                {   new PhaseOneAttackState(this,stat.Name),
                    new PhaseTwoAttackState(this,stat.Name),
                    new PhaseThreeAttackState(this,stat.Name) });
            stateMachine.Add
                (StateType.Die, new IState[3] 
                {   new DieState(this,stat.Name),
                    new DieState(this,stat.Name),
                    new DieState(this, stat.Name)});
        }

        public void StateMachineUpdate()
        {
            currState.Execute();
        }

        public void StateChange(StateType state)
        {
            currState.Exit();
            float currHP = stat.GetHPPercent;
            phase = currHP > 0.7f ? 0 : currHP > 0.3f ? 1 : 2;//0.7이상일시 0페, 이하 1페, 0.3보다 작으면 2페
            currState = stateMachine[state][phase];
            currState.Enter();
        }
    }


}
