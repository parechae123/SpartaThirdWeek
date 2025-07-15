using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RtanRPG.FSM.States
{
    class IdleState : StateParent
    {
        public IdleState(IStateMachine stateMachine,string name) : base(stateMachine, name)
        {
            this.stateMachine = stateMachine;
        }

        public override void Enter()
        {

        }

        public override void Execute()
        {

        }

        public override void Exit()
        {
            throw new NotImplementedException();
        }
    }

    class PhaseOneIdleState : IdleState
    {
        public PhaseOneIdleState(IStateMachine stateMachine, string name) : base(stateMachine,name)
        {
            this.stateMachine = stateMachine;
            this.name = name;
        }
        public override void Enter()
        {
            base.Enter();
        }
    }
    class PhaseTwoIdleState : IdleState
    {
        public PhaseTwoIdleState(IStateMachine stateMachine,string name) : base(stateMachine,name)
        {
            this.stateMachine = stateMachine;
            this.name = name;
        }


    }
    class PhaseThreeIdleState : IdleState
    {
        public PhaseThreeIdleState(IStateMachine stateMachine, string name) : base(stateMachine, name)
        {
            this.stateMachine = stateMachine;
            this.name = name;
        }
    }
}
