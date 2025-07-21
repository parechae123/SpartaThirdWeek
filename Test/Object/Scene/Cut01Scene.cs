using Test.Utils;
using Test.Utils.Console;

namespace Test.Object.Scene
{
    public class Cut01Scene : BaseScene
    {
        private int _index;
        
        private readonly SpriteRenderer[] _renderers;

        private readonly Vector2D _begin = new (12, 2);
        private readonly Vector2D _end = new (Layout.DefaultWidth - 12, Layout.MaximumContentHeight - 9);
        
        public Cut01Scene(int index) : base(index)
        {
            var width = _end.Left -  _begin.Left;
            var height = _end.Top - _begin.Top;
            
            _renderers = new SpriteRenderer[4];
            _renderers[0] = new SpriteRenderer(DataManager.GetSpriteFilePath("cut_sprite_01"));
            _renderers[0].Prepare(width, height);
            _renderers[1] = new SpriteRenderer(DataManager.GetSpriteFilePath("cut_sprite_02"));
            _renderers[1].Prepare(width, height);

            Commands[ConsoleKey.Z] = ToNext;
        }
        
        public override void Start()
        {
            base.Start();

            while (IsUnloaded == false)
            {
                if (_index > 4)
                {
                    SceneManager.Instance.Index = 3;
                    IsUnloaded = true;
                }
                
                Render();
            }
        }

        public override void Render()
        {
            string[] frame;
            switch (_index)
            {
                case 0:
                    Layout.SetDialog("[ Nia ] ", "Narration 01 ");
                    break;
                case 1:
                    frame = _renderers[0].GetFrame();
                    OutputStream.WriteBuffer(frame, _begin, _end);
                    Layout.SetDialog("[ Nia ] ", "Narration 02 ");
                    break;
                case 2:
                    frame = _renderers[0].GetFrame();
                    OutputStream.WriteBuffer(frame, _begin, _end);
                    Layout.SetDialog("[ Nia ] ", "Narration 03 ");
                    break;
                case 3:
                    frame = _renderers[1].GetFrame();
                    OutputStream.WriteBuffer(frame, _begin, _end);
                    Layout.SetDialog("[ Nia ] ", "Narration 04 ");
                    break;
                case 4:
                    frame = _renderers[1].GetFrame();
                    OutputStream.WriteBuffer(frame, _begin, _end);
                    Layout.SetDialog("[ Nia ] ", "Narration 05 ");
                    break;
            }
            
            base.Render();
        }

        private void ToNext()
        {
            _index++;
        }
    }
}
