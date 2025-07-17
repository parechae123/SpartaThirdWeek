

namespace RtanRPG.Object.Scene;

public class BattleScene : BaseScene
{
    int _index;
    public BattleScene(int index) : base(index)
    {
        _index = index;

        Commands[ConsoleKey.UpArrow] = UpArrow;
        Commands[ConsoleKey.DownArrow] = DownArrow;
        Commands[ConsoleKey.LeftArrow] = LeftArrow;
        Commands[ConsoleKey.RightArrow] = RightArrow;
    }
    LinkedList<ConsoleKey> patterns = new LinkedList<ConsoleKey>();
    public Action[] monsterReset;//패턴 전송용
    ushort goalPatternFrameCount = 30; //패턴 입력기한 10 == 1초
    ushort currPatternFrame = 0;
    ushort currWaitTime = 0;//공격,Idle 딜레이 10 == 1초
    ushort goalWaitTime = 20;//공격,Idle 딜레이 10 == 1초


    /// <summary>
    /// 판정때문에 업데이트가 불가피할 것 같아서 업데이트버전 작업입니다
    /// </summary>
    /// <returns></returns>
    public void Update()
    {
        if(currWaitTime < goalWaitTime)
        {
            currWaitTime++;
            return;
        }
        currPatternFrame++;
        if (goalPatternFrameCount <= currPatternFrame)
        {
            currPatternFrame = 0;
            currWaitTime = 0;

            if (patterns.Count > 0)//플레이어 데미지 처리

            PatternReset();
            for (int i = 0; i < monsterReset.Length; i++) monsterReset[i].Invoke();
        }
    }
    //혹시몰라서 테스크 버전도 준비해놨습니다
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
    }


    public void ReceiveMobPattern(ConsoleKey keys)
    {
        patterns.AddLast(keys);
    }
    public string PatternsToString()//혹시 이미지 네임 반환해야될 것 같아서 넣어뒀습니다, 아 그냥 배열이 나앗을려나
    {
        return string.Join(',', patterns);
    }
    public void PatternReset()
    {
        patterns.Clear();
    }

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
}