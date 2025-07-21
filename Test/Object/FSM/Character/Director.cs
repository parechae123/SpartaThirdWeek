using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.FSM.Character
{
    class Director : Character
    {
        public string GetName() => stat.Name;
        public Director(Stat stat) : base(stat)
        {
            stateMachine = new PlayerStateMachine();

        }
        //YOON : MonoBehavior 상속 시 override와 구문추가 필요
        public override void Awake() { }
        public override void Enable() { }
        public override void Start() { }
        public override void Update()
        {
            SMSequenceCheck();
            stateMachine.GetCurrentState().Execute();
        }
        public override void Disable() { }
        public override void Destroy() { }
        public override void SMSequenceCheck()
        {
            if (!isAttackSequence.Invoke()) stateMachine.StateChange(StateType.Idle);
            else stateMachine.StateChange(StateType.Attack);
        }
    }
}
