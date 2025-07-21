namespace Test.Utils
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private static T? _instance;

        /// <summary>
        /// Initializes an instance of the singleton pattern.
        /// </summary>
        protected virtual void Initialize() { }

        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }
                
                _instance = new T();
                _instance.Initialize();

                return _instance;
            }
        }
    }
}