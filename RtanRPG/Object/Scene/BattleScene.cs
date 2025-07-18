

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
    public delegate void GetMonsterPatern(LinkedList<ConsoleKey> keys);//���� ���ۿ�
    public GetMonsterPatern SetPattern;
    //�÷��̾� ���� ����ֱ�
    ushort goalPatternFrameCount = 30; //���� �Է±��� 10 == 1��
    ushort currPatternFrame = 0;
    ushort currWaitTime = 0;//����,Idle ������ 10 == 1��
    ushort goalWaitTime = 20;//����,Idle ������ 10 == 1��

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
    /// ���������� ������Ʈ�� �Ұ����� �� ���Ƽ� ������Ʈ���� �۾��Դϴ�
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

            //if (patterns.Count > 0)//�÷��̾� ������ ó��

            PatternReset();
            SetAttackKey();
            SetPattern?.Invoke(patterns);
        }
    }
/*    //Ȥ�ø��� �׽�ũ ������ �غ��س����ϴ�
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
        //TODO : ��� �����ʿ�
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

    #region Ű������
    public void ReceiveMobPattern(ConsoleKey keys)
    {
        patterns.AddLast(keys);
    }
    public string PatternsToString()//Ȥ�� �̹��� ���� ��ȯ�ؾߵ� �� ���Ƽ� �־�׽��ϴ�,
    {
        return $" {{{string.Join(',', patterns)}}}";
    }
    public void PatternReset()
    {
        patterns.Clear();
    }
    #endregion
    #region Ű����Լ�
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