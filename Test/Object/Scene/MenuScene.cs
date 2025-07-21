using Test.Utils;
using Test.Utils.Console;

namespace Test.Object.Scene
{
    public class MenuScene : BaseScene
    {
        private const string MenuName = "[ STATUS ] ";
        
        private readonly string[] _subjects = 
            { "[  NAME ]", "[    LV ]", "[ CLASS ]", "[    HP ]", "[   ATK ]", "[   DEF ]" };
        
        private readonly SpriteRenderer _renderer;
        
        public MenuScene(int index) : base(index)
        {
            var width = (Layout.DefaultWidth - 6) / 2;
            var height = Layout.MaximumContentHeight - 8;
            _renderer = new SpriteRenderer(DataManager.GetSpriteFilePath("Profile"));
            _renderer.Prepare(width, height);
        }

        public override void Start()
        {
            base.Start();

            while (IsUnloaded == false)
            {
                Render();
            }
        }
        
        public override void Render()
        {
            SetMenu();
            SetContent();
            
            base.Render();
        }

        private void SetMenu()
        {
            for (var i = 2; i < 5; i++)
            {
                OutputStream.Buffer[i][2] = Border.Normal.VerticalLine;
                OutputStream.Buffer[i][Layout.DefaultWidth - 3] = Border.Normal.VerticalLine;
            }
            
            for (var i = 3; i < Layout.DefaultWidth - 3; i++)
            {
                OutputStream.Buffer[1][i] = Border.Normal.HorizontalLine;
                OutputStream.Buffer[5][i] = Border.Normal.HorizontalLine;
            }
            
            OutputStream.Buffer[1][2] = Border.Normal.LeftTopEdge;
            OutputStream.Buffer[1][Layout.DefaultWidth - 3] = Border.Normal.RightTopEdge;
            OutputStream.Buffer[5][2] = Border.Normal.LeftBottomEdge;
            OutputStream.Buffer[5][Layout.DefaultWidth - 3] = Border.Normal.RightBottomEdge;
            
            var width = Layout.DefaultWidth / 2 - 5;
            OutputStream.WriteBuffer(MenuName, new Vector2D(width, 3));
        }

        private void SetContent()
        {
            for (var i = 6; i < Layout.MaximumContentHeight; i++)
            {
                OutputStream.Buffer[i][2] = Border.Normal.VerticalLine;
                OutputStream.Buffer[i][Layout.DefaultWidth - 3] = Border.Normal.VerticalLine;
            }

            for (var i = 3; i < Layout.DefaultWidth - 3; i++)
            {
                OutputStream.Buffer[Layout.MaximumContentHeight - 1][i] = Border.Normal.HorizontalLine;
                OutputStream.Buffer[6][i] = Border.Normal.HorizontalLine;
            }

            OutputStream.Buffer[6][2] = Border.Normal.LeftTopEdge;
            OutputStream.Buffer[6][Layout.DefaultWidth - 3] = Border.Normal.RightTopEdge;
            OutputStream.Buffer[Layout.MaximumContentHeight - 1][2] = Border.Normal.LeftBottomEdge;
            OutputStream.Buffer[Layout.MaximumContentHeight - 1][Layout.DefaultWidth - 3] = Border.Normal.RightBottomEdge;

            var width = (Layout.DefaultWidth - 2) / 2;
            var height = Layout.MaximumContentHeight - 1;
            
            // Set profile image frame.
            var frame = _renderer.GetFrame();
            var begin = new Vector2D(4, 7);
            var end = new Vector2D(width, Layout.MaximumContentHeight - 2);
            OutputStream.WriteBuffer(frame, begin, end);

            // Set separation line.
            for (var i = 7; i < Layout.MaximumContentHeight - 1; i++)
            {
                OutputStream.Buffer[i][width] = Border.Normal.VerticalLine;
            }
            OutputStream.Buffer[6][width] = Border.Normal.TopCross;
            OutputStream.Buffer[height][width] = Border.Normal.BottomCross;
            
            // Set player character status information.
            var data = DataManager.Instance.PlayerData;
            if (data == null)
            {
                return;
            }
            
            OutputStream.WriteBuffer($"{_subjects[0]} {data.Name} ", new Vector2D(width + 3, 8));
            OutputStream.WriteBuffer($"{_subjects[1]} {data.Level} ", new Vector2D(width + 3, 10));
            OutputStream.WriteBuffer($"{_subjects[2]} {data.Class} ", new Vector2D(width + 3, 12));
            OutputStream.WriteBuffer($"{_subjects[3]} {data.HealthPoint} ", new Vector2D(width + 3, 14));
            OutputStream.WriteBuffer($"{_subjects[4]} {data.AttackPoint} ", new Vector2D(width + 3, 16));
            OutputStream.WriteBuffer($"{_subjects[5]} {data.DefensePoint} ", new Vector2D(width + 3, 18));
        }
    }
}