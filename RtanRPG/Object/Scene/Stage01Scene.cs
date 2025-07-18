using RtanRPG.Utils;

namespace RtanRPG.Object.Scene;

public class Stage01Scene : BaseScene
{
    private Dictionary<ConsoleKey, Action> CommandsMap = new Dictionary<ConsoleKey, Action>();
    private int? previousDirection = null;                                  //������ �������� ������ ������ ���� (0~3: �����¿�)
    int count = 10;
    protected Vector2D enemyPosition = new Vector2D(0, 0);                  //��ġ,���º����� ��ġ�� ��������
    protected int enemyUpdateCount = 0;
    protected Vector2D playerPosition = new Vector2D(5, 5);

    protected Random random = new Random();

    public Stage01Scene(int index) : base(index)
    {
        
    }

    private int GetOppositeDirection(int dir)                               //�ݴ���� �����ִ� �Լ�
    {
        return dir switch
        {
            0 => 1, // �� �� �Ʒ�
            1 => 0, // �Ʒ� �� ��
            2 => 3, // ���� �� ������
            3 => 2, // ������ �� ����
            _ => -1
        };
    }

    private void InitializeCommands()                                       //�÷��̾� ��ɾ����Լ�
    {
        CommandsMap[ConsoleKey.UpArrow] = () => playerPosition += new Vector2D(0, -1);
        CommandsMap[ConsoleKey.DownArrow] = () => playerPosition += new Vector2D(0, 1);
        CommandsMap[ConsoleKey.LeftArrow] = () => playerPosition += new Vector2D(-1, 0);
        CommandsMap[ConsoleKey.RightArrow] = () => playerPosition += new Vector2D(1, 0);
        CommandsMap[ConsoleKey.X] = () => OpenMenu();
    }
    
    protected void OpenMenu()                                               // �޴� ���� ��ɸ� ����
    {
        Console.WriteLine("�޴��� ���Ƚ��ϴ�.");
    }

    protected void UpdateEnemy()                                            //�� ���� �̵� �Լ�
    {
        enemyUpdateCount++;

        if (enemyUpdateCount >= count)
        {
            enemyUpdateCount = 0;
            count = count == 10 ? 2 : 10;

            int dir;
            do                                                          // ������ ���� ����, �� ���� �ݴ� ������ ����
            {
                dir = random.Next(4); // 0: ��, 1: �Ʒ�, 2: ����, 3: ������
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
//    InitializeCommands();                                              //base�� ������ ��ɾ����Լ� ����  ������� �̵���

//                if (CommandsMap.ContainsKey(key))                   //execute Ű�Է� ó��
//                CommandsMap[key].Invoke();

//    var inputManager = new InputManager();              // start InputManager ���� �� �� �̵��Լ� �ݺ�����
//    inputManager.InputCallback = key => Execute(key);
//    inputManager.Start();

//            while (!IsReset)
//            {
//                UpdateEnemy();
//    Render();

//    Thread.Sleep(100);
//            }

//Console.WriteLine($"�� ��ġ: ({enemyPosition.Left}, {enemyPosition.Top})");            //Render ��ġ�����ֱ�        
//            Console.WriteLine($"�÷��̾� ��ġ: ({playerPosition.Left}, {playerPosition.Top})");

}