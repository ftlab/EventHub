using System;

namespace EventHub.Core
{
    public static class Guard
    {
        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ArgumentNotNull<T>(this T arg, string name)
        {
            if (arg == null) throw new ArgumentNullException(typeof(T).Name + ": " + name);
            return arg;
        }
    }
}
