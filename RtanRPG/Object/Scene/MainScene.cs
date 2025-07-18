using RtanRPG.Utils;
using RtanRPG.Utils.Console;
using RtanRPG.Utils.Extension;

namespace RtanRPG.Object.Scene
{
    public class MainScene : BaseScene
    {
        private int _index;
        
        private readonly VideoRenderer _video;
        private readonly Vector2D _begin = new Vector2D(1, 1);
        private readonly Vector2D _end = new Vector2D(Layout.MaximumContentWidth, Layout.MaximumContentHeight);

        private readonly string[] _menus = { "> Start ", "Start ", "Finish ", "> Finish " };
        
        public MainScene(int index) : base(index)
        {
            _index = 0;
            
            Commands[ConsoleKey.UpArrow] = SelectUpperMenu;
            Commands[ConsoleKey.DownArrow] = SelectLowerMenu;
            Commands[ConsoleKey.Z] = SelectMenu;

            // Create and prepare a video.
            _video = new VideoRenderer(DataManager.GetVideoFilePath("Introduction"));
            _video.Prepare(_begin, _end);
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
            OutputStream.WriteBuffer(_video.GetNextFrame(), _begin, _end);
                
            OutputStream.WriteBuffer(_menus[_index], new Vector2D(Layout.MaximumContentWidth / 2 - _menus[_index].GetGraphicLength() / 2 + _index - 1, Layout.MaximumContentHeight - 7));
            OutputStream.WriteBuffer(_menus[2 + _index], new Vector2D(Layout.MaximumContentWidth / 2 - _menus[2 + _index].GetGraphicLength() / 2 - _index, Layout.MaximumContentHeight - 5));
            
            base.Render();
        }

        private void SelectUpperMenu()
        {
            if (_index == 1)
            {
                _index--;
            }
        }

        private void SelectLowerMenu()
        {
            if (0 == _index)
            {
                _index++;
            }
        }

        private void SelectMenu()
        {
            switch (_index)
            {
                case 0:
                    SceneManager.Instance.Load(1);
                    break;
                case 1:
                    Environment.Exit(0);
                    break;
            }
        }
    }
}