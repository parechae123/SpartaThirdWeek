using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM.States
{
    class AttackState : StateParent
    {
        public AttackState(IStateMachine stateMachine,Stat stat) : base(stateMachine,stat)
        {
            this.stateMachine = stateMachine;
            this.stat = stat;
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

        public PhaseOneAttackState(IStateMachine stateMachine, Stat stat) : base(stateMachine, stat)
        {
            this.stateMachine = stateMachine;
            this.stat = stat;
            currTick = 0;
        }
    }
    class PhaseTwoAttackState : AttackState
    {

        public PhaseTwoAttackState(IStateMachine stateMachine, Stat stat) : base(stateMachine, stat)
        {
            this.stateMachine = stateMachine;
            this.stat = stat;
            currTick = 0;
        }
    }
    class PhaseThreeAttackState : AttackState
    {
        public PhaseThreeAttackState(IStateMachine stateMachine, Stat stat) : base(stateMachine, stat)
        {
            this.stateMachine = stateMachine;
            this.stat = stat;
            currTick = 0;
        }
    }
}
