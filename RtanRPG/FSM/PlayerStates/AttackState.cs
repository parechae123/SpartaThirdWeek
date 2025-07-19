using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RtanRPG.Utils;
using RtanRPG.Utils.Console;

namespace RtanRPG.FSM.PlayerStates
{
    class AttackState : StateParent
    {
        SpriteRenderer sr;
        public AttackState(IStateMachine stateMachine) : base(stateMachine)
        {
            this.stateMachine = stateMachine;
            sr = new SpriteRenderer(DataManager.GetSpriteFilePath("Player_Attack"));
            sr.Prepare(50, 50);
        }

        public override void Enter()
        {
            
            /*sb.ToString();//키값으로 불러 올 string*/
        }

        public override void Execute()
        {
            OutputStream.WriteBuffer(sr.GetFrame(), new Object.Vector2D(10, 10), new Object.Vector2D(60, 60));
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
