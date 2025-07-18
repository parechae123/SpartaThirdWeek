using RtanRPG.Object.Scene;

namespace RtanRPG.Utils
{
    public class SceneManager : Singleton<SceneManager>
    {
        private BaseScene[]? _scenes;

        private BaseScene? _current;

        protected override void Initialize()
        {
            var length = DataManager.Instance.SceneData.Length;
            
            // Create scene instance references.
            _scenes = new BaseScene[length];
            for (var i = 0; i < length; i++)
            {
                _scenes[i] = SceneFactory.Create(i);
            }

            // Create a graph by connecting scenes.
            for (var i = 0; i < length; i++)
            {
                var indexes = DataManager.Instance.SceneData[i].Indexes;
                for (var j = 0; j < indexes.Length; j++)
                {
                    _scenes[i].Iterators[j] = _scenes[indexes[j]];
                }
            }

            _current = null;
        }

        public void Load(int index)
        {
            _current?.Reset();
            
            _current = _scenes?[index];
            
            _current?.Start();
        }

        public void Execute(ConsoleKey key)
        {
            _current?.Execute(key);
        }
    }
}