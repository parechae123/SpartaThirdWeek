namespace RtanRPG.Object.Scene
{
    public class MainScene : BaseScene
    {
        private int _index;
    
        
        public MainScene(int index) : base(index)
        {
            _index = 0;
        
            
            Commands[ConsoleKey.UpArrow] = SelectUpperMenu;
            Commands[ConsoleKey.DownArrow] = SelectLowerMenu;
            Commands[ConsoleKey.Z] = SelectMenu;
        }

        public override void Start()
        {
            base.Start();
            
            while (IsReset == false)
            {
                Render();
            }
        }

        public override void Render()
        {
            base.Render();
        }

        private void SelectUpperMenu()
        {
            if (_index < 1)
            {
                _index++;
            }
        }

        private void SelectLowerMenu()
        {
            if (0 < _index)
            {
                _index--;
            }
        }

        private void SelectMenu()
        {            
            switch (_index)
            {
                case 0:
                    // SceneManager.Instance.Load(1);
                    break;
                case 1:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}