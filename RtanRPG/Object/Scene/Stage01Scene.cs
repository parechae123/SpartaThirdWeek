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
        private string[] map;   // ���� �� ������
        private int mapX = 0;
        private int mapY = 0;

        private int? previousDirection = null;
        protected int enemyUpdateCount = 0;
        public event Action OnPlayerEnemyCollision;

        protected Vector2D enemyPosition = new Vector2D(22, 22);
        protected Vector2D playerPosition = new Vector2D(20, 20);

        protected Random random = new Random();

        // ������: BaseScene�� int index �����ڸ� ȣ���ؾ� ������ ���� ����
        public Stage01Scene(int index) : base(index)
        {
            this.Index = index;

            mapX = 1;
            mapY = 1;

            LoadMap(mapX, mapY); // �ʱ� �� �ε�
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

            // ����� ũ�� Ȥ�� �ܼ� ũ�� �� ȯ�濡 ���� �������� ũ�� �����ϴ� �κ�
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
            Console.WriteLine("�޴��� ���Ƚ��ϴ�.");
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
                SceneManager.Instance.Index = 0; // TODO: ���� �� �߰� ����
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
//    //ItemData ������ �ϰ� ������ ����

//    private int Index;
//    int count = 10;
//    private bool isBossRoom = false;  // ���� ���� ���������� ����
//    private string[] map;   //�� ������ ������ ����
//    private int mapX = 0;   // ���� ���� ���� ��ġ 
//    private int mapY = 0;   // ���� ���� ���� ��ġ

//    private int? previousDirection = null;                                  //������ �������� ������ ������ ���� (0~3: �����¿�)
//    protected int enemyUpdateCount = 0;                                     
//    public event Action OnPlayerEnemyCollision;                             //�÷��̾�� �� �浹 �̺�Ʈ����

//    protected Vector2D enemyPosition = new Vector2D(22, 22);                //��ġ,���º����� ��ġ�� ��������
//    protected Vector2D playerPosition = new Vector2D(20, 20);

//    protected Random random = new Random();

//    private string[] ResizeMapToConsoleSize(string[] originalMap, int targetWidth, int targetHeight)        //�ʵ����� ������¡ �ϴ� �Լ�
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

//    private Vector2D GetRandomWalkablePosition()                //�� �̵��� �� ��ġ ��������
//    {
//        while (true)
//        {
//            int x = random.Next(0, map[0].Length);
//            int y = random.Next(0, map.Length);

//            if (map[y][x] == '.' && !(x == playerPosition.Left && y == playerPosition.Top))     //.������ ����
//                return new Vector2D(x, y);
//        }
//    }

//    private void LoadMap(int x, int y)                          //���� ��ǥ(mapX, mapY)�� �´� ���� �ҷ����� �Լ� + �� ũ�� ��������
//    {
//        int index;

//        if (y == 0)
//            index = x;  // ���� ��: 0,1
//        else if (y == 1)
//            index = 3 - x;  // �Ʒ��� ��: 2,3 ��� 3,2�� ������
//        else
//            index = y * 2 + x;

//        if (index < 0 || index >= DataManager.Instance.MapData.Count())
//            return;

//        isBossRoom = (index == 0); // ������ ���� ����//////////////////////////////////////

//        mapX = x;
//        mapY = y;
//        map = DataManager.Instance.MapData[index].Image;

//        var originalMap = DataManager.Instance.MapData[index].Image;

//        // �׵θ� ���� ũ�� ���ϱ� 
//        int targetWidth = Layout.MaximumContentWidth;
//        int targetHeight = Layout.MaximumContentHeight;

//        // �� ũ�� �°� �ٲٱ� (��������)
//        map = ResizeMapToConsoleSize(originalMap, targetWidth, targetHeight);

//        // �÷��̾�, �� ��ġ�� �� �ȿ� ������ ����
//        playerPosition = new Vector2D(
//            Math.Clamp(playerPosition.Left, 0, map[0].Length - 1),
//            Math.Clamp(playerPosition.Top, 0, map.Length - 1)
//        );
//        enemyPosition = new Vector2D(
//            Math.Clamp(enemyPosition.Left, 0, map[0].Length - 1),
//            Math.Clamp(enemyPosition.Top, 0, map.Length - 1));

//        enemyPosition = isBossRoom ? new Vector2D(114,6) : GetRandomWalkablePosition(); //�� ��ġ �������� �� ������ġ ����

//    }

//    private void TryMapTransition()                                 // �÷��̾ �� ���� �����ϸ� ���� ������ ��ȯ�ϴ� �Լ�
//    {
//        int maxY = map.Length;
//        int maxX = map[0].Length;

//        // ���� ��
//        if (playerPosition.Top == 0 && mapY > 0 && IsWalkable(playerPosition))
//        {
//            LoadMap(mapX, mapY - 1);
//            playerPosition = new Vector2D(playerPosition.Left, map.Length - 2); // �� �Ʒ������� ����
//        }
//        // �Ʒ��� ��
//        else if (playerPosition.Top == maxY - 1 && mapY < 1 && IsWalkable(playerPosition))
//        {
//            LoadMap(mapX, mapY + 1);
//            playerPosition = new Vector2D(playerPosition.Left, 1); // �� �������� ����
//        }
//        // ���� ��
//        else if (playerPosition.Left == 0 && mapX > 0 && IsWalkable(playerPosition))
//        {
//            LoadMap(mapX - 1, mapY);
//            playerPosition = new Vector2D(map[0].Length - 2, playerPosition.Top); // �����ʿ��� ����
//        }
//        // ������ ��
//        else if (playerPosition.Left == map[0].Length - 1 && mapX < 1 && IsWalkable(playerPosition))
//        {
//            LoadMap(mapX + 1, mapY);
//            playerPosition = new Vector2D(1, playerPosition.Top); // ���ʿ��� ����
//        }
//    }

//    private bool IsWalkable(Vector2D position)                              //�̵��ϴ� ���� "."���� Ȯ���ϴ� �Լ�
//    {
//        if (position.Top < 0 || position.Top >= map.Length || 
//            position.Left < 0 || position.Left >= map[0].Length)
//            return false;

//        return map[position.Top][position.Left] == '.';
//    }

//    public Stage01Scene(int index) : base(index)                            //������
//    {
//        this.Index = index; // ����

//        mapX = 1;
//        mapY = 1;

//        LoadMap(mapX, mapY); // index 2�� �� �ε�
//        playerPosition = new Vector2D(20, 20);

//        InitializeCommands();                                              
//    }

//    private int GetOppositeDirection(int dir)                               //�ݴ���� �����ִ� �Լ� (�� �������� ����)
//    {
//        return dir switch
//        {
//            0 => 1, // �� �� �Ʒ�
//            1 => 0, // �Ʒ� �� ��
//            2 => 3, // ���� �� ������
//            3 => 2, // ������ �� ����
//            _ => -1
//        };
//    }

//    private void InitializeCommands()                                       //�÷��̾� ���� ����Ű �̵� ���
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

//    protected void OpenMenu()                                               // �޴� ���� ��ɸ� ����
//    {
//        Console.WriteLine("�޴��� ���Ƚ��ϴ�.");
//    }

//    protected void UpdateEnemy()                                            //�� ���� �̵� �Լ�
//    {
//        if (isBossRoom) return; // �������� ��� �� �̵� ����

//        enemyUpdateCount++;

//        if (enemyUpdateCount >= count)
//        {
//            enemyUpdateCount = 0;
//            count = count == 10 ? 2 : 10;

//            int dir;
//            while (true)
//            {
//                do                                                          // ������ ���� ����, �� ���� �ݴ� ������ ����
//                {
//                    dir = random.Next(4);                                   
//                }
//                while (previousDirection.HasValue && dir == GetOppositeDirection(previousDirection.Value));
//                previousDirection = dir;

//                Vector2D offset = dir switch
//                {
//                    0 => new Vector2D(0, -1),                               // 0: ��, 1: �Ʒ�, 2: ����, 3: ������
//                    1 => new Vector2D(0, 1),
//                    2 => new Vector2D(-1, 0),
//                    3 => new Vector2D(1, 0),
//                    _ => new Vector2D(0, 0)
//                };

//                var newPos = enemyPosition + offset;                        

//                if (newPos.Top >= 0 && newPos.Top < map.Length &&     // ���� üũ + �� ������ üũ ('.'�� �̵� ����)
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

//    private void ChangeToBattleScene()                                      //�̺�Ʈ �߻������� ������ �Լ� (��Ʋ�� ��ȯ)
//    {
//        if (isBossRoom)
//            SceneManager.Instance.Index = 0; // TODO: ���߿� ������ �־������
//        else
//            SceneManager.Instance.Index = 2;
//        IsUnloaded = true; // ���� �� ����
//    }

//    public override void Start()
//    {
//        base.Start();

//        if (playerPosition == enemyPosition)        //��ġ ��ġ�� �̵������Ѱ����� �̵�
//        {
//            Vector2D[] dirs = {
//            new Vector2D(1, 0),  // ������
//            new Vector2D(0, 1),  // �Ʒ�
//            new Vector2D(0, -1), // ��
//            new Vector2D(-1, 0), // ����
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
//            OnPlayerEnemyCollision += ChangeToBattleScene;                      //�̺�Ʈ �߻� �� ������ �Լ����

//        while (IsUnloaded == false)
//        {
//            UpdateEnemy();
//            TryMapTransition();

//            if (playerPosition == enemyPosition)                            //�÷��̾�� �� ��ġ�� ������ �̺�Ʈ �߻�
//            {
//                OnPlayerEnemyCollision?.Invoke();
//            }

//            Render();

//            Thread.Sleep(100);
//        }
//    }

//    public override void Render()                                          // �ܼ� ȭ�鿡 �ʰ� ĳ���͸� ����ϴ� �Լ�  
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