namespace RtanRPG.Object
{
    public class MonoBehaviour : IRenderable
    {
        protected Transform Transform;

        protected MonoBehaviour()
        {
            Transform = new Transform();
        }

        public void Awake()
        {

        }

        public void Enable()
        {

        }

        public void Start()
        {

        }

        public void Update()
        {

        }

        public void Render()
        {

        }

        public void Disable()
        {

        }

        public void Destroy()
        {

        }
        
        public bool IsEnabled { get; set; }
    }
}