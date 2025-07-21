using Test.Object;
using Test.Utils;
using Test.Utils.Console;

namespace Test.FSM.MonsterStates
{
    public class DieState : StateParent
    {
        SpriteRenderer sr;
        
        private Vector2D _begin = new Vector2D(10, 10);
        private Vector2D _end = new Vector2D(Layout.MaximumContentWidth / 2 - 10, Layout.MaximumContentHeight - 10);
        
        public DieState(IStateMachine stateMachine) : base(stateMachine)
        {
            var width = _end.Left - _begin.Left;
            var height = _end.Top - _begin.Top;
            
            this.stateMachine = stateMachine;
            sr = new SpriteRenderer(DataManager.GetSpriteFilePath($"{stateMachine.GetName}_Die"));
            sr.Prepare(width, height);
        }

        public override void Enter() { }

        public override void Execute()
        {
            OutputStream.WriteBuffer(sr.GetFrame(), _begin, _end);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}
