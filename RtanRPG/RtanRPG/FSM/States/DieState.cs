using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM.States
{
    class DieState : StateParent
    {
        public DieState(IStateMachine stateMachine, int goal,Stat stat) : base(stateMachine,stat)
        {
            this.stateMachine = stateMachine;
            this.msGoal = goal;
            this.stat = stat;
            currTick = 0;
        }

        public override void Enter()
        {
            startTick = Environment.TickCount;

        }

        public override void Execute()
        {
            currTick = Environment.TickCount - startTick;
            if (currTick >= msGoal) Exit();
        }

        public override void Exit()
        {
            if (!IsChangeAble) return;
        }
    }
}
