using System.Text;
using Test.Data;
using Test.FSM.Character;
using Test.FSM.Character.Monsters;
using Test.Utils;
using Test.Utils.Console;
using Test.Utils.Extension;

namespace Test.Object.Scene;

public class BattleScene : BaseScene
{
    private bool isSceneChanging = false;  // 씬 전환 중복 방지용
    private bool _shouldGoToMain = false;

    int _index;
    int _monsterIndex;
    public BattleScene(int index) : base(index)
    {
        _index = 0;
        _monsterIndex = 0;
    }
    bool inTargetting; //Enum

    LinkedList<ConsoleKey> patterns = new LinkedList<ConsoleKey>();

    public delegate void GetMonsterPatternCallback(LinkedList<ConsoleKey> keys);
    public GetMonsterPatternCallback setPattern;//패턴을 수신하는 Delegate
    public Action<PlayerData> playerHit;

    public delegate bool IsDie();
    public IsDie die;

    public List<(Action<int>,IsDie,string)> monsterHit = new List<(Action<int>,IsDie, string)>();
    //플레이어 스텟 들고있기
    ushort goalPatternFrameCount = 40; //패턴 입력기한 10 == 1초
    ushort currPatternFrame = 0;
    ushort currWaitTime = 0;//공격,Idle 딜레이 10 == 1초
    ushort goalWaitTime = 30;//공격,Idle 딜레이 10 == 1초

    private readonly string[] _menus = { "Attack ", "Potion ", "Run " };
    private readonly string[] _selectedMenus = { "> Attack ", "> Potion ", "> Run " };
//    private SpriteRenderer[] arrows;
    public override void Start()
    {
        base.Start();

        isSceneChanging = false;
        _shouldGoToMain = false;
        /*        arrows = new[] {new SpriteRenderer(DataManager.GetSpriteFilePath("ArrowUp"))
                    , new SpriteRenderer((DataManager.GetSpriteFilePath("ArrowDown")))
                    , new SpriteRenderer((DataManager.GetSpriteFilePath("ArrowLeft")))
                    , new SpriteRenderer((DataManager.GetSpriteFilePath("ArrowRight")))};
                for (int i = 0; i < arrows.Length; i++) arrows[i].Prepare(20,20);*/

        _index = 0;
        _monsterIndex = 0;
        inTargetting = false;
        setPattern = null;
        monsterHit.Clear();
        playerHit = null;
        PatternReset();
        RegistGameObjects();
        for (int i = 0; i < GameObjects.Count; i++)
        {
            if (GameObjects[i] is Character)
            {
                Character charactor = (Character)GameObjects[i];
                charactor.isAttackSequence += IsAttackState;
                if (charactor is EnemyCharacter)
                {
                    EnemyCharacter enemy = (EnemyCharacter)GameObjects[i];
                    setPattern += enemy.SetNode;
                    playerHit += enemy.AttackPlayer;
                    monsterHit.Add((enemy.HitMonster,enemy.IsDie,enemy.GetName()));
                }
            }
        }
        SetAttackKey();
        while (!IsUnloaded)
        {
            foreach (MonoBehaviour item in GameObjects) item.Update();
            Update();
            Render();
        }
    }

    private void RegistGameObjects()
    {
        //TODO : 보스씬, 이미지 추가시 해당 함수 수정
        GameObjects.Add(new EnemyCharacter(new FSM.Stat(10, 10, 10, "DJ")));
        GameObjects.Add(new EnemyCharacter(new FSM.Stat(10, 10, 10, "DJ")));
        GameObjects.Add(new Director(null));
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
        if (inTargetting) return;
        if(currWaitTime <= goalWaitTime)
        {
            if(currWaitTime == goalWaitTime) 
            {
                setPattern?.Invoke(patterns);
                SetEvadeKey();
            }
            currWaitTime++;

            return;
        }
        currPatternFrame++;
        if (currPatternFrame >= goalPatternFrameCount || patterns.Count <= 0)
        {
            currPatternFrame = 0;
            currWaitTime = 0;

            if (patterns.Count > 0) playerHit.Invoke(DataManager.Instance.PlayerData);

            PatternReset();
            SetAttackKey();
            
        }
    }

    #region 렌더링
    public override void Render()
    {
        //OutputStream.WriteBuffer(_video.GetNextFrame(), _begin, _end);
        
        RenderPlayerUI();
        base.Render();
    }
    private void RenderPlayerUI()
    {

        if (inTargetting)
        {
            TargetingRender();
            return;
        }

        if (!IsAttackState())
        {
            NoteRender();
            Timer(goalPatternFrameCount, currPatternFrame, false);
        }
        else 
        {
            MenuRender();
            Timer(goalWaitTime, currWaitTime, true);
        }

    }

    private void MenuRender()
    {
        int len = Layout.MaximumContentHeight - (_selectedMenus.Length * 3);
        for (int i = 0; i < _selectedMenus.Length; i++)
        {
            if (_index == i)
            {
                OutputStream.WriteBuffer(
                    _selectedMenus[i],
                    new Vector2D(
                        Layout.MaximumContentWidth / 2 - _selectedMenus[i].GetGraphicLength() / 2 - 1, len + (i * 2)
                    )
                );
            }
            else
            {
                OutputStream.WriteBuffer(
                _menus[i],
                new Vector2D(Layout.MaximumContentWidth / 2 - _menus[i].GetGraphicLength() / 2, len + (i * 2)));
            }
        }
    }
    private void TargetingRender()
    {
        int len = Layout.MaximumContentHeight - (monsterHit.Count * 2)-2;
        for (int i = 0; i < monsterHit.Count; i++)
        {
            if (_monsterIndex == i)
            {
                string str = $" > {monsterHit[i].Item3} ";
                OutputStream.WriteBuffer(
                    str,
                    new Vector2D(
                        Layout.MaximumContentWidth / 2 - str.GetGraphicLength() / 2 - 1, len + (i * 2)
                    )
                );
            }
            else
            {
                string str = $" {monsterHit[i].Item3} ";
                OutputStream.WriteBuffer(
                str,
                new Vector2D(Layout.MaximumContentWidth / 2 - str.GetGraphicLength() / 2, len + (i * 2)));
            }
        }
        

        if (_monsterIndex == monsterHit.Count)
        {
            string exit = " > Return ";
            OutputStream.WriteBuffer(
            exit,
            new Vector2D(
                Layout.MaximumContentWidth / 2 - exit.GetGraphicLength() / 2 - 1, Layout.MaximumContentHeight-2
            ));
        }
        else
        {
            string exit = " Return ";
            OutputStream.WriteBuffer(
            exit,
            new Vector2D(Layout.MaximumContentWidth / 2 - exit.GetGraphicLength() / 2, Layout.MaximumContentHeight-2));
        }

    }

    private void NoteRender()
    {
        ConsoleKey[] keys = patterns.ToArray();
        StringBuilder str = new StringBuilder();
        for (int i = 0; i < keys.Length; i++)
        {
            str.Append(' ');
            switch (keys[i])
            {
                case ConsoleKey.UpArrow:
                    str.Append('↑');
                    break;
                case ConsoleKey.DownArrow:
                    str.Append('↓');
                    break;
                case ConsoleKey.LeftArrow:
                    str.Append('←');
                    break;
                case ConsoleKey.RightArrow:
                    str.Append('→');
                    break;
                default:
                    return;
            }
            str.Append(' ');
        }
        string result = str.ToString();
        OutputStream.WriteBuffer(result, new Vector2D(Layout.MaximumContentWidth / 2 - result.GetGraphicLength() / 2, Layout.MaximumContentHeight / 2));
        /*if (patterns.Count <= 0) return;
        ConsoleKey[] tempKeys = patterns.ToArray();
        string[] result;

        switch (tempKeys[0])
        {
            case ConsoleKey.UpArrow:
                result = arrows[0].GetFrame();
                break;
            case ConsoleKey.DownArrow:
                result = arrows[1].GetFrame();
                break;
            case ConsoleKey.LeftArrow:
                result = arrows[2].GetFrame();
                break;
            case ConsoleKey.RightArrow:
                result = arrows[3].GetFrame();
                break;
            default:
                return;
        }


        for (int i = 1; i < tempKeys.Length; i++)
        {
            string[] tempResult;
            switch (tempKeys[i])
            {
                case ConsoleKey.UpArrow:
                    tempResult = arrows[0].GetFrame();
                    break;
                case ConsoleKey.DownArrow:
                    tempResult = arrows[1].GetFrame();
                    break;
                case ConsoleKey.LeftArrow:
                    tempResult = arrows[2].GetFrame();
                    break;
                case ConsoleKey.RightArrow:
                    tempResult = arrows[3].GetFrame();
                    break;
                default:
                    return;
            }
            for (int j = 0; j < result.Length; j++)
            {
                result[j] += tempResult[j];
            }
        }
        if (result == null) return;
        OutputStream.WriteBuffer(result, 
            new Vector2D((Layout.MaximumContentWidth/2) - (result[0].Length/2),Layout.MaximumContentHeight- result.Length), 
            new Vector2D((Layout.MaximumContentWidth / 2) + (result[0].Length / 2), Layout.MaximumContentHeight-1));*/
    }
    private void Timer(int goal,int curr,bool attackState)
    {
        float g = goal;
        float c = curr;
        float per = (c / g)*10;
        int fullSquareMax = (int)MathF.Round(per);
        string r = new string(Enumerable.Repeat<char>('□', 10).Select((c, idx) => idx < per? '■':c).ToArray());
        string text = attackState ? " Select an action " : " Avoid enemy attacks! ";
        OutputStream.WriteBuffer(text, new Vector2D(Layout.MaximumContentWidth / 2 - text.GetGraphicLength() / 2, (Layout.MaximumContentHeight / 3)+1));
        OutputStream.WriteBuffer(r, new Vector2D(Layout.MaximumContentWidth / 2 - r.GetGraphicLength() / 2, Layout.MaximumContentHeight / 3));

    }
    #endregion

    #region 패턴관련함수
    public void ReceiveMobPattern(ConsoleKey keys)
    {
        patterns.AddLast(keys);
    }
    public void PatternReset()
    {
        patterns.Clear();
    }
    #endregion

    #region 키재지정
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
        ResetArrowKey();
        Commands[ConsoleKey.UpArrow] = SelectUpperMenu;
        Commands[ConsoleKey.DownArrow] = SelectLowerMenu;
        Commands[ConsoleKey.Z] = SelectMenu;
    }
    public void SetTargettingKey()
    {
        ResetArrowKey();
        Commands[ConsoleKey.UpArrow] = SelectUpperMonster;
        Commands[ConsoleKey.DownArrow] = SelectLowerMonster;
        Commands[ConsoleKey.Z] = SelectMonster;
    }
    private void ResetArrowKey()
    {
        Commands[ConsoleKey.UpArrow] = null;
        Commands[ConsoleKey.DownArrow] = null;
        Commands[ConsoleKey.LeftArrow] = null;
        Commands[ConsoleKey.RightArrow] = null;
        Commands[ConsoleKey.Z] = null;
    }

    #endregion

    #region 키기능함수
    private void UpArrow()
    {
        TryNote(ConsoleKey.UpArrow);
    }
    private void DownArrow()
    {
        TryNote(ConsoleKey.DownArrow);
    }
    private void LeftArrow()
    {
        TryNote(ConsoleKey.LeftArrow);
    }
    private void RightArrow()
    {
        TryNote(ConsoleKey.RightArrow);
    }
    private void TryNote(ConsoleKey key)
    {
        if (patterns.Count <= 0) return;
        if (patterns.First() == key) patterns.RemoveFirst();
        else FailPattern();
    }

    public void FailPattern() { currPatternFrame = goalPatternFrameCount; }


    private void SelectUpperMenu()
    {
        if (_index > 0)
        {
            _index--;
        }
    }

    private void SelectLowerMenu()
    {
        if (_index < _menus.Length-1)
        {
            _index++;
        }
    }

    private void SelectMenu()
    {
        switch (_index)
        {
            case 0:
                _monsterIndex = 0;
                inTargetting = true;
                SetTargettingKey();
                break;
            case 1:
                //아이템 사용로직
                break;
            case 2:
                if (!isSceneChanging)
                {
                    isSceneChanging = true;
                    IsUnloaded = true;
                    SceneManager.Instance.Index = 4;

                }
                break;
        }
    }

    private void SelectUpperMonster()
    {
        if (_monsterIndex > 0)
        {
            _monsterIndex--;
        }
    }

    private void SelectLowerMonster()
    {
        if (_monsterIndex < monsterHit.Count)
        {
            _monsterIndex++;
        }
    }

    private void SelectMonster()
    {

        if (_monsterIndex < monsterHit.Count)
        {
            monsterHit[_monsterIndex].Item1(DataManager.Instance.PlayerData.AttackPoint);
            if (monsterHit[_monsterIndex].Item2.Invoke()) monsterHit.RemoveAt(_monsterIndex);
            currWaitTime = goalWaitTime;
        }
        else
        {
            SetAttackKey();
        }
        inTargetting = false;

    }

    #endregion
}