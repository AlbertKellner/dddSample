using System;

namespace DDD.Sample.Foundation
{
    public static class Helper
    {
        public static Lazy<T> AsLazy<T>(this T objectValue)
        {
            return new Lazy<T>(() => objectValue);
        }

        public static Lazy<T> AsLazy<T>(Func<T> method)
        {
            return new Lazy<T>(method);
        }
    }
}
