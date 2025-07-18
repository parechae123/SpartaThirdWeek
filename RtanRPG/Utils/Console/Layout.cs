namespace RtanRPG.Utils.Console
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