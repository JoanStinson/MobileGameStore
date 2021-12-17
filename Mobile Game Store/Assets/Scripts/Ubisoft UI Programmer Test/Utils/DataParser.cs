using System;

namespace Ubisoft.UIProgrammerTest.Utils
{
    public static class DataParser
    {
        public static bool EnumTryParse<T>(string value, bool ignoreCase, out T result) where T : struct
        {
            try
            {
                result = (T)Enum.Parse(typeof(T), value, ignoreCase);
                return true;
            }
            catch
            {
                result = default(T);
                return false;
            }
        }
    }
}