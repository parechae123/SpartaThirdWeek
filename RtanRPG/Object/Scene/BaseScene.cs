namespace RtanRPG.Object.Scene
{
    public class BaseScene : Iterable, IRenderable
    {
        public Iterable GetNextIterator(int index)
        {
            return Iterators[index];
        }

        public virtual void Render()
        {
            
        }

        public virtual void Clear()
        {
            
        }

        public Iterable[] Iterators { get; }
    }
}