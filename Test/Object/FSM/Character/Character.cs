using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test.Object;

namespace Test.FSM.Character
{
    public class Character : MonoBehaviour
    {
        protected Stat stat;
        protected IStateMachine stateMachine;
        public delegate bool GetBattleState();
        public GetBattleState isAttackSequence;
        public bool IsDie()
        {
            return stat.IsDie;
        }
        public Character(Stat stat)
        {


        }
        public override void Awake()
        {
            base.Awake();
        }
        public override void Destroy()
        {
            
        }
        public override void Update()
        {
            SMSequenceCheck();
        }
        public override void Start()
        {
            
        }
        public override void Enable()
        {
            
        }
        public override void Disable()
        {
            
        }
        public virtual void SMSequenceCheck()
        {
            if (isAttackSequence.Invoke()) stateMachine.StateChange(StateType.Attack);
            else stateMachine.StateChange(StateType.Idle);
        }
    }
}
