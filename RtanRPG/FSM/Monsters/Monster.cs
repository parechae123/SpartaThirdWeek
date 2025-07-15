using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM.Monsters
{
    //YOON : 추후 GameObject 받으면 상속필요
    class Monster
    {
        protected Stat stat;
        protected IStateMachine states;
        protected bool[,] patterns = new bool[200,3];
        public Monster(Stat stat , bool[,] patterns)
        {
            this.stat = stat;
            this.patterns = patterns;
            states = new MonsterStateMachine(stat);
        }
        
    }
    class BossMonster : Monster
    {

        public BossMonster(Stat stat, bool[,] patterns) : base(stat,patterns)
        {
            this.stat = stat;
            this.patterns = patterns;
            states = new MonsterStateMachine(stat);

        }
        private void Update()
        {

        }
    }
}
