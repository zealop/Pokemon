using System.Collections.Generic;

namespace Utils
{
    public static class Lists
    {
        public static List<T> Of<T>(T item)
        {
            return new List<T> { item };
        }
    }
}