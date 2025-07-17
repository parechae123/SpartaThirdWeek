using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Object
{
    public class Transform
    {
        private Vector2D _begin;
        private Vector2D _end;

        public int Width => _end.Left - _begin.Left;
        public int Height => _end.Top - _begin.Top;

        public Transform(Vector2D begin, Vector2D end)
        {
            _begin = begin;
            _end = end;
        }
    }
}
