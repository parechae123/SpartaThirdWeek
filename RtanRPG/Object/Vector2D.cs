using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Object
{
    public struct Vector2D : ICloneable, IEquatable<Vector2D>
    {
        public int Left { get; }
        public int Top { get; }

        public Vector2D(int left, int top)
        {
            Left = left;
            Top = top;
        }

        public Vector2D(Vector2D other)
        {
            if (other == null) throw new ArgumentNullException(nameof(other));
            Left = other.Left;
            Top = other.Top;
        }

        public object Clone()
        {
            return new Vector2D(this);
        }


        public bool Equals(Vector2D other)
        {
            if (other == null) return false;
            return Left == other.Left && Top == other.Top;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Left, Top);
        }

        public static Vector2D operator +(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.Left + b.Left, a.Top + b.Top);
        }

        public static Vector2D operator -(Vector2D a, Vector2D b)
        {
            return new Vector2D(a.Left - b.Left, a.Top - b.Top);
        }

        public static bool operator ==(Vector2D a, Vector2D b)
        {
            if (ReferenceEquals(a, b)) return true;
            return a.Equals(b);
        }

        public static bool operator !=(Vector2D a, Vector2D b)
        {
            return !(a == b);
        }
    }
}
