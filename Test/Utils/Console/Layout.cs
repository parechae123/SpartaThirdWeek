using Test.Object;

namespace Test.Utils.Console
{
    public static class Layout
    {
        public const int DefaultLeftMargin = 2;
        public const int DefaultTopMargin = 2;
        
        public static void SetWindowBorder()
        {
            for (var i = 0; i < DefaultHeight - 1; i++)
            {
                OutputStream.Buffer[i][0] = Border.Bold.VerticalLine;
                OutputStream.Buffer[i][DefaultWidth - 1] = Border.Bold.VerticalLine;
            }

            for (var i = 0; i < DefaultWidth; i++)
            {
                OutputStream.Buffer[0][i] = Border.Bold.HorizontalLine;
                OutputStream.Buffer[DefaultHeight - 2][i] = Border.Bold.HorizontalLine;
            }

            OutputStream.Buffer[0][0] = Border.Bold.LeftTopEdge;
            OutputStream.Buffer[0][DefaultWidth - 1] = Border.Bold.RightTopEdge;
            OutputStream.Buffer[DefaultHeight - 2][0] = Border.Bold.LeftBottomEdge;
            OutputStream.Buffer[DefaultHeight - 2][DefaultWidth - 1] = Border.Bold.RightBottomEdge;
        }

        public static void SetDialog(string name, string content)
        {
            var width = DefaultWidth - 2;
            
            var top = MaximumContentHeight - 7;
            var bottom = MaximumContentHeight - 1;
            var right = DefaultWidth -3;
            
            // Set dialog border.
            for (var i = 0; i < 6; i++)
            {
                OutputStream.Buffer[top + i][DefaultLeftMargin] = Border.Normal.VerticalLine;
                OutputStream.Buffer[top + i][right] = Border.Normal.VerticalLine;
            }

            for (var i = 2; i < width; i++)
            {
                OutputStream.Buffer[top][i] = Border.Normal.HorizontalLine;
                OutputStream.Buffer[bottom][i] = Border.Normal.HorizontalLine;
            }

            OutputStream.Buffer[top][DefaultLeftMargin] = Border.Normal.LeftTopEdge;
            OutputStream.Buffer[top][right] = Border.Normal.RightTopEdge;
            OutputStream.Buffer[bottom][DefaultLeftMargin] = Border.Normal.LeftBottomEdge;
            OutputStream.Buffer[bottom][right] = Border.Normal.RightBottomEdge;
            
            OutputStream.WriteBuffer(name, new Vector2D(4, MaximumContentHeight - 7));
            OutputStream.WriteBuffer(content, new Vector2D(4, MaximumContentHeight - 5));
        }
        
        public static void Log(string message, Status status)
        {
            System.Console.Write("[");

            switch (status)
            {
                case Status.Fail:
                    System.Console.ForegroundColor = ConsoleColor.Red;
                    System.Console.Write("FAIL");
                    break;
                case Status.Done:
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.Write("DONE");
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Status));
            }
            
            System.Console.ResetColor();
            System.Console.WriteLine($"] {message}");

            Thread.Sleep(100);
        }

        public enum Status
        {
            Fail, Done
        }
        
        public static int DefaultWidth { get; set; }

        public static int DefaultHeight { get; set; }

        public static int MaximumContentWidth => DefaultWidth - 2;

        public static int MaximumContentHeight => DefaultHeight - 2;
    }
}