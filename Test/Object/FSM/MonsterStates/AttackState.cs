
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Test.Object;
using Test.Utils;
using Test.Utils.Console;

namespace Test.FSM.MonsterStates
{
    class AttackState : StateParent
    {
        SpriteRenderer sr;

        private Vector2D _begin = new Vector2D(10, 10);
        private Vector2D _end = new Vector2D(Layout.MaximumContentWidth / 2 - 10, Layout.MaximumContentHeight - 10);
        
        public AttackState(IStateMachine stateMachine) : base(stateMachine)
        {
            var width = _end.Left - _begin.Left;
            var height = _end.Top - _begin.Top;
            
            this.stateMachine = stateMachine;
            sr = new SpriteRenderer(DataManager.GetSpriteFilePath($"{stateMachine.GetName}_Attack"));
            sr.Prepare(width, height);
        }

        public override void Enter()
        {

            /*sb.ToString();//키값으로 불러 올 string*/
        }

        public override void Execute()
        {
            //280X70
            OutputStream.WriteBuffer(sr.GetFrame(), _begin, _end);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
