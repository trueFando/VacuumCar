using System;

namespace Extensions
{
    public static class EnumExtensions
    {
        public static T Next<T>(this T src) where T : struct, Enum
        {
            var values = (T[])Enum.GetValues(typeof(T));
            var index = Array.IndexOf(values, src) + 1;
            return (index == values.Length) ? values[0] : values[index];
        }
    }
}