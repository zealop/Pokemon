using UnityEngine;

namespace Game.Utils
{
    public static class AnyObjectExtension
    {
        public static AnyObject ToAnyObject(this object o)
        {
            return JsonUtility.FromJson<AnyObject>(JsonUtility.ToJson(o));
        }
    }
}