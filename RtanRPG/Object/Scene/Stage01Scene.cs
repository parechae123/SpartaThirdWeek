using RtanRPG.Utils;

namespace RtanRPG.Object.Scene;

public class Stage01Scene : BaseScene
{
    private Dictionary<ConsoleKey, Action> CommandsMap = new Dictionary<ConsoleKey, Action>();
    private int? previousDirection = null;                                  //직전에 움직였던 방향을 저장할 변수 (0~3: 상하좌우)
    int count = 10;
    protected Vector2D enemyPosition = new Vector2D(0, 0);                  //위치,상태변수들 위치는 임의지정
    protected int enemyUpdateCount = 0;
    protected Vector2D playerPosition = new Vector2D(5, 5);

    protected Random random = new Random();

    public Stage01Scene(int index) : base(index)
    {
        
    }

    private int GetOppositeDirection(int dir)                               //반대방향 구해주는 함수
    {
        return dir switch
        {
            0 => 1, // 위 → 아래
            1 => 0, // 아래 → 위
            2 => 3, // 왼쪽 → 오른쪽
            3 => 2, // 오른쪽 → 왼쪽
            _ => -1
        };
    }

    private void InitializeCommands()                                       //플레이어 명령어등록함수
    {
        CommandsMap[ConsoleKey.UpArrow] = () => playerPosition += new Vector2D(0, -1);
        CommandsMap[ConsoleKey.DownArrow] = () => playerPosition += new Vector2D(0, 1);
        CommandsMap[ConsoleKey.LeftArrow] = () => playerPosition += new Vector2D(-1, 0);
        CommandsMap[ConsoleKey.RightArrow] = () => playerPosition += new Vector2D(1, 0);
        CommandsMap[ConsoleKey.X] = () => OpenMenu();
    }
    
    protected void OpenMenu()                                               // 메뉴 열기 기능만 구현
    {
        Console.WriteLine("메뉴가 열렸습니다.");
    }

    protected void UpdateEnemy()                                            //적 랜덤 이동 함수
    {
        enemyUpdateCount++;

        if (enemyUpdateCount >= count)
        {
            enemyUpdateCount = 0;
            count = count == 10 ? 2 : 10;

            int dir;
            do                                                          // 무작위 방향 선택, 단 직전 반대 방향은 제외
            {
                dir = random.Next(4); // 0: 위, 1: 아래, 2: 왼쪽, 3: 오른쪽
            }
            while (previousDirection.HasValue && dir == GetOppositeDirection(previousDirection.Value));
            previousDirection = dir;

            Vector2D offset = dir switch
            {
                0 => new Vector2D(0, -1),
                1 => new Vector2D(0, 1),
                2 => new Vector2D(-1, 0),
                3 => new Vector2D(1, 0),
                _ => new Vector2D(0, 0)
            };

            enemyPosition += offset;
        }
    }
//    InitializeCommands();                                              //base씬 생성자 명령어등록함수 실행  여기부터 이따가

//                if (CommandsMap.ContainsKey(key))                   //execute 키입력 처리
//                CommandsMap[key].Invoke();

//    var inputManager = new InputManager();              // start InputManager 세팅 및 적 이동함수 반복실행
//    inputManager.InputCallback = key => Execute(key);
//    inputManager.Start();

//            while (!IsReset)
//            {
//                UpdateEnemy();
//    Render();

//    Thread.Sleep(100);
//            }

//Console.WriteLine($"적 위치: ({enemyPosition.Left}, {enemyPosition.Top})");            //Render 위치보여주기        
//            Console.WriteLine($"플레이어 위치: ({playerPosition.Left}, {playerPosition.Top})");

}