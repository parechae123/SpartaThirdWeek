using System.Numerics;

namespace RtanRPG.Object
{
    public class Transform
    {
        private Vector2D _begin;
        private Vector2D _end;

        public static Transform operator +(Transform lhs, Vector2D rhs)
        {
            lhs._begin += rhs;
            lhs._end += rhs;

            return lhs;
        }

        public static Transform operator -(Transform lhs, Vector2D rhs)
        {
            lhs._begin -= rhs;
            lhs._end -= rhs;

            return lhs;
        }
    }
}