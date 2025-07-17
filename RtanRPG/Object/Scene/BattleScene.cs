

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
    public Action[] monsterReset;//���� ���ۿ�
    ushort goalPatternFrameCount = 30; //���� �Է±��� 10 == 1��
    ushort currPatternFrame = 0;
    ushort currWaitTime = 0;//����,Idle ������ 10 == 1��
    ushort goalWaitTime = 20;//����,Idle ������ 10 == 1��


    /// <summary>
    /// ���������� ������Ʈ�� �Ұ����� �� ���Ƽ� ������Ʈ���� �۾��Դϴ�
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

            if (patterns.Count > 0)//�÷��̾� ������ ó��

            PatternReset();
            for (int i = 0; i < monsterReset.Length; i++) monsterReset[i].Invoke();
        }
    }
    //Ȥ�ø��� �׽�ũ ������ �غ��س����ϴ�
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
    public string PatternsToString()//Ȥ�� �̹��� ���� ��ȯ�ؾߵ� �� ���Ƽ� �־�׽��ϴ�, �� �׳� �迭�� ����������
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