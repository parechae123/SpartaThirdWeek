using Test.Utils;
using Test.Utils.Console;

namespace Test.Object.Scene
{
    public class StageScene : BaseScene
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
        public StageScene(int index) : base(index)
        {
            this.Index = index;

            mapX = 1;
            mapY = 1;

            LoadMap(mapX, mapY); // 초기 맵 로딩
            playerPosition = new Vector2D(20, 20);

            InitializeCommands();
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
                    if (IsWalkable(np) == false || np == enemyPosition)
                    {
                        continue;
                    }
                    
                    playerPosition = np;
                    break;
                }
            }

            OnPlayerEnemyCollision += ChangeToBattleScene;

            while (IsUnloaded == false)
            {
                GetRandomWalkablePosition();
                
                UpdateEnemy();
                TryMapTransition();

                if (playerPosition == enemyPosition)
                {
                    OnPlayerEnemyCollision?.Invoke();
                }

                Render();

                Thread.Sleep(100);
            }
        }

        public override void Render()
        {
            for (var y = 0; y < map.Length; y++)
            {
                for (var x = 0; x < map[y].Length; x++)
                {
                    OutputStream.WriteBuffer(
                        map[y][x] == '.' ? " " : map[y][x].ToString(),
                        new Vector2D(x + Layout.DefaultLeftMargin, y + Layout.DefaultTopMargin - 2),
                        1);
                }
            }

            OutputStream.WriteBuffer("P", playerPosition + new Vector2D(Layout.DefaultLeftMargin, Layout.DefaultTopMargin - 2), 100);
            OutputStream.WriteBuffer(isBossRoom ? "B" : "E", enemyPosition + new Vector2D(Layout.DefaultLeftMargin, Layout.DefaultTopMargin - 2), 100);
            
            base.Render();
        }

        private string[] ResizeMapToConsoleSize(string[] originalMap, int targetWidth, int targetHeight)
        {
            var originalHeight = originalMap.Length;
            var originalWidth = originalMap[0].Length;

            var resizedMap = new string[targetHeight];

            for (var y = 0; y < targetHeight; y++)
            {
                var origY = y * originalHeight / targetHeight;
                var row = new char[targetWidth];

                for (var x = 0; x < targetWidth; x++)
                {
                    var origX = x * originalWidth / targetWidth;
                    row[x] = originalMap[origY][origX];
                }

                resizedMap[y] = new string(row);
            }

            return resizedMap;
        }

        private Vector2D GetRandomWalkablePosition()
        {
            var x = random.Next(0, map[0].Length);
            var y = random.Next(0, map.Length);

            if (map[y][x] == '.' && !(x == playerPosition.Left && y == playerPosition.Top))
                return new Vector2D(x, y);
            
            return new Vector2D(playerPosition.Left, playerPosition.Top);
        }

        private void LoadMap(int x, int y)
        {
            int index;
            switch (y)
            {
                case 0:
                    index = x;
                    break;
                case 1:
                    index = 3 - x;
                    break;
                default:
                    index = y * 2 + x;
                    break;
            }

            if (index < 0 || index >= DataManager.Instance.MapData.Count())
                return;

            isBossRoom = index == 0;

            mapX = x;
            mapY = y;

            var originalMap = DataManager.Instance.MapData[index].Image;

            // 모니터 크기 혹은 콘솔 크기 등 환경에 맞춰 동적으로 크기 조절하는 부분
            var targetWidth = Layout.MaximumContentWidth;
            var targetHeight = Layout.MaximumContentHeight;

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

                while (true)
                {
                    int dir;
                    do
                    {
                        dir = random.Next(4);
                    }
                    while (previousDirection.HasValue && dir == GetOppositeDirection(previousDirection.Value));
                    previousDirection = dir;

                    var offset = dir switch
                                 {
                                     0 => new Vector2D(0, -1),
                                     1 => new Vector2D(0, 1),
                                     2 => new Vector2D(-1, 0),
                                     3 => new Vector2D(1, 0),
                                     _ => new Vector2D(0, 0)
                                 };

                    var newPos = enemyPosition + offset;

                    if (newPos.Top < 0 || newPos.Top >= map.Length ||
                        newPos.Left < 0 || newPos.Left >= map[0].Length ||
                        IsWalkable(newPos) == false)
                    {
                        continue;
                    }
                    enemyPosition = newPos;
                    break;
                }
            }
        }

        private void ChangeToBattleScene()
        {
            SceneManager.Instance.Index = isBossRoom ? 4 : 4;
            
            IsUnloaded = true;
        }
    }
}