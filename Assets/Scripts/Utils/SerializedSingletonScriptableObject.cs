using Sirenix.OdinInspector;
using UnityEngine;

namespace Utils
{
    public abstract class SerializedSingletonScriptableObject<T> : SerializedScriptableObject
        where T : SerializedScriptableObject
    {
        private static T _instance;

        public static T I
        {
            get
            {
                if (_instance is not null) return _instance;
                
                return _instance = Resources.Load<T>(typeof(T).ToString());
            }
        }
    }
}