namespace Test.Object
{
    public class MonoBehaviour : IRenderable
    {
        protected Transform Transform;

        protected MonoBehaviour()
        {
            Transform = new Transform();
        }

        public virtual void Awake()
        {

        }

        public virtual void Enable()
        {

        }

        public virtual void Start()
        {

        }

        public virtual void Update()
        {

        }

        public virtual void Render()
        {

        }

        public virtual void Disable()
        {

        }

        public virtual void Destroy()
        {

        }
        
        public bool IsEnabled { get; set; }
    }
}