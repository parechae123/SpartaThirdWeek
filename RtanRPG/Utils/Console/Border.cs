namespace Test.Utils.Console
{
    public struct Border
    {
        public struct Normal
        {
            public const char VerticalLine = '│';
            public const char HorizontalLine = '─';

            public const char LeftTopEdge = '┌';
            public const char RightTopEdge = '┐';
            public const char RightBottomEdge = '┘';
            public const char LeftBottomEdge = '└';

            public const char LeftCross = '├';
            public const char RightCross = '┤';
            public const char TopCross = '┬';
            public const char BottomCross = '┴';
            public const char CenterCross = '┼';
        }

        public struct Bold
        {
            public const char VerticalLine = '┃';
            public const char HorizontalLine = '━';

            public const char LeftTopEdge = '┏';
            public const char RightTopEdge = '┓';
            public const char RightBottomEdge = '┛';
            public const char LeftBottomEdge = '┗';

            public const char LeftCross = '┣';
            public const char RightCross = '┫';
            public const char TopCross = '┳';
            public const char BottomCross = '┻';
            public const char CenterCross = '╋';
        }
    }
}