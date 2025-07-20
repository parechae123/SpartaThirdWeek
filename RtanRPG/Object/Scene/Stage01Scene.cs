using RtanRPG.Utils;
using RtanRPG.Utils.Console;
using System;
using System.Collections.Generic;
using RtanRPG.Data;

namespace RtanRPG.Object.Scene
{
    public class Stage01Scene : BaseScene
    {
        private int Index;
        private int count = 10;
        private bool isBossRoom = false;
        private string[] map;   // 현재 맵 데이터
        private int mapX = 0;
        private int mapY = 0;

        private int? previousDirection = null;
        protected int enemyUpdateCount = 0;
        public event Action OnPlayerEnemyCollision;

        protected Vector2D enemyPosition = new Vector2D(22, 22);
        protected Vector2D playerPosition = new Vector2D(20, 20);

        protected Random random = new Random();

        // 생성자: BaseScene의 int index 생성자를 호출해야 컴파일 오류 없음
        public Stage01Scene(int index) : base(index)
        {
            this.Index = index;

            mapX = 1;
            mapY = 1;

            LoadMap(mapX, mapY); // 초기 맵 로딩
            playerPosition = new Vector2D(20, 20);

            InitializeCommands();
        }

        private string[] ResizeMapToConsoleSize(string[] originalMap, int targetWidth, int targetHeight)
        {
            int originalHeight = originalMap.Length;
            int originalWidth = originalMap[0].Length;

            string[] resizedMap = new string[targetHeight];

            for (int y = 0; y < targetHeight; y++)
            {
                int origY = y * originalHeight / targetHeight;
                char[] row = new char[targetWidth];

                for (int x = 0; x < targetWidth; x++)
                {
                    int origX = x * originalWidth / targetWidth;
                    row[x] = originalMap[origY][origX];
                }

                resizedMap[y] = new string(row);
            }

            return resizedMap;
        }

        private Vector2D GetRandomWalkablePosition()
        {
            while (true)
            {
                int x = random.Next(0, map[0].Length);
                int y = random.Next(0, map.Length);

                if (map[y][x] == '.' && !(x == playerPosition.Left && y == playerPosition.Top))
                    return new Vector2D(x, y);
            }
        }

        private void LoadMap(int x, int y)
        {
            int index;

            if (y == 0)
                index = x;
            else if (y == 1)
                index = 3 - x;
            else
                index = y * 2 + x;

            if (index < 0 || index >= DataManager.Instance.MapData.Count())
                return;

            isBossRoom = (index == 0);

            mapX = x;
            mapY = y;

            var originalMap = DataManager.Instance.MapData[index].Image;

            // 모니터 크기 혹은 콘솔 크기 등 환경에 맞춰 동적으로 크기 조절하는 부분
            int targetWidth = Layout.MaximumContentWidth;
            int targetHeight = Layout.MaximumContentHeight;

            map = ResizeMapToConsoleSize(originalMap, targetWidth, targetHeight);

            playerPosition = new Vector2D(
                Math.Clamp(playerPosition.Left, 0, map[0].Length - 1),
                Math.Clamp(playerPosition.Top, 0, map.Length - 1)
            );
            enemyPosition = new Vector2D(
                Math.Clamp(enemyPosition.Left, 0, map[0].Length - 1),
                Math.Clamp(enemyPosition.Top, 0, map.Length - 1)
            );

            enemyPosition = isBossRoom ? new Vector2D(114, 6) : GetRandomWalkablePosition();
        }

        private void TryMapTransition()
        {
            int maxY = map.Length;
            int maxX = map[0].Length;

            if (playerPosition.Top == 0 && mapY > 0 && IsWalkable(playerPosition))
            {
                LoadMap(mapX, mapY - 1);
                playerPosition = new Vector2D(playerPosition.Left, map.Length - 2);
            }
            else if (playerPosition.Top == maxY - 1 && mapY < 1 && IsWalkable(playerPosition))
            {
                LoadMap(mapX, mapY + 1);
                playerPosition = new Vector2D(playerPosition.Left, 1);
            }
            else if (playerPosition.Left == 0 && mapX > 0 && IsWalkable(playerPosition))
            {
                LoadMap(mapX - 1, mapY);
                playerPosition = new Vector2D(map[0].Length - 2, playerPosition.Top);
            }
            else if (playerPosition.Left == map[0].Length - 1 && mapX < 1 && IsWalkable(playerPosition))
            {
                LoadMap(mapX + 1, mapY);
                playerPosition = new Vector2D(1, playerPosition.Top);
            }
        }

        private bool IsWalkable(Vector2D position)
        {
            if (position.Top < 0 || position.Top >= map.Length ||
                position.Left < 0 || position.Left >= map[0].Length)
                return false;

            return map[position.Top][position.Left] == '.';
        }

        private int GetOppositeDirection(int dir)
        {
            return dir switch
            {
                0 => 1,
                1 => 0,
                2 => 3,
                3 => 2,
                _ => -1
            };
        }

        private void InitializeCommands()
        {
            Commands[ConsoleKey.UpArrow] = () =>
            {
                var newPos = playerPosition + new Vector2D(0, -1);
                if (IsWalkable(newPos))
                    playerPosition = newPos;
            };
            Commands[ConsoleKey.DownArrow] = () =>
            {
                var newPos = playerPosition + new Vector2D(0, 1);
                if (IsWalkable(newPos))
                    playerPosition = newPos;
            };
            Commands[ConsoleKey.LeftArrow] = () =>
            {
                var newPos = playerPosition + new Vector2D(-1, 0);
                if (IsWalkable(newPos))
                    playerPosition = newPos;
            };
            Commands[ConsoleKey.RightArrow] = () =>
            {
                var newPos = playerPosition + new Vector2D(1, 0);
                if (IsWalkable(newPos))
                    playerPosition = newPos;
            };
            Commands[ConsoleKey.X] = () => OpenMenu();
            Commands[ConsoleKey.Escape] = () => Reset();
        }

        protected void OpenMenu()
        {
            Console.WriteLine("메뉴가 열렸습니다.");
        }

        protected void UpdateEnemy()
        {
            if (isBossRoom) return;

            enemyUpdateCount++;

            if (enemyUpdateCount >= count)
            {
                enemyUpdateCount = 0;
                count = count == 10 ? 2 : 10;

                int dir;
                while (true)
                {
                    do
                    {
                        dir = random.Next(4);
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

                    var newPos = enemyPosition + offset;

                    if (newPos.Top >= 0 && newPos.Top < map.Length &&
                        newPos.Left >= 0 && newPos.Left < map[0].Length &&
                        IsWalkable(newPos))
                    {
                        enemyPosition = newPos;
                        break;
                    }
                }
            }
        }

        private void ChangeToBattleScene()
        {
            if (isBossRoom)
                SceneManager.Instance.Index = 0; // TODO: 보스 씬 추가 예정
            else
                SceneManager.Instance.Index = 2;
            IsUnloaded = true;
        }

        public override void Start()
        {
            base.Start();

            if (playerPosition == enemyPosition)
            {
                Vector2D[] dirs = {
                    new Vector2D(1, 0),
                    new Vector2D(0, 1),
                    new Vector2D(0, -1),
                    new Vector2D(-1, 0),
                };

                foreach (var d in dirs)
                {
                    var np = playerPosition + d;
                    if (IsWalkable(np) && np != enemyPosition)
                    {
                        playerPosition = np;
                        break;
                    }
                }
            }

            OnPlayerEnemyCollision += ChangeToBattleScene;

            while (!IsUnloaded)
            {
                UpdateEnemy();
                TryMapTransition();

                if (playerPosition == enemyPosition)
                {
                    OnPlayerEnemyCollision?.Invoke();
                }

                Render();

                System.Threading.Thread.Sleep(100);
            }
        }

        public override void Render()
        {
            base.Render();

            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    OutputStream.WriteBuffer(
                        map[y][x] == '.' ? " " : map[y][x].ToString(),
                        new Vector2D(x + Layout.DefaultLeftMargin, y + Layout.DefaultTopMargin - 2),
                        1);
                }
            }

            OutputStream.WriteBuffer("P", playerPosition + new Vector2D(Layout.DefaultLeftMargin, Layout.DefaultTopMargin - 2), 100);
            OutputStream.WriteBuffer(isBossRoom ? "B" : "E", enemyPosition + new Vector2D(Layout.DefaultLeftMargin, Layout.DefaultTopMargin - 2), 100);
        }
    }
}




























//using RtanRPG.Utils;
//using RtanRPG.Utils.Console;
//using System.ComponentModel.Design;
//using RtanRPG.Data;
//namespace RtanRPG.Object.Scene;

//public class Stage01Scene : BaseScene
//{
//    //ItemData 만들어야 하고 아이템 구현

//    private int Index;
//    int count = 10;
//    private bool isBossRoom = false;  // 현재 맵이 보스방인지 여부
//    private string[] map;   //맵 데이터 저장할 변수
//    private int mapX = 0;   // 현재 맵의 가로 위치 
//    private int mapY = 0;   // 현재 맵의 세로 위치

//    private int? previousDirection = null;                                  //직전에 움직였던 방향을 저장할 변수 (0~3: 상하좌우)
//    protected int enemyUpdateCount = 0;                                     
//    public event Action OnPlayerEnemyCollision;                             //플레이어와 적 충돌 이벤트생성

//    protected Vector2D enemyPosition = new Vector2D(22, 22);                //위치,상태변수들 위치는 임의지정
//    protected Vector2D playerPosition = new Vector2D(20, 20);

//    protected Random random = new Random();

//    private string[] ResizeMapToConsoleSize(string[] originalMap, int targetWidth, int targetHeight)        //맵데이터 리사이징 하는 함수
//    {
//        int originalHeight = originalMap.Length;
//        int originalWidth = originalMap[0].Length;

//        string[] resizedMap = new string[targetHeight];

//        for (int y = 0; y < targetHeight; y++)
//        {
//            int origY = y * originalHeight / targetHeight;
//            char[] row = new char[targetWidth];

//            for (int x = 0; x < targetWidth; x++)
//            {
//                int origX = x * originalWidth / targetWidth;
//                row[x] = originalMap[origY][origX];
//            }

//            resizedMap[y] = new string(row);
//        }

//        return resizedMap;
//    }

//    private Vector2D GetRandomWalkablePosition()                //맵 이동시 적 위치 랜덤생성
//    {
//        while (true)
//        {
//            int x = random.Next(0, map[0].Length);
//            int y = random.Next(0, map.Length);

//            if (map[y][x] == '.' && !(x == playerPosition.Left && y == playerPosition.Top))     //.위에만 생성
//                return new Vector2D(x, y);
//        }
//    }

//    private void LoadMap(int x, int y)                          //현재 좌표(mapX, mapY)에 맞는 맵을 불러오는 함수 + 맵 크기 리사이즈
//    {
//        int index;

//        if (y == 0)
//            index = x;  // 위쪽 맵: 0,1
//        else if (y == 1)
//            index = 3 - x;  // 아래쪽 맵: 2,3 대신 3,2로 뒤집음
//        else
//            index = y * 2 + x;

//        if (index < 0 || index >= DataManager.Instance.MapData.Count())
//            return;

//        isBossRoom = (index == 0); // 보스방 여부 설정//////////////////////////////////////

//        mapX = x;
//        mapY = y;
//        map = DataManager.Instance.MapData[index].Image;

//        var originalMap = DataManager.Instance.MapData[index].Image;

//        // 테두리 안쪽 크기 구하기 
//        int targetWidth = Layout.MaximumContentWidth;
//        int targetHeight = Layout.MaximumContentHeight;

//        // 맵 크기 맞게 바꾸기 (리사이즈)
//        map = ResizeMapToConsoleSize(originalMap, targetWidth, targetHeight);

//        // 플레이어, 적 위치도 맵 안에 들어오게 조절
//        playerPosition = new Vector2D(
//            Math.Clamp(playerPosition.Left, 0, map[0].Length - 1),
//            Math.Clamp(playerPosition.Top, 0, map.Length - 1)
//        );
//        enemyPosition = new Vector2D(
//            Math.Clamp(enemyPosition.Left, 0, map[0].Length - 1),
//            Math.Clamp(enemyPosition.Top, 0, map.Length - 1));

//        enemyPosition = isBossRoom ? new Vector2D(114,6) : GetRandomWalkablePosition(); //적 위치 랜덤생성 및 보스위치 고정

//    }

//    private void TryMapTransition()                                 // 플레이어가 맵 끝에 도달하면 다음 맵으로 전환하는 함수
//    {
//        int maxY = map.Length;
//        int maxX = map[0].Length;

//        // 위쪽 끝
//        if (playerPosition.Top == 0 && mapY > 0 && IsWalkable(playerPosition))
//        {
//            LoadMap(mapX, mapY - 1);
//            playerPosition = new Vector2D(playerPosition.Left, map.Length - 2); // 맵 아래쪽으로 등장
//        }
//        // 아래쪽 끝
//        else if (playerPosition.Top == maxY - 1 && mapY < 1 && IsWalkable(playerPosition))
//        {
//            LoadMap(mapX, mapY + 1);
//            playerPosition = new Vector2D(playerPosition.Left, 1); // 맵 위쪽으로 등장
//        }
//        // 왼쪽 끝
//        else if (playerPosition.Left == 0 && mapX > 0 && IsWalkable(playerPosition))
//        {
//            LoadMap(mapX - 1, mapY);
//            playerPosition = new Vector2D(map[0].Length - 2, playerPosition.Top); // 오른쪽에서 등장
//        }
//        // 오른쪽 끝
//        else if (playerPosition.Left == map[0].Length - 1 && mapX < 1 && IsWalkable(playerPosition))
//        {
//            LoadMap(mapX + 1, mapY);
//            playerPosition = new Vector2D(1, playerPosition.Top); // 왼쪽에서 등장
//        }
//    }

//    private bool IsWalkable(Vector2D position)                              //이동하는 곳이 "."인지 확인하는 함수
//    {
//        if (position.Top < 0 || position.Top >= map.Length || 
//            position.Left < 0 || position.Left >= map[0].Length)
//            return false;

//        return map[position.Top][position.Left] == '.';
//    }

//    public Stage01Scene(int index) : base(index)                            //생성자
//    {
//        this.Index = index; // 저장

//        mapX = 1;
//        mapY = 1;

//        LoadMap(mapX, mapY); // index 2번 맵 로딩
//        playerPosition = new Vector2D(20, 20);

//        InitializeCommands();                                              
//    }

//    private int GetOppositeDirection(int dir)                               //반대방향 구해주는 함수 (적 같은방향 방지)
//    {
//        return dir switch
//        {
//            0 => 1, // 위 → 아래
//            1 => 0, // 아래 → 위
//            2 => 3, // 왼쪽 → 오른쪽
//            3 => 2, // 오른쪽 → 왼쪽
//            _ => -1
//        };
//    }

//    private void InitializeCommands()                                       //플레이어 조작 방향키 이동 등등
//    {
//        Commands[ConsoleKey.UpArrow] = () =>
//        {
//            var newPos = playerPosition + new Vector2D(0, -1);
//            if (IsWalkable(newPos))
//                playerPosition = newPos;
//        };
//        Commands[ConsoleKey.DownArrow] = () =>
//        {
//            var newPos = playerPosition + new Vector2D(0, 1);
//            if (IsWalkable(newPos))
//                playerPosition = newPos;
//        };
//        Commands[ConsoleKey.LeftArrow] = () =>
//        {
//            var newPos = playerPosition + new Vector2D(-1, 0);
//            if (IsWalkable(newPos))
//                playerPosition = newPos;
//        };
//        Commands[ConsoleKey.RightArrow] = () =>
//        {
//            var newPos = playerPosition + new Vector2D(1, 0);
//            if (IsWalkable(newPos))
//                playerPosition = newPos;
//        };
//        Commands[ConsoleKey.X] = () => OpenMenu();
//        Commands[ConsoleKey.Escape] = () => Reset();
//    }

//    protected void OpenMenu()                                               // 메뉴 열기 기능만 구현
//    {
//        Console.WriteLine("메뉴가 열렸습니다.");
//    }

//    protected void UpdateEnemy()                                            //적 랜덤 이동 함수
//    {
//        if (isBossRoom) return; // 보스맵일 경우 적 이동 금지

//        enemyUpdateCount++;

//        if (enemyUpdateCount >= count)
//        {
//            enemyUpdateCount = 0;
//            count = count == 10 ? 2 : 10;

//            int dir;
//            while (true)
//            {
//                do                                                          // 무작위 방향 선택, 단 직전 반대 방향은 제외
//                {
//                    dir = random.Next(4);                                   
//                }
//                while (previousDirection.HasValue && dir == GetOppositeDirection(previousDirection.Value));
//                previousDirection = dir;

//                Vector2D offset = dir switch
//                {
//                    0 => new Vector2D(0, -1),                               // 0: 위, 1: 아래, 2: 왼쪽, 3: 오른쪽
//                    1 => new Vector2D(0, 1),
//                    2 => new Vector2D(-1, 0),
//                    3 => new Vector2D(1, 0),
//                    _ => new Vector2D(0, 0)
//                };

//                var newPos = enemyPosition + offset;                        

//                if (newPos.Top >= 0 && newPos.Top < map.Length &&     // 범위 체크 + 맵 데이터 체크 ('.'만 이동 가능)
//                    newPos.Left >= 0 && newPos.Left < map[0].Length &&
//                    IsWalkable(newPos))
//                {
//                    enemyPosition = newPos;
//                    break;
//                }
//                else continue;
//            }
//        }
//    }

//    private void ChangeToBattleScene()                                      //이벤트 발생했을때 실행할 함수 (배틀씬 전환)
//    {
//        if (isBossRoom)
//            SceneManager.Instance.Index = 0; // TODO: 나중에 보스씬 넣어줘야함
//        else
//            SceneManager.Instance.Index = 2;
//        IsUnloaded = true; // 현재 씬 종료
//    }

//    public override void Start()
//    {
//        base.Start();

//        if (playerPosition == enemyPosition)        //위치 겹치면 이동가능한곳으로 이동
//        {
//            Vector2D[] dirs = {
//            new Vector2D(1, 0),  // 오른쪽
//            new Vector2D(0, 1),  // 아래
//            new Vector2D(0, -1), // 위
//            new Vector2D(-1, 0), // 왼쪽
//        };

//            foreach (var d in dirs)
//            {
//                var np = playerPosition + d;
//                if (IsWalkable(np) && np != enemyPosition)
//                {
//                    playerPosition = np;
//                    break;
//                }
//            }
//        }
//            OnPlayerEnemyCollision += ChangeToBattleScene;                      //이벤트 발생 후 실행할 함수등록

//        while (IsUnloaded == false)
//        {
//            UpdateEnemy();
//            TryMapTransition();

//            if (playerPosition == enemyPosition)                            //플레이어와 적 위치가 같으면 이벤트 발생
//            {
//                OnPlayerEnemyCollision?.Invoke();
//            }

//            Render();

//            Thread.Sleep(100);
//        }
//    }

//    public override void Render()                                          // 콘솔 화면에 맵과 캐릭터를 출력하는 함수  
//    {
//        base.Render();

//        for (int y = 0; y < map.Length; y++)
//        {
//            for (int x = 0; x < map[y].Length; x++)
//            {
//                OutputStream.WriteBuffer(
//                    map[y][x] == '.' ? " " : map[y][x].ToString(),
//                    new Vector2D(x + Layout.DefaultLeftMargin, y + Layout.DefaultTopMargin - 2),
//                    1
//                );
//            }
//        }
//        OutputStream.WriteBuffer("P", playerPosition + new Vector2D(Layout.DefaultLeftMargin, Layout.DefaultTopMargin - 2), 100);
//        OutputStream.WriteBuffer(isBossRoom ? "B" : "E",enemyPosition + new Vector2D(Layout.DefaultLeftMargin, Layout.DefaultTopMargin - 2),100); /////
//    }
//}