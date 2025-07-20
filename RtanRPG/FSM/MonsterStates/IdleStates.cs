using RtanRPG.Utils.Console;
using RtanRPG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RtanRPG.FSM.MonsterStates
{
    class IdleState : StateParent
    {
        //Vector값을 battleScene에서 간격 조정해서 생성자로 인수 넘겨주고 
        SpriteRenderer sr;
        public IdleState(IStateMachine stateMachine) : base(stateMachine)
        {
            this.stateMachine = stateMachine;
            sr = new SpriteRenderer(DataManager.GetSpriteFilePath($"{stateMachine.GetName}_Idle"));
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
