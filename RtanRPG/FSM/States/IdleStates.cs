using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM.States
{
    class IdleState : StateParent
    {
        public IdleState(IStateMachine stateMachine, int goal,Stat stat) : base(stateMachine, stat)
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
            throw new NotImplementedException();
        }
    }

    class PhaseOneIdleState : IdleState
    {
        public PhaseOneIdleState(IStateMachine stateMachine, int goal,Stat stat) : base(stateMachine, goal,stat)
        {
            this.stateMachine = stateMachine;
            this.sGoal = goal;
            currTick = 0;
        }
    }
    class PhaseTwoIdleState : IdleState
    {
        public PhaseTwoIdleState(IStateMachine stateMachine, int goal, Stat stat) : base(stateMachine, goal, stat)
        {
            this.stateMachine = stateMachine;
            this.sGoal = goal;
            currTick = 0;
        }

    }
    class PhaseThreeIdleState : IdleState
    {
        public PhaseThreeIdleState(IStateMachine stateMachine, int goal, Stat stat) : base(stateMachine, goal, stat)
        {
            this.stateMachine = stateMachine;
            this.sGoal = goal;
            currTick = 0;
        }
    }
}
