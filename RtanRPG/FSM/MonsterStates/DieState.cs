using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM.MonsterStates
{
    class DieState : StateParent
    {
        public DieState(IStateMachine stateMachine) : base(stateMachine)
        {
            this.stateMachine = stateMachine;
            
        }

        public override void Enter()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(stateMachine.GetName);
            sb.Append("_Die_");
            sb.Append(stateMachine.GetPhase);
            /*sb.ToString();//키값으로 불러 올 string*/

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
