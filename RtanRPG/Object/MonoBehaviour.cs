
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RtanRPG.Object
{
    public abstract class MonoBehaviour : IRenderable
    {
        protected char[][] Image;
        protected Transform Transform;

        public virtual void Awake() { }
        public abstract void Enable();
        public abstract void Start();
        public abstract void Update();
        public virtual void Render()
        {
            // 기본 구현 예시
            Console.WriteLine("Rendering...");
        }
        public abstract void Disable();
        public abstract void Destroy();

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }
    }
}