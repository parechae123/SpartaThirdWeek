using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM.States
{
    class DieState : StateParent
    {
        public DieState(IStateMachine stateMachine, string name) : base(stateMachine, name)
        {
            this.stateMachine = stateMachine;
            this.name = name;
        }

        public override void Enter()
        {
            

        }

        public override void Execute()
        {
            
        }

        public override void Exit()
        {
            if (!IsChangeAble) return;
        }
    }
}
