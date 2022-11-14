using System.Collections.Generic;

namespace Utils
{
    public static class Lists
    {
        public static List<T> Of<T>(params T[] items)
        {
            return new List<T>(items);
        }
    }
}