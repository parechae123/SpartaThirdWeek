using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM.States
{
    class AttackState : StateParent
    {
        public AttackState(IStateMachine stateMachine, string name) : base(stateMachine, name)
        {
            this.stateMachine = stateMachine;
            this.name = name;
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

        public PhaseOneAttackState(IStateMachine stateMachine, string name) : base(stateMachine, name)
        {
            this.stateMachine = stateMachine;
            this.name = name;
        }
    }
    class PhaseTwoAttackState : AttackState
    {

        public PhaseTwoAttackState(IStateMachine stateMachine, string name) : base(stateMachine, name)
        {
            this.stateMachine = stateMachine;
            this.name = name;
        }
    }
    class PhaseThreeAttackState : AttackState
    {
        public PhaseThreeAttackState(IStateMachine stateMachine, string name) : base(stateMachine, name)
        {
            this.stateMachine = stateMachine;
            this.name = name;
        }
    }
}
