namespace RtanRPG.Object.Scene
{
    public class SceneManager
    {
        private readonly BaseScene[] _scenes;

        private BaseScene? _currentScene;
        
        public SceneManager()
        {
            //TODO: You must fix this code.
            var length = 5;
            _scenes = new BaseScene[length];
            for (var i = 0; i < length; i++)
            {
                //TODO: You must fix this code.
                _scenes[i] = SceneFactory.Create(i);
            }
            
            //TODO: You must implement scene connection code.
            
            _currentScene = null;
        }

        public void Load(int index)
        {
            if (_currentScene == null)
            {
                _currentScene = _scenes[index];
                _currentScene.Render();

                return;
            }
            
            _currentScene.Clear();

            if (_currentScene.Iterators[index] is BaseScene nextScene)
            {
                _currentScene = nextScene;
            }
            
            _currentScene.Render();
        }
    }
}