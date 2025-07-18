using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM.PlayerStates
{
    class IdleState : StateParent
    {
        public IdleState(IStateMachine stateMachine) : base(stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public override void Enter()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Player_Idle_");
            sb.Append(stateMachine.GetPhase);
            /*sb.ToString();//키값으로 불러 올 string*/
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
}
