using RtanRPG.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.FSM.Charactors.Monsters
{
    //YOON : 추후 GameObject 받으면 상속필요
    class EnemyCharactor : Charactor
    {

        public EnemyCharactor(Stat stat, char[] keys) : base(stat,keys)
        {
            this.keys = keys;
            this.stat = stat;
            stateMachine = new MonsterStateMachine(stat);
            
        }
        //YOON : MonoBehavior 상속 시 override와 구문추가 필요
        public override void Awake(){}
        public override void Enable(){}
        public override void Start(){}
        public override void Update() 
        {
            stateMachine.GetCurrentState().Execute();
        }
        public override void Disable() { }
        public override void Destroy() { }
        public void SendAttackOrder()
        {
            SetAttackState();
            //keys[Random.Shared.Next(0, keys.Length)];//추후 BattleScene의 키값
            //SetIdleState()끝나는 이벤트 전송이 가능하면 이걸 보낸다
        }
        public void SetAttackState() { stateMachine.StateChange(StateType.Attack); }
        public void SetIdleState() { stateMachine.StateChange(StateType.Idle); }

        #region 미사용 함수 및 변수

        /*
         *         protected bool[,] patterns = new bool[200,3];
        protected char[] keys = new char[3];
        protected List<Queue<bool>> visualizedNote = new List<Queue<bool>>();

        protected int currIndex = 0;//판정의 기준이 되는 인덱스
        
        protected int createDelay;//노드 판정 이전 createDelay 만큼 빠르게 노드가 생성(판정 이전 보여주기 위함)
         * 
         * public virtual void CheckInput()//업데이트 실행,노트판별
                {
                    if (currIndex < 0 || currIndex<= patterns.Length) return;  //인덱스가 0 미만 혹은 
                    currIndex++;
                    bool noAttack = true;
                    for (int i = 0; i < patterns.GetLength(0); i++)
                    {
                        if (patterns[currIndex, i] *//*&& keys와 현재 인풋을 비교*//*)
                        {
                            noAttack = false;
                            stateMachine.StateChange(StateType.Attack);
                        }//YOON : 추후 UI 혹은 gameobject 추가시 
                    }

                    if (!noAttack)
                    {
                        stateMachine.StateChange(StateType.Idle);
                    }
                }

                public virtual void PrintNotes()//업데이트 실행
                {
                    if (visualizedNote.First().Count <= 0) return;//스텍 언더플로우 방지
                    if (currIndex < 0) currIndex++;

                    for (int i = 0; i < visualizedNote.Count; i++)
                    {
                        visualizedNote[i].Dequeue();//YOON : 추후 UI 혹은 gameobject 추가시 
                    }
                }
                protected void NoteVisualize()//Init용
                {

                    for (int i = 0; i < patterns.GetLength(0); i++)
                    {
                        for (int j = 0; j < patterns.GetLength(1); j++)
                        {
                            if (visualizedNote.Count <= i) visualizedNote.Add(new Queue<bool>());
                            visualizedNote[i].Enqueue(patterns[i, j]);
                        }
                    }
                }
                */
        #endregion
    }
    class BossEnemyCharactor : EnemyCharactor
    {

        public BossEnemyCharactor(Stat stat,char[] keys) : base(stat,keys)
        {
            this.keys = keys;
            this.stat = stat;
            stateMachine = new MonsterStateMachine(stat);

        }
    }
}
