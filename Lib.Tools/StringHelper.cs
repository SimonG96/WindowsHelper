using System;

namespace Lib.Tools
{
    public static class StringHelper
    {
        public static bool IsNullOrEmpty(this string value)
        {
            return String.IsNullOrEmpty(value);
        }
    }
}