using System.Collections.Generic;

namespace Utils
{
    public static class Sets
    {
        public static HashSet<T> Of<T>(params T[] items)
        {
            return new HashSet<T>(items);
        }
    }
}