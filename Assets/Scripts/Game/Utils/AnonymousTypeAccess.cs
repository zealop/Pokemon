namespace Game.Utils
{
    public static class AnonymousTypeAccess
    {
        public static object GetAnonymousProp(this object o, string propName)
        {
            return o?.GetType().GetProperty(propName)?.GetValue(o, null);
        }

        public static bool HasMethod(this object o, string methodName)
        {
            return o?.GetType().GetMethod(methodName) != null;
        }
    }
}