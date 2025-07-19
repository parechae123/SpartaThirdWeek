using RtanRPG.Utils.Console;
using RtanRPG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM.MonsterStates
{
    class AttackState : StateParent
    {
        SpriteRenderer sr;
        public AttackState(IStateMachine stateMachine) : base(stateMachine)
        {
            this.stateMachine = stateMachine;
            sr = new SpriteRenderer(DataManager.GetSpriteFilePath($"{stateMachine.GetName}_Attack"));
            sr.Prepare(50, 50);
        }

        public override void Enter()
        {

            /*sb.ToString();//키값으로 불러 올 string*/
        }

        public override void Execute()
        {
            //280X70
            OutputStream.WriteBuffer(sr.GetFrame(), new Object.Vector2D(70, 10), new Object.Vector2D(100, 40));

        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
