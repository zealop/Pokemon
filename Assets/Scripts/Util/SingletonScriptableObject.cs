using UnityEngine;

namespace Util
{
    public abstract class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
    {
        private static T _instance;

        public static T I
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<T>(typeof(T).ToString());
                    (_instance as SingletonScriptableObject<T>)?.OnInitialize();
                }

                return _instance;
            }
        }

        protected virtual void OnInitialize()
        {
        }
    }
}