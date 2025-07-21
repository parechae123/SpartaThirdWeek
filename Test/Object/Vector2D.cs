namespace Test.Object
{
    [Serializable]
    public struct Vector2D : ICloneable, IEquatable<Vector2D>
    {
        public Vector2D()
        {
            Left = Top = 0;
        }

        public Vector2D(int left, int top)
        {
            Left = left;
            Top = top;
        }

        public Vector2D(Vector2D other)
        {
            Left = other.Left;
            Top = other.Top;
        }

        public object Clone()
        {
            return MemberwiseClone();
        }

        public readonly override bool Equals(object? obj)
        {
            return obj is Vector2D other && Equals(other);
        }

        public readonly bool Equals(Vector2D other)
        {
            return Left == other.Left && Top == other.Top;
        }

        public readonly override int GetHashCode()
        {
            return HashCode.Combine(Left, Top);
        }

        public static bool operator ==(Vector2D lhs, Vector2D rhs)
        {
            return lhs.Equals(rhs);
        }

        public static bool operator !=(Vector2D lhs, Vector2D rhs)
        {
            return !(lhs == rhs);
        }

        public static Vector2D operator +(Vector2D lhs, Vector2D rhs)
        {
            return new Vector2D(lhs.Left + rhs.Left, lhs.Top + rhs.Top);
        }

        public static Vector2D operator -(Vector2D lhs, Vector2D rhs)
        {
            return new Vector2D(lhs.Left - rhs.Left, lhs.Top - rhs.Top);
        }

        public int Left { get; set; }

        public int Top { get; set; }
    }
}