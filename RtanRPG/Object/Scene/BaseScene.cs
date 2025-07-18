using RtanRPG.Utils;
using RtanRPG.Utils.Console;

namespace RtanRPG.Object.Scene
{
    public abstract class BaseScene : ICommandable, Iterable, IRenderable
    {
        protected BaseScene(int index)
        {
            var data = DataManager.Instance.SceneData[index];

            GameObjects = new List<MonoBehaviour>();
            Iterators = new Iterable[data.Indexes.Length];
            
            // Initialize command list.
            Commands = new Dictionary<ConsoleKey, Action?>
            {
                { ConsoleKey.UpArrow, null },
                { ConsoleKey.DownArrow, null },
                { ConsoleKey.LeftArrow, null },
                { ConsoleKey.RightArrow, null },
                { ConsoleKey.Z, null },
                { ConsoleKey.X, null }
            };
        }

        public void Initialize()
        {
            var count = GameObjects.Count;
            for (var i = 0; i < count; i++)
            {
                GameObjects[i].Awake();
            }

            for (var i = 0; i < count; i++)
            {
                GameObjects[i].Enable();
            }

            for (var i = 0; i < count; i++)
            {
                GameObjects[i].Start();
            }
        }

        public Iterable GetNextIterator(int index)
        {
            return Iterators[index];
        }

        public void Execute(ConsoleKey key)
        {
            if (Commands.TryGetValue(key, out var action))
            {
                action?.Invoke();
            }
        }

        public virtual void Start()
        {
            IsReset = false;
        }

        public virtual void Render()
        {
            Layout.SetWindowBorder();
            
            OutputStream.Render();
        }
        
        public virtual void Reset()
        {
            IsReset = true;
        }
        
        public List<MonoBehaviour> GameObjects { get; }

        public Dictionary<ConsoleKey, Action?> Commands { get; }

        public Iterable[] Iterators { get; set; }
        
        public bool IsReset { get; set; }
    }
}