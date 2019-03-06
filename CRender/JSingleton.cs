using System;

namespace CRender
{
    public abstract class JSingleton<T> where T : JSingleton<T>, new()
    {
        public static T Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new T();
                return _instance;
            }
        }

        private static T _instance;

        public JSingleton()
        {
            _instance = _instance == null ? this as T : throw new Exception($"Singleton {GetType()} has existed");
        }
    }
}
