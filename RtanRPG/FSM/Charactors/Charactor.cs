using RtanRPG.Object;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM.Charactors
{
    class Charactor : MonoBehavior
    {
        protected Stat stat;
        protected IStateMachine stateMachine;
        public char[] keys;

        public Charactor(Stat stat, char[] keys)
        {
            this.keys = keys;
            this.stat = stat;
            stateMachine = new MonsterStateMachine(stat);

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
    }
}
