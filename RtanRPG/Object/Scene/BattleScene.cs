

using RtanRPG.FSM.Charactors;
using RtanRPG.FSM.Charactors.Monsters;

namespace RtanRPG.Object.Scene;

public class BattleScene : BaseScene
{
    int _index;
    public BattleScene(int index) : base(index)
    {
        _index = index;

        
    }
    LinkedList<ConsoleKey> patterns = new LinkedList<ConsoleKey>();
    public delegate void GetMonsterPatern(LinkedList<ConsoleKey> keys);//패턴 전송용
    public GetMonsterPatern SetPattern;
    //플레이어 스텟 들고있기
    ushort goalPatternFrameCount = 30; //패턴 입력기한 10 == 1초
    ushort currPatternFrame = 0;
    ushort currWaitTime = 0;//공격,Idle 딜레이 10 == 1초
    ushort goalWaitTime = 20;//공격,Idle 딜레이 10 == 1초

    public override void Start()
    {
        base.Start();
        SetPattern = null;
        for (int i = 0; i < GameObjects.Count; i++)
        {
            
            if (GameObjects[i] is Charactor)
            {
                Charactor charactor = (Charactor)GameObjects[i];
                charactor.IsAttackSequence = IsAttackState;
                if (charactor is EnemyCharactor)
                {
                    EnemyCharactor enemy = (EnemyCharactor)GameObjects[i];
                    SetPattern += enemy.SetNode;
                }
            }
        }
    }
    public bool IsAttackState()
    {
        return currWaitTime < goalWaitTime;
    }
    /// <summary>
    /// 판정때문에 업데이트가 불가피할 것 같아서 업데이트버전 작업입니다
    /// </summary>
    /// <returns></returns>
    public void Update()
    {
        if(currWaitTime < goalWaitTime)
        {
            currWaitTime++;
            SetEvadeKey();
            return;
        }
        currPatternFrame++;
        if (goalPatternFrameCount <= currPatternFrame)
        {
            currPatternFrame = 0;
            currWaitTime = 0;

            //if (patterns.Count > 0)//플레이어 데미지 처리

            PatternReset();
            SetAttackKey();
            SetPattern?.Invoke(patterns);
        }
    }
/*    //혹시몰라서 테스크 버전도 준비해놨습니다
    public async Task<bool> IsPatternFailTask()
    {
        await Task.Delay(goalPatternFrameCount * 100);
        if (patterns.Count == 0)
        {
            PatternReset();
            return false;
        }
        else
        {
            PatternReset();
            return true;
        }
    }*/
    
    public void SetEvadeKey()
    {
        ResetArrowKey();
        Commands[ConsoleKey.UpArrow] = UpArrow;
        Commands[ConsoleKey.DownArrow] = DownArrow;
        Commands[ConsoleKey.LeftArrow] = LeftArrow;
        Commands[ConsoleKey.RightArrow] = RightArrow;
    }
    public void SetAttackKey()
    {
        //TODO : 기능 적용필요
        ResetArrowKey();
        /*Commands[ConsoleKey.UpArrow] = UpArrow;
        Commands[ConsoleKey.DownArrow] = DownArrow;
        Commands[ConsoleKey.LeftArrow] = LeftArrow;
        Commands[ConsoleKey.RightArrow] = RightArrow;*/
    }
    private void ResetArrowKey()
    {
        Commands[ConsoleKey.UpArrow] = null;
        Commands[ConsoleKey.DownArrow] = null;
        Commands[ConsoleKey.LeftArrow] = null;
        Commands[ConsoleKey.RightArrow] = null;
    }

    #region 키재지정
    public void ReceiveMobPattern(ConsoleKey keys)
    {
        patterns.AddLast(keys);
    }
    public string PatternsToString()//혹시 이미지 네임 반환해야될 것 같아서 넣어뒀습니다,
    {
        return $" {{{string.Join(',', patterns)}}}";
    }
    public void PatternReset()
    {
        patterns.Clear();
    }
    #endregion
    #region 키등록함수
    public void UpArrow()
    {
        if (patterns.Count <= 0) return;
        if (patterns.First() == ConsoleKey.UpArrow)
        {
            patterns.RemoveFirst();
        }
    }
    public void DownArrow()
    {
        if (patterns.Count <= 0) return;
        if (patterns.First() == ConsoleKey.DownArrow)
        {
            patterns.RemoveFirst();
        }
    }
    public void LeftArrow()
    {
        if (patterns.Count <= 0) return;
        if (patterns.First() == ConsoleKey.LeftArrow)
        {
            patterns.RemoveFirst();
        }
    }
    public void RightArrow()
    {
        if (patterns.Count <= 0) return;
        if (patterns.First() == ConsoleKey.RightArrow)
        {
            patterns.RemoveFirst();
        }
    }
    #endregion
}