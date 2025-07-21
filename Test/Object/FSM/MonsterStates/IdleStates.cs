using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Test.Object;
using Test.Utils;
using Test.Utils.Console;

namespace Test.FSM.MonsterStates
{
    class IdleState : StateParent
    {
        //Vector값을 battleScene에서 간격 조정해서 생성자로 인수 넘겨주고 
        private readonly SpriteRenderer _renderer;
        
        private Vector2D _begin = new Vector2D(10, 10);
        private Vector2D _end = new Vector2D(Layout.MaximumContentWidth / 2 - 10, Layout.MaximumContentHeight - 10);
        
        public IdleState(IStateMachine stateMachine) : base(stateMachine)
        {
            var width = _end.Left - _begin.Left;
            var height = _end.Top - _begin.Top;
            
            this.stateMachine = stateMachine;
            _renderer = new SpriteRenderer(DataManager.GetSpriteFilePath($"{stateMachine.GetName}_Idle"));
            _renderer.Prepare(width, height);
        }

        public override void Enter()
        {
            /*sb.ToString();//키값으로 불러 올 string*/
        }

        public override void Execute()
        {
            OutputStream.WriteBuffer(_renderer.GetFrame(), _begin, _end);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
