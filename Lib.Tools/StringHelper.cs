using System;
using System.Linq;

namespace Lib.Tools
{
    public static class StringHelper
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }

        public static string ConvertToReadableString(this string value)
        {
            if (value.IsNullOrEmpty())
                return value;

            return string.Concat(value.Select(v => char.IsUpper(v) ? " " + v : v.ToString())).TrimStart(' ');
        }
    }
}