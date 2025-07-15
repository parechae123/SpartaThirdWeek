using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM.States
{
    class AttackState : StateParent
    {
        public AttackState(IStateMachine stateMachine,int goal) : base(stateMachine)
        {
            this.stateMachine = stateMachine;
            this.sGoal = goal;
        }

        public override void Enter()
        {
            base.Enter();

        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
    class PhaseOneAttackState : AttackState
    {

        public PhaseOneAttackState(IStateMachine stateMachine, int goal) : base(stateMachine, goal)
        {
            this.stateMachine = stateMachine;
            this.sGoal = goal;
            currTick = 0;
        }
    }
    class PhaseTwoAttackState : AttackState
    {

        public PhaseTwoAttackState(IStateMachine stateMachine, int goal) : base(stateMachine, goal)
        {
            this.stateMachine = stateMachine;
            this.sGoal = goal;
            currTick = 0;
        }
    }
    class PhaseThreeAttackState : AttackState
    {
        public PhaseThreeAttackState(IStateMachine stateMachine, int goal) : base(stateMachine, goal)
        {
            this.stateMachine = stateMachine;
            this.sGoal = goal;
            currTick = 0;
        }
    }
}
