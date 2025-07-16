using RtanRPG.Object.Scene;

namespace RtanRPG
{
    public class Application
    {
        private SceneManager _sceneManager;

        private int _index;
        
        public Application()
        {
            _sceneManager = new SceneManager();
        }

        public void Play()
        {
            while (true)
            {
                _sceneManager.Load(_index);
            }
        }
    }
}