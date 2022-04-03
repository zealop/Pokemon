using Sirenix.OdinInspector;
using UnityEngine;

namespace Util
{
    public abstract class SerializedSingletonScriptableObject<T> : SerializedScriptableObject
        where T : SerializedScriptableObject
    {
        private static T _instance;

        public static T I
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<T>(typeof(T).ToString());
                    (_instance as SerializedSingletonScriptableObject<T>)?.OnInitialize();
                }

                return _instance;
            }
        }

        protected virtual void OnInitialize()
        {
        }
    }
}