using RtanRPG.Utils.Console;
using RtanRPG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM.MonsterStates
{
    class DieState : StateParent
    {
        SpriteRenderer sr;
        public DieState(IStateMachine stateMachine) : base(stateMachine)
        {
            this.stateMachine = stateMachine;
            sr = new SpriteRenderer(DataManager.GetSpriteFilePath($"{stateMachine.GetName}_Die"));
            sr.Prepare(30, 30);
        }

        public override void Enter()
        {

            /*sb.ToString();//키값으로 불러 올 string*/
        }

        public override void Execute()
        {
            OutputStream.WriteBuffer(sr.GetFrame(), new Object.Vector2D(70, 10), new Object.Vector2D(100, 40));
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
