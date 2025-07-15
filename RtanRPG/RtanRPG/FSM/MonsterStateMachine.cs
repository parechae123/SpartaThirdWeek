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
        public MonsterStateMachine()
        {
            phase = 0;
            stateMachine = new Dictionary<StateType, IState[]>(3);

            stateMachine.Add(StateType.Attack, new IState[3] 
            {   new PhaseOneAttackState(this,1),
                new PhaseTwoAttackState(this,1),
                new PhaseThreeAttackState(this,1) });

            stateMachine.Add
                (StateType.Idle, new IState[3] 
                {   new PhaseOneAttackState(this, 1),
                    new PhaseTwoAttackState(this, 1),
                    new PhaseThreeAttackState(this, 1) });
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
