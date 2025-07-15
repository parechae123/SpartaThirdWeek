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
        public int phase = 0;
        //일단은 3페이즈까지 있다는 가정 하에 작성
        public MonsterStateMachine(Stat stat)
        {
            phase = 0;
            stateMachine = new Dictionary<StateType, IState[]>(3);

            stateMachine.Add(StateType.Attack, new IState[3] 
            {   new PhaseOneAttackState(this,stat),
                new PhaseTwoAttackState(this,stat),
                new PhaseThreeAttackState(this,stat) });

            stateMachine.Add
                (StateType.Idle, new IState[3] 
                {   new PhaseOneAttackState(this, stat),
                    new PhaseTwoAttackState(this, stat),
                    new PhaseThreeAttackState(this, stat) });
            currState = stateMachine[StateType.Idle][0];
        }

        public void StateMachineUpdate()
        {
            currState.Execute();
        }

        public void StateChange(StateType state)
        {
            currState.Exit();
            currState = stateMachine[state][phase];
            currState.Enter();
        }
    }


}
